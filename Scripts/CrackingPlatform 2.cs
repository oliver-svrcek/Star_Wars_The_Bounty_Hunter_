using System.Collections;
using UnityEngine;

public class CrackingPlatform : MonoBehaviour
{
    
    private AudioManagement AudioManagement { get; set; }
    private Animator Animator { get; set; }
    private Coroutine DestroyCoroutine { set; get; }

    private void Awake()
    {
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <CrackingPlatform> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if ((Animator = this.gameObject.GetComponent<Animator>()) is null)
        {
            Debug.LogError(
                "ERROR: <CrackingPlatform> - " + this.gameObject.transform.name + " game object is missing " +
                "Animator component."
                );
            Application.Quit(1);
        }

        DestroyCoroutine = null;
    }
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && DestroyCoroutine is null)
        {
            AudioManagement.PlayOneShot("CrackingPlatformSound");
            Animator.enabled = true;
            DestroyCoroutine = StartCoroutine(Destroy());
        }
    }
    
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManagement.RemoveFromMainAudioManagement();
        Destroy(this.gameObject);
    }
}
