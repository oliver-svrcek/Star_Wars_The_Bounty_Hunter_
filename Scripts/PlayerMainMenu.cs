using UnityEngine;

public class PlayerMainMenu : MonoBehaviour
{
    private FadeManagement FadeManagement { get; set; } = null;
    private AudioManagement AudioManagement { get; set; } = null;
    private GameObject PrimaryMenuGameObject { get; set; } = null;
    private GameObject PlayerLoginMenuGameObject { get; set; } = null;
    private GameObject SaveDataNotFoundWarningGameObject { get; set; } = null;
    private GameObject NewGameOverwriteWarningGameObject { get; set; } = null;
    
    private void Awake()
    {
        if (GameObject.Find("Interface/MainCamera/FadeCanvas/Fade") is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Interface/MainCamera/FadeCanvas/Fade game object was not found in " + 
                "game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((FadeManagement = GameObject.Find(
                "Interface/MainCamera/FadeCanvas/Fade"
            ).GetComponent<FadeManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Interface/MainCamera/FadeCanvas/Fade game object is missing " + 
                "FadeManagement component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/Audio/Sounds") is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Interface/MainCamera/Audio/Sounds game object was not " + 
                "found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((AudioManagement = GameObject.Find(
                "Interface/MainCamera/Audio/Sounds"
            ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Interface/MainCamera/Audio/Sounds game object is " +
                "missing AudioManagement component."
                );
            Application.Quit(1);
        }

        if ((PrimaryMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerMainMenu/PrimaryMenu"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/" +
                "PlayerMainMenu/PrimaryMenu game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((PlayerLoginMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PrimaryMenu"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/" +
                "PrimaryMenu game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((SaveDataNotFoundWarningGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerMainMenu/SaveDataNotFoundWarning"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/" +
                "PlayerMainMenu/SaveDataNotFoundWarning game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((NewGameOverwriteWarningGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerMainMenu/NewGameOverwriteWarning"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerMainMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/" +
                "PlayerMainMenu/NewGameOverwriteWarning game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
    }

    public void ContinueGame()
    {
        if (ActivePlayer.PlayerData.SceneBuildIndex == 0)
        {
            AudioManagement.PlayOneShot("ErrorSound");
            PrimaryMenuGameObject.SetActive(false);
            SaveDataNotFoundWarningGameObject.SetActive(true);
            return;
        }
        
        AudioManagement.PlayOneShot("ButtonSound");
        FadeManagement.FadeOut("saved");
    }

    public void NewGame()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        
        if (ActivePlayer.PlayerData.SceneBuildIndex != 0)
        {
            PrimaryMenuGameObject.SetActive(false);
            NewGameOverwriteWarningGameObject.SetActive(true);
            return;
        }

        ActivePlayer.PlayerData.SceneBuildIndex = 1;
        ActivePlayer.PlayerData.UpdateData();
        FadeManagement.FadeOut("saved");
    }
    
    public void OverwriteGame()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        ActivePlayer.PlayerData.ResetData();
        ActivePlayer.PlayerData.SceneBuildIndex = 1;
        ActivePlayer.PlayerData.UpdateData();
        FadeManagement.FadeOut("saved");
    }
    
    public void SwitchBackToPrimaryMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        SaveDataNotFoundWarningGameObject.SetActive(false);
        NewGameOverwriteWarningGameObject.SetActive(false);
        PrimaryMenuGameObject.SetActive(true);
    }
    
    public void ExitPlayerLoginMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        PrimaryMenuGameObject.SetActive(false);
        PlayerLoginMenuGameObject.SetActive(true);
    }
}
