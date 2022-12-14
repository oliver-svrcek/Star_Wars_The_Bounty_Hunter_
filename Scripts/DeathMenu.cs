using System;
using System.Collections;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    private FadeManagement FadeManagement { get; set; } = null;
    private AudioManagement AudioManagement { get; set; } = null;
    private AudioManagement MusicAudioManagement { get; set; } = null;
    private GameObject PrimaryMenuGameObject { get; set; } = null;
    private GameObject GameTipGameObject { get; set; } = null;
    private GameObject RespawnButtonGameObject { get; set; } = null;
    private GameObject MainMenuButtonGameObject { get; set; } = null;
    private GameObject RespawnTimerGameObject { get; set; } = null;
    private BarManagement RespawnTimerBar { get; set; } = null;
    private PauseMenu PauseMenu { get; set; } = null;
    public Coroutine ActivateCoroutine { get; private set; } = null;


    private void Awake()
    {
        if (GameObject.Find("Interface/MainCamera/FadeCanvas/Fade") is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/FadeCanvas/Fade game object was not found in game " +
                "object hierarchy."
                );
            Application.Quit(1);
        }

        if ((FadeManagement = GameObject.Find(
                "Interface/MainCamera/FadeCanvas/Fade"
                ).GetComponent<FadeManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/FadeCanvas/Fade game object is missing " +
                "FadeManagement component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/Audio/Sounds") is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/Audio/Sounds game object was not found in game " +
                "object hierarchy."
                );
            Application.Quit(1);
        }
        if ((AudioManagement = GameObject.Find(
                "Interface/MainCamera/Audio/Sounds"
            ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/Audio/Sounds game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/Audio/Music") is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/Audio/Music game object was not found in game" +
                " object hierarchy."
            );
            Application.Quit(1);
        }
        if ((MusicAudioManagement = GameObject.Find(
                "Interface/MainCamera/Audio/Music"
            ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/Audio/Music game object is missing " +
                "FadeManagement component."
            );
            Application.Quit(1);
        }
        
        if ((PrimaryMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu game object " +
                "was not found in game object hierarchy."
                );
            Application.Quit(1);
        }

        if ((GameTipGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/GameTip"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/GameTip" +
                " game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }

        if ((RespawnButtonGameObject = GameObject.Find(
                    "Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/RespawnButton"
                    )) is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/" +
                "RespawnButton game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }

        if ((MainMenuButtonGameObject = GameObject.Find(
                    "Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/MainMenuButton"
                    )) is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/" +
                "MainMenuButton game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }

        if (GameObject.Find("Interface/MainCamera/UICanvas/PauseMenu") is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/UICanvas/PauseMenu game object was not found" +
                " in game object hierarchy."
                );
            Application.Quit(1);
        }

        if ((PauseMenu = GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu"
                ).GetComponent<PauseMenu>()) is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - game object Interface/MainCamera/UICanvas/PauseMenu is missing " +
                "PauseMenu component."
                );
            Application.Quit(1);
        }
        
        if ((RespawnTimerGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/RespawnTimerBar"
            )) is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/RespawnTimerBar" +
                " game object was not found in game object hierarchy."
            );
            Application.Quit(1);
        }
        
        if ((RespawnTimerBar = GameObject.Find(
                "Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/RespawnTimerBar/Slider"
            ).GetComponent<BarManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <DeathMenu> - game object Interface/MainCamera/UICanvas/DeathMenu/PrimaryMenu/" +
                "RespawnTimerBar/Slider is missing BarManagement component."
            );
            Application.Quit(1);
        }
    }

    // private void Start()
    // {
    //     PrimaryMenuGameObject.SetActive(true);
    //     RespawnTimerBar.SetMaxValue(1f);
    //     RespawnTimerBar.SetValue(1f);
    //     RespawnTimerBar.SetGradient("Recharging");
    //     PrimaryMenuGameObject.SetActive(false);
    // }

    public void Activate()
    {
        if (ActivateCoroutine is null)
        {
            ActivateCoroutine = StartCoroutine(ActivateRoutine());
        }
    }
    
    private IEnumerator ActivateRoutine()
    {
        PauseMenu.Resume();
        PauseMenu.SetCanPause(false);
        ActivePlayer.PlayerData.LoadData();

        yield return new WaitForSecondsRealtime(0.5f);
        
        Time.timeScale = 0f;
        MainAudioManagement.StopAll();
        MusicAudioManagement.Play("PlayerDeathMusic", false);
        Cursor.visible = true;
        
        PrimaryMenuGameObject.SetActive(true);
        RespawnTimerBar.SetMaxValue(1f);
        RespawnTimerBar.SetValue(1f);
        RespawnTimerBar.SetGradient("Recharging");
        StartCoroutine(RespawnCountdown());

        yield return new WaitForSecondsRealtime(5f);

        GameTipGameObject.SetActive(false);
        RespawnTimerGameObject.SetActive(false);
        RespawnButtonGameObject.SetActive(true);
        MainMenuButtonGameObject.SetActive(true);
    }
    
    private IEnumerator RespawnCountdown()
    {
        while (RespawnTimerBar.GetValue() > 0f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            RespawnTimerBar.SetValue(((float) RespawnTimerBar.GetValue() - 0.0032f));
        }
        
        RespawnTimerBar.SetValue(0f);
    }

    public void Respawn()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        FadeManagement.FadeOut("saved");
    }
    
    public void ExitToMainMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        FadeManagement.FadeOut("main menu");
    }
}
