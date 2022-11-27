using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; }
    private Rigidbody2D Rigidbody2D { get; set; }
    private GameObject DetectionArea { get; set; }
    private GameObject Spikes { get; set; }
    
    private void Awake()
    {
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <FallingPlatform> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if ((Rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <FallingPlatform> - " + this.gameObject.transform.name + " game object is missing " +
                "Rigidbody2D component."
                );
            Application.Quit(1);
        }
        
        if (this.gameObject.transform.Find("DetectionArea") is null)
        {
            Debug.LogError(
                "ERROR: <FallingPlatform> - " + this.gameObject.transform.name + "/DetectionArea game object " +
                "was not found in game object hierarchy."
            );
            Application.Quit(1);
        }
        DetectionArea = this.gameObject.transform.Find("DetectionArea").gameObject;
        
        if (this.gameObject.transform.Find("SpikesLong") is null)
        {
            Debug.LogError(
                "ERROR: <FallingPlatform> - " + this.gameObject.transform.name + "/SpikesLong game object " +
                "was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        Spikes = this.gameObject.transform.Find("SpikesLong").gameObject;
    }
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DetectionArea.SetActive(false);
            AudioManagement.PlayOneShot("FallingPlatformSound");
            Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            Spikes.SetActive(true);
        }
    }
}
