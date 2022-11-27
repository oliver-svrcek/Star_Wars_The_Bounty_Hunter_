using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; }
    private BoxCollider2D BoxCollider2D { get; set; }
    private Animator Animator { get; set; }
    private void Awake()
    {
        if (ActivePlayer.PlayerData is null)
        {
            Debug.LogError("ERROR: <Checkpoint> - ActivePlayer.PlayerData is null.");
            Application.Quit(1);
        }
        
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Checkpoint> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if ((BoxCollider2D  = this.gameObject.GetComponent<BoxCollider2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <Checkpoint> - " + this.gameObject.transform.name + " game object is missing " +
                "BoxCollider2D component."
                );
            Application.Quit(1);
        }
        
        if ((Animator = this.gameObject.GetComponent<Animator>()) is null)
        {
            Debug.LogError(
                "ERROR: <Checkpoint> - " + this.gameObject.transform.name + " game object is missing " +
                "Animator component."
                );
            Application.Quit(1);
        }
    }

    private void Start()
    {
        if ((ActivePlayer.PlayerData.PositionAxisX - 1f) > this.gameObject.transform.position.x)
        {
            this.gameObject.SetActive(false);
        }
        else if (Mathf.Abs(ActivePlayer.PlayerData.PositionAxisX - this.gameObject.transform.position.x) < 1f)
        {
            Animator.enabled = true;
            BoxCollider2D.enabled = false;
        }
        else
        {
            Animator.enabled = false;
            BoxCollider2D.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManagement.PlayOneShot("CheckpointSound");
            ActivePlayer.PlayerData.PositionAxisX = this.gameObject.transform.position.x;
            ActivePlayer.PlayerData.PositionAxisY = this.gameObject.transform.position.y;
            ActivePlayer.PlayerData.UpdateData();
            Animator.enabled = true;
            BoxCollider2D.enabled = false;
        }
    }
}
