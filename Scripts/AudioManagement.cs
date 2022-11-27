using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using Random=UnityEngine.Random;

public class AudioManagement : MonoBehaviour
{
    private AudioSource AudioSource { get; set; } = null;
    private Dictionary<string, AudioClip> AudioClips { get; set; } = new Dictionary<string, AudioClip>();
    [field: SerializeField] private bool LoadMusic { get; set; } = false;
    [field: SerializeField] private bool LoadSounds { get; set; } = false;
    [field: SerializeField] private bool LoadVoiceLines { get; set; } = false;

    protected void Awake()
    {
        if ((AudioSource = this.gameObject.GetComponent<AudioSource>()) is null)
        {
            Debug.LogError(
                "ERROR: <AudioManagement> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioSource component."
                );
            Application.Quit(1);
        }
        
        List<string> audioPaths = new List<string>();
        if (LoadMusic)
        {
            audioPaths.Add("Audio/Music");
        }
        if (LoadSounds)
        {
            audioPaths.Add("Audio/Sounds");
        }
        if (LoadVoiceLines)
        {
            audioPaths.Add("Audio/VoiceLines");
        }

        foreach (string audioPath in audioPaths)
        {
            AudioClip[] audioClips = Resources.LoadAll<AudioClip>(audioPath);
            if (audioClips.Length == 0)
            {
                Debug.LogError("ERROR: <AudioManagement> - audio path: " + audioPath + " was not loaded");
                Application.Quit(1);
            }

            foreach (var audioClip in audioClips)
            {
                AudioClips.Add(audioClip.name, audioClip);
            }
        }
        
        AudioSource.loop = false;
    }

    private void Start()
    {
        MainAudioManagement.AddAudioManagement(this);
    }

    public void RemoveFromMainAudioManagement()
    {
        MainAudioManagement.RemoveAudioManagement(this);
    }

    public bool IsPlaying()
    {
        return AudioSource.isPlaying;
    }

    public void SetAudioClip(AudioClip audioClip)
    {
        AudioSource.clip = audioClip;
    }
    
    public void Play()
    {
        AudioSource.Play();
    }

    public void Stop()
    {
        AudioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        if (volume < 0f || volume > 1f)
        {
            Debug.LogWarning("ERROR: <AudioManagement> - volume value is out of range.");
            return;
        }
        
        AudioSource.volume = volume;
    }

    public void SetMute(bool mute)
    {
        AudioSource.mute = mute;
    }
    
    public void SetPause(bool pause)
    {
        if (pause)
        {
            AudioSource.Pause();
        }
        else
        {
            AudioSource.UnPause();
        }
    }

    public void SetLoop(bool loop)
    {
        AudioSource.loop = loop;
    }
    
    public void PlayClipAtPoint(string audioClipName, Vector3 pointInSpace)
    {
        AudioClip audioClip;
        if (!AudioClips.TryGetValue(audioClipName, out audioClip)) {
            Debug.LogWarning(
                "WARNING: <AudioManagement> - " + audioClipName + " key was not found in AudioClipsGroups."
                );
            return;
        }

        GameObject audioGameObject = new GameObject("TemporaryAudioSource", typeof(AudioSource));
        audioGameObject.transform.position = pointInSpace;
        AudioSource audioSource = audioGameObject.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = false;
        audioSource.volume = AudioSource.volume;
        audioSource.spatialBlend = 1f;
        audioSource.dopplerLevel = 0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 12f;
        audioSource.maxDistance = 15f;
        
        audioSource.Play();
        Destroy(audioGameObject, audioClip.length);
    }

    public void Play(string audioClipName, bool loop)
    {
        AudioClip audioClip;
        if (!AudioClips.TryGetValue(audioClipName, out audioClip)) {
            Debug.LogWarning(
                "WARNING: <AudioManagement> - " + audioClipName + " key was not found in AudioClipsGroups."
                );
            return;
        }

        AudioSource.clip = audioClip;
        AudioSource.loop = loop;
        AudioSource.Play();
    }

    public void PlayRandom(string audioClipGroup, bool loop)
    {
        List<AudioClip> audioClips = new List<AudioClip>();
        foreach (KeyValuePair<string, AudioClip> audioClip in AudioClips)
        {
            if (audioClip.Key.StartsWith(audioClipGroup))
            {
                audioClips.Add(audioClip.Value);
            }
        }

        if (audioClips.Count == 0)
        {
            Debug.LogWarning("WARNING: <AudioManagement> - no clips were found in " + audioClipGroup);
            return;
        }

        AudioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
        AudioSource.loop = loop;
        AudioSource.Play();
    }

    public void PlayOneShot(string audioClipName)
    {
        AudioClip audioClip;
        if (!AudioClips.TryGetValue(audioClipName, out audioClip)) {
            Debug.LogWarning(
                "WARNING: <AudioManagement> - " + audioClipName + " key was not found in AudioClipsGroups."
                );
            return;
        }
        
        AudioSource.PlayOneShot(audioClip);
    }

    public void PlayOneShotRandom(string audioClipGroup)
    {
        List<AudioClip> audioClips = new List<AudioClip>();
        foreach (KeyValuePair<string, AudioClip> audioClip in AudioClips)
        {
            if (audioClip.Key.StartsWith(audioClipGroup))
            {
                audioClips.Add(audioClip.Value);
            }
        }

        if (audioClips.Count == 0)
        {
            Debug.LogWarning("WARNING: <AudioManagement> - no clips were found in " + audioClipGroup);
            return;
        }
        
        AudioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
    }

    public void PlaySequence(string[] audioClipsSequence, bool loopLast)
    {
        if (audioClipsSequence.Length == 0)
        {
            Debug.LogWarning("WARNING: <AudioManagement> - no audio clips given.");
            return;
        }
        
        StartCoroutine(PlaySequenceCoroutine(audioClipsSequence, loopLast));
    }

    private IEnumerator PlaySequenceCoroutine(string[] audioClipsSequence, bool loopLast)
    {
        for (int index = 0; index < audioClipsSequence.Length; index++)
        {
            string audioClipName = audioClipsSequence[index];
            
            AudioClip audioClip;
            if (!AudioClips.TryGetValue(audioClipName, out audioClip)) {
                Debug.LogWarning(
                    "WARNING: <AudioManagement> - " + audioClipName + " key was not found in AudioClipsGroups."
                    );
                yield break;
            }
            
            AudioSource.clip = audioClip;
            AudioSource.loop = false;
            AudioSource.Play();
            
            if (index == audioClipsSequence.Length - 1)
            {
                AudioSource.loop = loopLast;
            }
            else
            {
                yield return new WaitForSeconds(audioClip.length);
            }
        }
    }
}
