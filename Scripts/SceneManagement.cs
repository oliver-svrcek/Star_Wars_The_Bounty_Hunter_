using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagement
{
    public static void LoadNextScene()
    {
        int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneBuildIndex == SceneManager.sceneCountInBuildSettings)
        {
            Cursor.visible = true;
            SceneManager.LoadScene(0); // Load main menu
            return;
        }
        
        if (SceneUtility.GetScenePathByBuildIndex(nextSceneBuildIndex).Contains("Level") 
            || SceneUtility.GetScenePathByBuildIndex(nextSceneBuildIndex).Contains("Boss"))
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }

        SceneManager.LoadScene(nextSceneBuildIndex);
    }

    public static void LoadSavedScene()
    {
        if (ActivePlayer.PlayerData is null)
        {
            Debug.LogError("ERROR: <SceneManagement> - ActivePlayer.PlayerData is null.");
            Application.Quit(1);
        }

        if (SceneUtility.GetScenePathByBuildIndex(ActivePlayer.PlayerData.SceneBuildIndex).Contains("Level") 
            || SceneUtility.GetScenePathByBuildIndex(ActivePlayer.PlayerData.SceneBuildIndex).Contains("Boss"))
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
        
        SceneManager.LoadScene(ActivePlayer.PlayerData.SceneBuildIndex);
    }

    public static void LoadSceneByBuildIndex(int sceneBuildIndex)
    {
        if (SceneUtility.GetScenePathByBuildIndex(sceneBuildIndex).Contains("Level") 
            || SceneUtility.GetScenePathByBuildIndex(sceneBuildIndex).Contains("Boss"))
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
        
        SceneManager.LoadScene(sceneBuildIndex);
    }
    
    public static void LoadSceneByName(string sceneName)
    {
        if (sceneName.Contains("Level") || sceneName.Contains("Boss"))
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
        
        SceneManager.LoadScene(sceneName);
    }
}
