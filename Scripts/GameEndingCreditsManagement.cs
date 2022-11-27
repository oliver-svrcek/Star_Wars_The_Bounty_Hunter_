using UnityEngine;
public class GameEndingCreditsManagement : MonoBehaviour
{
    private FadeManagement FadeManagement { get; set; } = null;

    private void Awake()
    {
        if (GameObject.Find("Interface/MainCamera/FadeCanvas/Fade") is null)
        {
            Debug.LogError(
                "ERROR: <GameEndingCreditsManagement> - Interface/MainCamera/FadeCanvas/Fade game object was " +
                "not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((FadeManagement = GameObject.Find(
                "Interface/MainCamera/FadeCanvas/Fade"
                ).GetComponent<FadeManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <GameEndingCreditsManagement> - Interface/MainCamera/FadeCanvas/Fade game object is " +
                "missing FadeManagement component."
                );
            Application.Quit(1);
        }
    }

    public void EndGame()
    {
        ActivePlayer.PlayerData.ResetData();
        ActivePlayer.PlayerData.UpdateData();
        FadeManagement.FadeOut("main menu");
    }
}
