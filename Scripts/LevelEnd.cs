using System;
using System.Collections;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private Player Player { get; set; }
    private FadeManagement FadeManagement { get; set; }
    private BoxCollider2D BoxCollider2D { get; set; }
    private void Awake()
    {
        if (GameObject.Find("Player") is null)
        {
            Debug.LogError(
                "ERROR: <LevelEnd> - Player game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((Player = GameObject.Find("Player").GetComponent<Player>()) is null)
        {
            Debug.LogError(
                "ERROR: <LevelEnd> - Player game object is missing Player component."
                );
            Application.Quit(1);
        }

        if (GameObject.Find("Interface/MainCamera/FadeCanvas/Fade") is null)
        {
            Debug.LogError(
                "ERROR: <LevelEnd> - Interface/MainCamera/FadeCanvas/Fade game object was not found in game" +
                " object hierarchy."
                );
            Application.Quit(1);
        }
        if ((FadeManagement = GameObject.Find(
                "Interface/MainCamera/FadeCanvas/Fade"
            ).GetComponent<FadeManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <LevelEnd> - Interface/MainCamera/FadeCanvas/Fade game object is missing " +
                "FadeManagement component."
                );
            Application.Quit(1);
        }
        
        if ((BoxCollider2D  = this.gameObject.GetComponent<BoxCollider2D>()) is null)
        {
            Debug.LogError(
                "ERROR: <LevelEnd> - " + this.gameObject.transform.name + " game object is missing " +
                "BoxCollider2D component."
                );
            Application.Quit(1);
        }
    }

    private void Start()
    {
        if (Player.PlayerData is null)
        {
            Debug.LogError("ERROR: <LevelEnd> - Player.PlayerData is null.");
            Application.Quit(1);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.ReloadGearValues();
            Player.PlayerData.SceneBuildIndex += 1;
            Player.PlayerData.PositionAxisX = 0;
            Player.PlayerData.PositionAxisY = 0;
            Player.PlayerData.UpdateData();
            BoxCollider2D.enabled = false;
            StartCoroutine(EndLevel());
        }
    }

    private IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(3f);
        FadeManagement.FadeOut("saved");
    }
}
