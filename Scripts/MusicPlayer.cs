using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; } = null;
    [field: SerializeField] public string ListOfAudios { get; set; } = "";
    [field: SerializeField] public bool LoopLastAudio { get; set; } = false;

    private void Awake()
    {
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <AudioPlayer> - " + this.gameObject.transform.name + " is missing AudioManagement " +
                "component."
                );
            Application.Quit(1);
        } 
    }

    private void Start()
    {
        AudioManagement.SetMute(ActivePlayer.MuteMusic);
        AudioManagement.PlaySequence(GetArrayOfAudios(), LoopLastAudio);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ActivePlayer.MuteMusic = !ActivePlayer.MuteMusic;
            AudioManagement.SetMute(ActivePlayer.MuteMusic);
        }
    }

    private string[] GetArrayOfAudios()
    {
        return JsonConvert.DeserializeObject<string[]>(ListOfAudios);
    }

    public void MuteMusic(bool mute)
    {
        ActivePlayer.MuteMusic = mute;
        AudioManagement.SetMute(mute);
    }
}
