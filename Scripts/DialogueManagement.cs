using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManagement : MonoBehaviour
{
    private FadeManagement FadeManagement { get; set; }
    private AudioManagement AudioManagement { get; set; }
    private RectTransform DialogueBoxRectTransform { get; set; }
    private TextMeshProUGUI CharacterNameText { get; set; }
    private TextMeshProUGUI DialogueLineText { get; set; }
    private GameObject PrevButtonDeactivatorGameObject { get; set; }
    private GameObject NextButtonDeactivatorGameObject { get; set; }
    private List<DialogueLine> DialogueLines { get; set; }
    protected Coroutine PrintLettersCoroutine { get; set; }
    private int CurrentLineIndex { get; set; }

    private void Awake()
    {
        if (ActivePlayer.PlayerData is null)
        {
            Debug.LogError("ERROR: <DialogueManagement> - ActivePlayer.PlayerData is null.");
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/FadeCanvas/Fade") is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/FadeCanvas/Fade game object was not found" +
                " in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((FadeManagement = GameObject.Find(
                "Interface/MainCamera/FadeCanvas/Fade"
                ).GetComponent<FadeManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/FadeCanvas/Fade game object is " +
                "missing FadeManagement component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/Audio/Sounds") is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/Audio/Sounds game object was not found " +
                "in game object hierarchy."
                );
            Application.Quit(1);
        }

        if ((AudioManagement = GameObject.Find(
                "Interface/MainCamera/Audio/Sounds"
            ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/Audio/Sounds game object is " +
                "missing AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/DialogueBox") is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/" +
                "DialogueBox game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((DialogueBoxRectTransform = GameObject.Find(
                "Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/DialogueBox"
                ).GetComponent<RectTransform>()) is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/" +
                "DialogueBox game object is missing RectTransform component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/CharacterNameText") is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/" +
                "CharacterNameText game object was not found in game object hierarchy."
                );
        }
        if ((CharacterNameText = GameObject.Find(
                "Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/CharacterNameText"
                ).GetComponent<TextMeshProUGUI>()) is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/" +
                "CharacterNameText game object is missing TextMeshProUGUI component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/DialogueLineText") is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/" +
                "DialogueLineText game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((DialogueLineText = GameObject.Find(
                "Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/DialogueLineText"
                ).GetComponent<TextMeshProUGUI>()) is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/" +
                "DialogueLineText game object is missing TextMeshProUGUI component."
                );
            Application.Quit(1);
        }
        
        if ((PrevButtonDeactivatorGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/ButtonsArea/PrevLineWrapper/ButtonDeactivator"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/" +
                "ButtonsArea/PrevLineWrapper/ButtonDeactivator game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((NextButtonDeactivatorGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/ButtonsArea/NextLineWrapper/ButtonDeactivator"
            )) is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Interface/MainCamera/UICanvas/DialogueMenu/PrimaryMenu/" +
                "ButtonsArea/NextLineWrapper/ButtonDeactivator game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }

        TextAsset textAsset;
        if ((textAsset = Resources.Load<TextAsset>("Text/Dialogue/" + SceneManager.GetActiveScene().name)) is null)
        {
            Debug.LogError(
                "ERROR: <DialogueManagement> - Text/Dialogue/" + SceneManager.GetActiveScene().name +
                " resource was not loaded."
                );
            Application.Quit(1);
        }

        if ((DialogueLines = JsonConvert.DeserializeObject<List<DialogueLine>>(textAsset.ToString())) is null)
        {
            Debug.LogError("ERROR: <DialogueManagement> - textAsset could not be deserialized.");
            Application.Quit(1);
        }
        
    }

    private void Start()
    {
        PrintLettersCoroutine = null;
        CurrentLineIndex = 0;
        PrintLine();
    }

    public void PrevLine()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        CurrentLineIndex -= 1;
        PrintLine();
    }
    
    public void NextLine()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        CurrentLineIndex += 1;
        PrintLine();
    }

    public void ContinueToLevel()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        ActivePlayer.PlayerData.SceneBuildIndex += 1;
        ActivePlayer.PlayerData.UpdateData();
        FadeManagement.FadeOut("saved");
    }

    private void PrintLine()
    {
        PrevButtonDeactivatorGameObject.gameObject.SetActive(false);
        NextButtonDeactivatorGameObject.gameObject.SetActive(false);

        if (CurrentLineIndex == 0)
        {
            PrevButtonDeactivatorGameObject.gameObject.SetActive(true);
        }
        
        if (CurrentLineIndex == (DialogueLines.Count - 1))
        {
            NextButtonDeactivatorGameObject.gameObject.SetActive(true);
        }
        
        RotateDialogueBox(DialogueLines[CurrentLineIndex].CharacterName);
        CharacterNameText.text = DialogueLines[CurrentLineIndex].CharacterName;

        if (PrintLettersCoroutine is not null)
        {
            StopCoroutine(PrintLettersCoroutine);
        }

        PrintLettersCoroutine = StartCoroutine(PrintLetters(DialogueLines[CurrentLineIndex].LineText));
    }

    private IEnumerator PrintLetters(string line)
    {
        DialogueLineText.text = "";
        foreach (char letter in line)
        {
            DialogueLineText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void RotateDialogueBox(string characterName)
    {
        if (characterName == "Boba Fett")
        {
            DialogueBoxRectTransform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else
        {   
            DialogueBoxRectTransform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}
