using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdminMenu : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; }
    private TMP_InputField PlayerNameInputField { get; set; }
    private Scrollbar HorizontalScrollbar { get; set; }
    private GameObject HeaderGameObject { get; set; }
    private TextMeshProUGUI PlayerDataTextArea { get; set; }
    private TextMeshProUGUI HeaderText { get; set; }
    private GameObject PrimaryMenuGameObject { get; set; }
    private GameObject UserTypeMenuGameObject { get; set; }
    private GameObject PlayerNotFoundWarningGameObject { get; set; }
    private GameObject PlayerAlreadyExistsWaringGameObject { get; set; }
    private GameObject EmptyPlayerNameWarningGameObject { get; set; }
    private GameObject ConfirmDeleteWarningGameObject { get; set; }
    private GameObject ConfirmDeleteAllWarningGameObject { get; set; }
    private string PlayerName { get; set; }
    private float HeaderStartPositionX { get; set; }

    private void Awake()
    {
        if (GameObject.Find("Interface/MainCamera/Audio/Sounds") is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/Audio/Sounds game object was not found in game " +
                "object hierarchy."
                );
            Application.Quit(1);
        }
        if ((AudioManagement = GameObject.Find(
                "Interface/MainCamera/Audio/Sounds"
                ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/Audio/Sounds game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/PlayerNameInputField"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/" +
                "PlayerNameInputField game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((PlayerNameInputField = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/PlayerNameInputField"
                ).GetComponent<TMP_InputField>()) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/" +
                "PlayerNameInputField game object is missing TMP_InputField component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/PlayerStats/" +
                "PlayerData/Scrollbar Horizontal"
            ) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/" +
                "PlayerStats/PlayerData/Scrollbar Horizontal game object was not found in game object hierarchy."
            );
            Application.Quit(1);
        }
        if((HorizontalScrollbar = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/PlayerStats/" +
                "PlayerData/Scrollbar Horizontal"
            ).GetComponent<Scrollbar>()) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/" +
                "PlayerStats/PlayerData/Scrollbar Horizontal game object is missing Scrollbar component."
            );
            Application.Quit(1);
        }

        if ((HeaderGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/PlayerStats/Header/Text (TMP)"
            )) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/" +
                "PlayerStats/Header/Text (TMP) game object was not found in game object hierarchy."
            );
            Application.Quit(1);
        }
        
        if ((HeaderText = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/PlayerStats/Header/Text (TMP)"
            ).GetComponent<TextMeshProUGUI>()) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/" +
                "PlayerStats/Header/Text (TMP) game object is missing TextMeshProUGUI component."
            );
            Application.Quit(1);
        }
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/PlayerStats/" +
                "PlayerData/Viewport/Content"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/" +
                "PlayerStats/PlayerData/Viewport/Content game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((PlayerDataTextArea = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/PlayerStats/" +
                "PlayerData/Viewport/Content"
                ).GetComponent<TextMeshProUGUI>()) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu/" +
                "PlayerStats/PlayerData/Viewport/Content game object is missing TextMeshProUGUI component."
                );
            Application.Quit(1);
        }

        if ((PrimaryMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu"
            )) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu game " +
                "object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }

        if ((UserTypeMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PrimaryMenu"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/PrimaryMenu game object " +
                "was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((PlayerNotFoundWarningGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PlayerNotFoundWarning"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/" +
                "PlayerNotFoundWarning game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((PlayerAlreadyExistsWaringGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PlayerAlreadyExistsWarning"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/" +
                "PlayerAlreadyExistsWarning game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((EmptyPlayerNameWarningGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/EmptyPlayerNameWarning"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/" +
                "EmptyPlayerNameWarning game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((ConfirmDeleteWarningGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/ConfirmDeleteWarning"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/" +
                "ConfirmDeleteWarning game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((ConfirmDeleteAllWarningGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/ConfirmDeleteAllWarning"
            )) is null)
        {
            Debug.LogError(
                "ERROR: <AdminMenu> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/" +
                "ConfirmDeleteAllWarning game object was not found in game object hierarchy."
            );
            Application.Quit(1);
        }
    }
    
    private void Start()
    {
        
        PlayerName = "";
        PlayerDataTextArea.text = "";
        
        foreach (string columnName in DatabaseManagement.GetTableHeader("PlayerData"))
        {
            if (columnName == "CollectedCoins")
            {
                continue;
            }
            
            HeaderText.text += columnName.PadRight(20);
        }
        
        HeaderStartPositionX = HeaderGameObject.transform.position.x;
        
        LoadAllPlayerData();
    }

    public void MoveHeader()
    {
        HeaderGameObject.transform.position = new Vector3(
            HeaderStartPositionX - (42.4f * HorizontalScrollbar.value),
            HeaderGameObject.transform.position.y,
            HeaderGameObject.transform.position.z
        );
    }

    private void LoadSinglePlayerData()
    {
        PlayerDataTextArea.text = "";
        
        Dictionary<string, string> entry = DatabaseManagement.GetEntry("PlayerData", PlayerName);
        if (entry is null)
        {
            Debug.LogError("ERROR: <AdminMenu> - entry is null");
            return;
        }
        
        PrintEntryLine(entry);
    }
    
    public void LoadAllPlayerData()
    {
        PlayerDataTextArea.text = "";
        
        List<Dictionary<string, string>> entries = DatabaseManagement.GetEntries("PlayerData");

        foreach (Dictionary<string, string> entry in entries)
        {
            PrintEntryLine(entry);
            PlayerDataTextArea.text += "\n";
        }
    }

    public void PrintEntryLine(Dictionary<string, string> entry)
    {
        foreach (KeyValuePair<string, string> record in entry)
        {
            string value = record.Value;

            if (record.Key == "CollectedCoins")
            {
                continue;
            }
            if (record.Key == "PositionAxisX" || record.Key == "PositionAxisY")
            {
                value = Math.Round(float.Parse(value), 3).ToString();
            }

            PlayerDataTextArea.text += value.PadRight(20);
        }
        PlayerDataTextArea.text += "|";   
    }

    public void SearchPlayer()
    {
        PlayerName = PlayerNameInputField.text;
        PlayerName = PlayerName.Replace("\u200B", "");
        
        if (PlayerName != "" && !DatabaseManagement.EntryExists("PlayerData", PlayerName))
        {
            LoadAllPlayerData();
            AudioManagement.PlayOneShot("ErrorSound");
            PrimaryMenuGameObject.SetActive(false);
            PlayerNotFoundWarningGameObject.SetActive(true);
            PlayerNameInputField.text = "";
            PlayerName = "";
            return;
        }
        
        AudioManagement.PlayOneShot("ButtonSound");
        
        if (PlayerName == "")
        {
            LoadAllPlayerData();
        }
        else
        {
            LoadSinglePlayerData();
        }
        
        PlayerNameInputField.text = "";
        PlayerName = "";
    }
    
    public void AddPlayer()
    {
        PlayerName = PlayerNameInputField.text;
        PlayerName = PlayerName.Replace("\u200B", "");

        if (PlayerName == "")
        {
            AudioManagement.PlayOneShot("ErrorSound");
            PrimaryMenuGameObject.SetActive(false);
            EmptyPlayerNameWarningGameObject.SetActive(true);
            return;
        }
        
        if (DatabaseManagement.EntryExists("PlayerData", PlayerName))
        {
            AudioManagement.PlayOneShot("ErrorSound");
            PrimaryMenuGameObject.SetActive(false);
            PlayerAlreadyExistsWaringGameObject.SetActive(true);
            PlayerNameInputField.text = "";
            PlayerName = "";
            return;
        }
        
        AudioManagement.PlayOneShot("ButtonSound");
        PlayerData playerData = new PlayerData(PlayerName);
        playerData.SaveNewData();
        LoadAllPlayerData();
        PlayerNameInputField.text = "";
        PlayerName = "";
    }
    
    public void DeletePlayer()
    {
        PlayerName = PlayerNameInputField.text;
        PlayerName = PlayerName.Replace("\u200B", "");

        if (PlayerName == "")
        {
            AudioManagement.PlayOneShot("ButtonSound");
            PrimaryMenuGameObject.SetActive(false);
            ConfirmDeleteAllWarningGameObject.SetActive(true);
            return;
        }
        
        if (!DatabaseManagement.EntryExists("PlayerData", PlayerName))
        {
            AudioManagement.PlayOneShot("ErrorSound");
            PrimaryMenuGameObject.SetActive(false);
            PlayerNotFoundWarningGameObject.SetActive(true);
            PlayerNameInputField.text = "";
            PlayerName = "";
            return;
        }
        
        AudioManagement.PlayOneShot("ButtonSound");
        PrimaryMenuGameObject.SetActive(false);
        ConfirmDeleteWarningGameObject.SetActive(true);
    }
    
    public void DeletePlayerConfirm()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        DatabaseManagement.DeleteEntry("PlayerData", PlayerName);
        ConfirmDeleteWarningGameObject.SetActive(false);
        PrimaryMenuGameObject.SetActive(true);
        PlayerNameInputField.text = "";
        PlayerName = "";
        LoadAllPlayerData();
    }
    
    public void DeleteAllPlayersConfirm()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        DatabaseManagement.DeleteEntries("PlayerData");
        ConfirmDeleteAllWarningGameObject.SetActive(false);
        PrimaryMenuGameObject.SetActive(true);
        PlayerNameInputField.text = "";
        PlayerName = "";
        LoadAllPlayerData();
    }
    
    public void SwitchBackToPrimaryMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        PlayerNotFoundWarningGameObject.SetActive(false);
        PlayerAlreadyExistsWaringGameObject.SetActive(false);
        EmptyPlayerNameWarningGameObject.SetActive(false);
        ConfirmDeleteWarningGameObject.SetActive(false);
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
