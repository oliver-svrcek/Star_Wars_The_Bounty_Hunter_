using System.Collections;
using UnityEngine;

public class BlastDoors : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; }
    private Animator Animator { get; set; }
    private BoxCollider2D BoxCollider2D { get; set; }
    private EdgeCollider2D EdgeCollider2D { get; set; }
    private Coroutine OpenCloseRepeatCoroutine { get; set; }
    [field: SerializeField] private bool RepeatOpenClose { get; set; } = false;
    [field: SerializeField] private float OpenTime { get; set; } = 5f;
    [field: SerializeField] private float CloseTime { get; set; } = 2f;

    private void Awake()
    {
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <BlastDoors> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if ((Animator = this.gameObject.GetComponent<Animator>()) is null)
        {
            Debug.LogError(
                "ERROR: <BlastDoors> - " + this.gameObject.transform.name + " game object is missing " +
                "Animator component."
                );
            Application.Quit(1);
        }
        
        if ((BoxCollider2D = this.gameObject.GetComponent<BoxCollider2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <BlastDoors> - " + this.gameObject.transform.name + " game object is missing " +
                "BoxCollider2D component."
                );
            Application.Quit(1);
        }
        
        if ((EdgeCollider2D = this.gameObject.GetComponent<EdgeCollider2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <BlastDoors> - " + this.gameObject.transform.name + " game object is missing " +
                "EdgeCollider2D component."
                );
            Application.Quit(1);
        }
    }

    private void Start()
    {
        OpenCloseRepeatCoroutine = null;
        
        if (RepeatOpenClose)
        {
            OpenCloseRepeatCoroutine = StartCoroutine(OpenCloseRepeat());
        }
    }

    private IEnumerator OpenCloseRepeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(CloseTime);
            StartCoroutine(Close());
            
            yield return new WaitForSeconds(OpenTime);
            StartCoroutine(Open());
        }
    }

    private IEnumerator Open()
    {
        AudioManagement.PlayOneShot("BlastDoorsSound");
        Animator.SetBool("IsOpening", true);
        Animator.SetBool("IsClosing", false);
        
        yield break;
    }

    private IEnumerator Close()
    {
        AudioManagement.PlayOneShot("BlastDoorsSound");
        Animator.SetBool("IsOpening", false);
        Animator.SetBool("IsClosing", true);
        
        yield break;
    }

    public void EnableDoorsCollider()
    {
        BoxCollider2D.enabled = true;
    }

    public void DisableDoorsCollider()
    { 
        BoxCollider2D.enabled = false;
    }
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (OpenCloseRepeatCoroutine is not null)
            {
                StopCoroutine(OpenCloseRepeatCoroutine);
            }
            
            StartCoroutine(Close());

            EdgeCollider2D.enabled = false;
        }
    }
}
