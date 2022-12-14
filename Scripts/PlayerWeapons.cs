using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    private AudioManagement AudioManagement { get; set; } = null;
    public Blaster Blaster { get; private set; } = null;
    public Flamethrower Flamethrower { get; private set; } = null;

    private void Awake()
     {
         if (GameObject.Find("Interface/MainCamera/Audio/Sounds") is null)
         {
             Debug.LogError(
                 "ERROR: <PlayerWeapons> - Interface/MainCamera/Audio/Sounds game object was not found in " +
                 "game object hierarchy."
                 );
             Application.Quit(1);
         }
         if ((AudioManagement = GameObject.Find(
                 "Interface/MainCamera/Audio/Sounds"
             ).GetComponent<AudioManagement>()) is null)
         {
             Debug.LogError(
                 "ERROR: <PlayerWeapons> - Interface/MainCamera/Audio/Sounds game object is missing " +
                 "AudioManagement component."
                 );
             Application.Quit(1);
         }
         
         if (GameObject.Find("Player/Blaster") is null)
         {
             Debug.LogError(
                 "ERROR: <PlayerWeapons > - Player/Blaster game object was not found in the game object " +
                 "hierarchy."
                 );
             Application.Quit(1);
         }
         if ((Blaster = GameObject.Find("Player/Blaster").GetComponent<Blaster>()) is null)
         {
             Debug.LogError(
                 "ERROR: <PlayerWeapons> - Player/Blaster game object is missing Blaster component."
                 );
             Application.Quit(1);
         }
         
         if (GameObject.Find("Player/Flamethrower") is null)
         {
             Debug.LogError(
                 "ERROR: <PlayerWeapons > - Player/Flamethrower game object was not found in the game object" +
                 " hierarchy."
                 );
             Application.Quit(1);
         }
         if ((Flamethrower = GameObject.Find("Player/Flamethrower").GetComponent<Flamethrower>()) is null)
         {
             Debug.LogError(
                 "ERROR: <PlayerWeapons> - Player/Flamethrower game object is missing Flamethrower component."
                 );
             Application.Quit(1);
         }
     }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Blaster.Activate();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Flamethrower.Activate();
        }
    }
}
