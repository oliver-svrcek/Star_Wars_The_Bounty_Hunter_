using UnityEngine;
public class GameEndingCreditsAnimationManagement : MonoBehaviour
{
    private GameEndingCreditsManagement GameEndingCreditsManagement { get; set; }

    private void Awake()
    {
        if (GameObject.Find("Interface/MainCamera/UICanvas/GameEndingCredits") is null)
        {
            Debug.LogError(
                "ERROR: <GameEndingCreditsAnimationManagement> - Interface/MainCamera/UICanvas/" +
                "GameEndingCredits game object was not found in game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((GameEndingCreditsManagement = GameObject.Find(
                "Interface/MainCamera/UICanvas/GameEndingCredits"
                ).GetComponent<GameEndingCreditsManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <GameEndingCreditsAnimationManagement> - Interface/MainCamera/UICanvas/" +
                "GameEndingCredits game object is missing GameEndingCreditsManagement component."
                );
            Application.Quit(1);
        }
    }

    private void EndGame()
    {
        GameEndingCreditsManagement.EndGame();
    }
}