using UnityEngine;

public class Box : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; }
    private GameObject CoinGameObject { get; set; }
    private GameObject LevelEndGameObject { get; set; }

    private void Awake()
    {
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError("ERROR: <Box> - Box game object is missing AudioManagement component.");
            Application.Quit(1);
        }

        if (GameObject.Find("Coin (1)") is null)
        {
            Debug.LogError("ERROR: <Box> - Coin (1) game object was not found in the game object hierarchy.");
            Application.Quit(1);
        }
        if ((CoinGameObject = GameObject.Find("Coin (1)")) is null)
        {
            Debug.LogError("ERROR: <Box> - Coin (1) game object is missing Coin component.");
            Application.Quit(1);
        }
        
        if ((LevelEndGameObject = GameObject.Find("LevelEnd")) is null)
        {
            Debug.LogError("ERROR: <Box> - LevelEnd game object was not found in the game object hierarchy.");
            Application.Quit(1);
        }
    }

    private void Start()
    {
        LevelEndGameObject.SetActive(false);
        CoinGameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            LevelEndGameObject.SetActive(true);
            CoinGameObject.SetActive(true);
            AudioManagement.PlayClipAtPoint("CoinSpawnSound", this.gameObject.transform.position);
            AudioManagement.RemoveFromMainAudioManagement();
            Destroy(this.gameObject);
        }
    }
}
