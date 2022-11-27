using TMPro;
using UnityEngine;

public class PlayerLoginMenuManagement : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; } = null;
    private TMP_InputField PlayerNameInputField { get; set; } = null;
    private GameObject PrimaryMenuGameObject { get; set; } = null;
    private GameObject PlayerMainMenuGameObject { get; set; } = null;
    private GameObject UserTypeMenuGameObject { get; set; } = null;
    private GameObject PlayerNotFoundWarningGameObject { get; set; } = null;
    private GameObject PlayerAlreadyExistsWaringGameObject { get; set; } = null;
    private GameObject EmptyPlayerNameWarningGameObject { get; set; } = null;
    
    private void Awake()
    {
        if (GameObject.Find("Interface/MainCamera/Audio/Sounds") is null)
        {
            Debug.LogError(
                "ERROR: <PlayerLoginMenuManagement> - Interface/MainCamera/Audio/Sounds game object was not " +
                "found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((AudioManagement = GameObject.Find(
                "Interface/MainCamera/Audio/Sounds"
            ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerLoginMenuManagement> - Interface/MainCamera/Audio/Sounds game object is " +
                "missing AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PrimaryMenu/PlayerNameInputField/"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerLoginMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/" +
                "PlayerLoginMenu/PrimaryMenu/PlayerNameInputField game object was not found in game " +
                "object hierarchy."
                );
            Application.Quit(1);
        }
        if ((PlayerNameInputField = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PrimaryMenu/PlayerNameInputField"
                ).GetComponent<TMP_InputField>()) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerLoginMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/" +
                "PlayerLoginMenu/PrimaryMenu/PlayerNameInputField game object is missing TMP_InputField component."
                );
            Application.Quit(1);
        }
        
        if ((PrimaryMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PrimaryMenu"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerLoginMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/" +
                "PlayerLoginMenu/PrimaryMenu game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((PlayerMainMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerMainMenu/PrimaryMenu"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerLoginMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/" +
                "PlayerLoginMenu/PlayerMainMenu/PrimaryMenu game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((UserTypeMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PrimaryMenu"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerLoginMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/PrimaryMenu" +
                " game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((PlayerNotFoundWarningGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerNotFoundWarning"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerLoginMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/" +
                "PlayerLoginMenu/PlayerNotFoundWarning game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((PlayerAlreadyExistsWaringGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PlayerAlreadyExistsWarning"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerLoginMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/" +
                "PlayerLoginMenu/PlayerAlreadyExistsWarning game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((EmptyPlayerNameWarningGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/EmptyPlayerNameWarning"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <PlayerLoginMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/" +
                "PlayerLoginMenu/EmptyPlayerNameWarning game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
    }

    public void PlayerLogin()
    {
        string playerName = PlayerNameInputField.text;
        playerName = playerName.Replace("\u200B", "");

        if (playerName == "")
        {
            AudioManagement.PlayOneShot("ErrorSound");
            PrimaryMenuGameObject.SetActive(false);
            EmptyPlayerNameWarningGameObject.SetActive(true);
            return;
        }
        
        if (!DatabaseManagement.EntryExists("PlayerData", playerName))
        {
            AudioManagement.PlayOneShot("ErrorSound");
            PrimaryMenuGameObject.SetActive(false);
            PlayerNotFoundWarningGameObject.SetActive(true);
            PlayerNameInputField.text = "";
            return;
        }
        
        AudioManagement.PlayOneShot("ButtonSound");
        PlayerData playerData = new PlayerData(playerName);
        playerData.LoadData();
        ActivePlayer.PlayerData = playerData;
        SwitchToPlayerMainMenu();
    }
    
    public void PlayerRegister()
    {
        string playerName = PlayerNameInputField.text;
        playerName = playerName.Replace("\u200B", "");

        if (playerName == "")
        {
            AudioManagement.PlayOneShot("ErrorSound");
            PrimaryMenuGameObject.SetActive(false);
            EmptyPlayerNameWarningGameObject.SetActive(true);
            return;
        }
        
        if (DatabaseManagement.EntryExists("PlayerData", playerName))
        {
            AudioManagement.PlayOneShot("ErrorSound");
            PrimaryMenuGameObject.SetActive(false);
            PlayerAlreadyExistsWaringGameObject.SetActive(true);
            PlayerNameInputField.text = "";
            return;
        }
        
        AudioManagement.PlayOneShot("ButtonSound");
        PlayerData playerData = new PlayerData(playerName);
        playerData.SaveNewData();
        ActivePlayer.PlayerData = playerData;
        SwitchToPlayerMainMenu();
    }

    public void SwitchToPlayerMainMenu()
    {
        PrimaryMenuGameObject.SetActive(false);
        PlayerMainMenuGameObject.SetActive(true);
        PlayerNameInputField.text = "";
    }

    public void SwitchBackToPrimaryMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        PlayerNotFoundWarningGameObject.SetActive(false);
        PlayerAlreadyExistsWaringGameObject.SetActive(false);
        EmptyPlayerNameWarningGameObject.SetActive(false);
        PrimaryMenuGameObject.SetActive(true);
    }
    
    public void ExitToUserTypeMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        PrimaryMenuGameObject.SetActive(false);
        UserTypeMenuGameObject.SetActive(true);
        PlayerNameInputField.text = "";
    }
}
