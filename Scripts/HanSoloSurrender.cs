using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanSoloSurrender : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; } = null;
    private GameObject Coin1GameObject { get; set; } = null;
    private GameObject Coin2GameObject { get; set; } = null;
    private GameObject LevelEndGameObject { get; set; } = null;

    private void Awake()
    {
        if ((AudioManagement = this.gameObject.GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError("ERROR: <HanSoloSurrender> - Box game object is missing AudioManagement component.");
            Application.Quit(1);
        }

        if ((Coin1GameObject = GameObject.Find("Coin (1)")) is null)
        {
            Debug.LogError(
                "ERROR: <HanSoloSurrender> - Coin (1) game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }

        if ((Coin2GameObject = GameObject.Find("Coin (2)")) is null)
        {
            Debug.LogError(
                "ERROR: <HanSoloSurrender> - Coin (2) game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((LevelEndGameObject = GameObject.Find("LevelEnd")) is null)
        {
            Debug.LogError(
                "ERROR: <HanSoloSurrender> - LevelEnd game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            LevelEndGameObject.SetActive(true);
            Coin1GameObject.SetActive(true);
            Coin2GameObject.SetActive(true);
            AudioManagement.PlayClipAtPoint("CoinSpawnSound", this.gameObject.transform.position);
            AudioManagement.RemoveFromMainAudioManagement();
            Destroy(this.gameObject);
        }
    }
}
