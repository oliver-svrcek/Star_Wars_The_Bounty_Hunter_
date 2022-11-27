using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; }

    private void Awake()
    {
        if (ActivePlayer.PlayerData is null)
        {
            Debug.LogError("ERROR: <Coin> - ActivePlayer.PlayerData is null.");
            Application.Quit(1);
        }
        
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <Coin> - " + this.gameObject.transform.name + " game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
    }

    private void Start()
    {
        string levelName = SceneManager.GetActiveScene().name;
        if (!ActivePlayer.PlayerData.CollectedCoins.ContainsKey(levelName))
        {
            ActivePlayer.PlayerData.CollectedCoins.Add(levelName, new List<string>());
        }
        if (ActivePlayer.PlayerData.CollectedCoins[levelName].Contains(this.gameObject.name))
        {
            AudioManagement.RemoveFromMainAudioManagement();
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            string levelName = SceneManager.GetActiveScene().name;
            AudioManagement.PlayClipAtPoint("CoinPickupSound", this.gameObject.transform.position);
            ActivePlayer.PlayerData.CoinCount += 1;
            ActivePlayer.PlayerData.CollectedCoins[levelName].Add(this.gameObject.name);
            AudioManagement.RemoveFromMainAudioManagement();
            Destroy(this.gameObject);
        }
    }
}
