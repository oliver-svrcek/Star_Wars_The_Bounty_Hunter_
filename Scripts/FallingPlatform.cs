using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; } = null;
    private Rigidbody2D Rigidbody2D { get; set; } = null;
    private GameObject DetectionAreaGameObject { get; set; } = null;
    private GameObject SpikesGameObject { get; set; } = null;
    
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
        DetectionAreaGameObject = this.gameObject.transform.Find("DetectionArea").gameObject;
        
        if (this.gameObject.transform.Find("SpikesLong") is null)
        {
            Debug.LogError(
                "ERROR: <FallingPlatform> - " + this.gameObject.transform.name + "/SpikesLong game object " +
                "was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        SpikesGameObject = this.gameObject.transform.Find("SpikesLong").gameObject;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DetectionAreaGameObject.SetActive(false);
            AudioManagement.PlayOneShot("FallingPlatformSound");
            Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            SpikesGameObject.SetActive(true);
        }
    }
}
