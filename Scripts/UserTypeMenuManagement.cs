using System;
using System.Collections.Generic;
using UnityEngine;

public class UserTypeMenuManagement : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; }
    private GameObject PrimaryMenuGameObject { get; set; }
    private GameObject PlayerLoginMenuGameObject { get; set; }
    private GameObject AdminMenuGameObject { get; set; }
    private AdminMenu AdminMenu { get; set; }

    private void Awake()
    {
        if (GameObject.Find("Interface/MainCamera/Audio/Sounds") is null)
        {
            Debug.LogError(
                "ERROR: <UserTypeMenuManagement> - Interface/MainCamera/Audio/Sounds game object was not " +
                "found in the game object hierarchy."
                );
            Application.Quit(1);
        }

        if ((AudioManagement = GameObject.Find(
                "Interface/MainCamera/Audio/Sounds"
            ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <UserTypeMenuManagement> - Interface/MainCamera/Audio/Sounds game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if ((PrimaryMenuGameObject = GameObject.Find("Interface/MainCamera/UICanvas/UserTypeMenu/PrimaryMenu")) is null)
        {
            Debug.LogError(
                "ERROR: <UserTypeMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/PrimaryMenu " +
                "game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((PlayerLoginMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/PlayerLoginMenu/PrimaryMenu"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <UserTypeMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/" +
                "PlayerLoginMenu/PrimaryMenu game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((AdminMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/PrimaryMenu"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <UserTypeMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu/" +
                "PrimaryMenu game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }

        if ((AdminMenu = GameObject.Find(
                "Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu"
                ).GetComponent<AdminMenu>()) is null)
        {
            Debug.LogError(
                "ERROR: <UserTypeMenuManagement> - Interface/MainCamera/UICanvas/UserTypeMenu/AdminMenu game " +
                "object is missing AdminMenu component."
                );
            Application.Quit(1);
        }
        
        DatabaseInitialization();
    }

    private void DatabaseInitialization()
    {
        DatabaseManagement.InitialiseConnection(Application.persistentDataPath + "/" + "PlayerDataDB");
        DatabaseManagement.CreateTable(new DatabaseTable("PlayerData", 
            new List<string>()
            {
                "SceneBuildIndex", "PositionAxisX", "PositionAxisY", "CoinCount", "CollectedCoins",
                "ArmorLevel", "BlasterLevel", "JetpackLevel", "FlamethrowerLevel"
            }));
    }

    public void SwitchToPlayerLoginMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        PrimaryMenuGameObject.SetActive(false);
        PlayerLoginMenuGameObject.SetActive(true);
    }
    
    public void SwitchToAdminMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        PrimaryMenuGameObject.SetActive(false);
        AdminMenu.LoadAllPlayerData();
        AdminMenuGameObject.SetActive(true);
    }

    public void QuitGame()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        DatabaseManagement.CloseConnection();
        Application.Quit(0);
    }
}
