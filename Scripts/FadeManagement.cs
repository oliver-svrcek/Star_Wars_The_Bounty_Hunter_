using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManagement : MonoBehaviour
{
    private PauseMenu PauseMenu { get; set; }
    private string SceneToLoad { get; set; }
    private Image Image { get; set; }
    private Animator Animator { get; set; }

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name.Contains("Level")
            || SceneManager.GetActiveScene().name.Contains("Boss"))
        {
            if (GameObject.Find("Interface/MainCamera/UICanvas/PauseMenu") is null)
            {
                Debug.LogError(
                    "ERROR: <FadeManagement> - Interface/MainCamera/UICanvas/PauseMenu game object was not " +
                    "found in game object hierarchy."
                    );
                Application.Quit(1);
            }
            if ((PauseMenu = GameObject.Find(
                    "Interface/MainCamera/UICanvas/PauseMenu"
                ).GetComponent<PauseMenu>()) is null)
            {
            
                Debug.LogError(
                    "ERROR: <FadeManagement> - Interface/MainCamera/UICanvas/PauseMenu game object is " +
                    "missing PauseMenu component."
                    );
                Application.Quit(1);
            }
        }

        if ((Image = this.gameObject.GetComponent<Image>()) is null)
        {
            Debug.LogError(
                "ERROR: <FadeManagement> - " + this.gameObject.transform.name + " game object is missing " +
                "Image component."
                );
            Application.Quit(1);
        }
        
        if ((Animator = this.gameObject.GetComponent<Animator>()) is null)
        {
            Debug.LogError(
                "ERROR: <FadeManagement> - " + this.gameObject.transform.name + " game object is missing " +
                "Animator component."
                );
            Application.Quit(1);
        }

        SceneToLoad = "saved";
    }

    public void FadeIn()
    {
        Animator.SetBool(nameof(FadeOut), false);
        Animator.SetBool(nameof(FadeIn), true);
    }

    public void FadeOut(string sceneToLoad)
    {
        SceneToLoad = sceneToLoad;
        Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        Animator.SetBool(nameof(FadeIn), false);
        Animator.SetBool(nameof(FadeOut), true);
    }

    public void ResetAnimation()
    {
        Animator.SetBool(nameof(FadeIn), false);
        Animator.SetBool(nameof(FadeOut), false);
        Image.enabled = false;
    }

    public void EnableImage(bool enable)
    {
        Image.enabled = enable;
    }

    public void SetImageAlpha(byte alpha)
    {
        Image.color = new Color32(0, 0, 0, alpha); 
    }
    
    public void BlockInteractions(int enable)
    {
        if (enable == 1)
        {
            Image.enabled = true;
            if (SceneManager.GetActiveScene().name.Contains("Level")
                || SceneManager.GetActiveScene().name.Contains("Boss"))
            {
                PauseMenu.SetCanPause(false);
            }
        }
        else
        {
            Image.enabled = false;
            if (SceneManager.GetActiveScene().name.Contains("Level")
                || SceneManager.GetActiveScene().name.Contains("Boss"))
            {
                PauseMenu.SetCanPause(true);
            }
        }
    }

    private void LoadScene()
    {
        Animator.updateMode = AnimatorUpdateMode.Normal;
        if (SceneManager.GetActiveScene().name.Contains("Level")
            || SceneManager.GetActiveScene().name.Contains("Boss"))
        {
            PauseMenu.SetTimeScale(1f);
        }

        if (SceneToLoad == "saved")
        {
            SceneManagement.LoadSavedScene();
        }
        else if (SceneToLoad == "next")
        {
            SceneManagement.LoadNextScene();
        }
        else if (SceneToLoad == "main menu")
        {
            SceneManagement.LoadSceneByBuildIndex(0);
        }
    }
}
