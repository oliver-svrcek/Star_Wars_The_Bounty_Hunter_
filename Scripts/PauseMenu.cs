using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private FadeManagement FadeManagement { get; set; } = null;
    private AudioManagement AudioManagement { get; set; } = null;
    private GameObject PrimaryMenuGameObject { get; set; } = null;
    private GameObject UpgradesMenuGameObject { get; set; } = null;
    private GameObject ExitWarningGameObject { get; set; } = null;
    private UpgradesMenu UpgradesMenu { get; set; } = null;
    private bool CanPause { get; set; } = true;
    public bool Paused { get; private set; } = false;

    private void Awake()
    {
        if (GameObject.Find("Interface/MainCamera/FadeCanvas/Fade") is null)
        {
            Debug.LogError(
                "ERROR: <PauseMenu> - Interface/MainCamera/FadeCanvas/Fade game object was not found in game" +
                " object hierarchy."
                );
            Application.Quit(1);
        }
        if ((FadeManagement = GameObject.Find(
                "Interface/MainCamera/FadeCanvas/Fade"
            ).GetComponent<FadeManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <PauseMenu> - Interface/MainCamera/FadeCanvas/Fade game object is missing " +
                "FadeManagement component."
                );
            Application.Quit(1);
        }

        if (GameObject.Find("Interface/MainCamera/Audio/Sounds") is null)
        {
            Debug.LogError(
                "ERROR: <PauseMenu> - Interface/MainCamera/Audio/Sounds game object was not found " +
                "in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((AudioManagement = GameObject.Find(
                "Interface/MainCamera/Audio/Sounds"
                ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <PauseMenu> - Interface/MainCamera/Audio/Sounds game object is missing " + 
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if ((PrimaryMenuGameObject = GameObject.Find("Interface/MainCamera/UICanvas/PauseMenu/PrimaryMenu")) is null)
        {
            Debug.LogError(
                "ERROR: <PauseMenu> - Interface/MainCamera/UICanvas/PauseMenu/PrimaryMenu game " +
                "object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((UpgradesMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <PauseMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu game " +
                "object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((ExitWarningGameObject = GameObject.Find("Interface/MainCamera/UICanvas/PauseMenu/ExitWarning")) is null)
        {
            Debug.LogError(
                "ERROR: <PauseMenu> - Interface/MainCamera/UICanvas/PauseMenu/ExitWarning game object was " +
                "not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((UpgradesMenu = GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu"
                ).GetComponent<UpgradesMenu>()) is null)
        {
            Debug.LogError(
                "ERROR: <PauseMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu game object is " +
                "missing UpgradesMenu component."
                );
            Application.Quit(1);
        }
    }

    public void ResumeClick()
    {
        Resume();
        AudioManagement.PlayOneShot("ButtonSound");
    }
    
    public void Resume()
    {
        Cursor.visible = false;
        PrimaryMenuGameObject.SetActive(false);
        UpgradesMenuGameObject.SetActive(false);
        ExitWarningGameObject.SetActive(false);
        MainAudioManagement.SetPauseAll(false);
        Time.timeScale = 1f;
        Paused = false;
        CanPause = true;
    }
    
    public void Pause()
    {
        if (Paused)
        {
            Resume();
            return;
        }
        
        if (!CanPause)
        {
            return;
        }

        Cursor.visible = true;
        PrimaryMenuGameObject.SetActive(true);
        MainAudioManagement.SetPauseAll(true);
        Time.timeScale = 0f;
        Paused = true;
        CanPause = false;
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void SetCanPause(bool canPause)
    {
        CanPause = canPause;
    }
    
    public void SwitchBackToPrimaryMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        UpgradesMenuGameObject.SetActive(false);
        ExitWarningGameObject.SetActive(false);
        PrimaryMenuGameObject.SetActive(true);
    }

    public void SwitchToUpgradesMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        PrimaryMenuGameObject.SetActive(false);
        UpgradesMenu.RefreshLoadedPlayerData();
        UpgradesMenuGameObject.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        PrimaryMenuGameObject.SetActive(false);
        ExitWarningGameObject.SetActive(true);
    }

    public void ConfirmExitToMainMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        FadeManagement.FadeOut("main menu");
    }
}
