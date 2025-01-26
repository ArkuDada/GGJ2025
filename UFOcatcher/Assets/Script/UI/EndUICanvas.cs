using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndUICanvas : MonoBehaviour
{
    [Header("UI Screens")]
    public GameObject enterNameScreen;
    public GameObject highscoreScreen;
    public GameObject leaderboardScreen;
    public GameObject congratsScreen;


    [Header("Navigation Buttons")]
    public Button nextButton;


    private enum ScreenState
    {
        EnterName,
        Highscore,
        Leaderboard,
        Congrats
    }

    private ScreenState currentScreen;

    private void Start()
    {
        currentScreen = ScreenState.EnterName;
        ShowEnterNameScreen();

        nextButton.onClick.AddListener(() =>
        {
            HandleNextButtonPress();
        });
    }

    private void HandleNextButtonPress()
    {
        switch (currentScreen)
        {
            case ScreenState.EnterName:
                ShowHighscoreScreen();
                break;
            case ScreenState.Highscore:
                ShowLeaderboardScreen();
                break;
            case ScreenState.Leaderboard:
                ShowCongratsScreen();
                break;
            case ScreenState.Congrats:
                Destroy(GameManager.Instance.gameObject);
                SceneManager.LoadScene("SampleScene");
                break;
        }
    }

    private void ShowEnterNameScreen()
    {
        HideAllScreens();
        enterNameScreen.SetActive(true);
        currentScreen = ScreenState.EnterName;
    }

    private void ShowHighscoreScreen()
    {
        HideAllScreens();
        highscoreScreen.SetActive(true);
        currentScreen = ScreenState.Highscore;
    }

    private void ShowLeaderboardScreen()
    {
        HideAllScreens();
        leaderboardScreen.SetActive(true);
        currentScreen = ScreenState.Leaderboard;
    }

    private void ShowCongratsScreen()
    {
        HideAllScreens();
        congratsScreen.SetActive(true);
        currentScreen = ScreenState.Congrats;
    }

    private void HideAllScreens()
    {
        enterNameScreen.SetActive(false);
        highscoreScreen.SetActive(false);
        leaderboardScreen.SetActive(false);
        congratsScreen.SetActive(false);
    }
}
