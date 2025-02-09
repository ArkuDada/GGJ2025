using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    [SerializeField] private List<GameObject> Objects = new List<GameObject>();

    GameState lastgameState;

    private bool isPaused = false;

    void Start()
    {

        // Ensure the pause menu is hidden at start
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // Toggle pause menu when pressing Escape
    }

    public void PauseGame()
    {
        if (GameManager.Instance.State == GameState.Pause ||
           GameManager.Instance.State == GameState.GameOver ||
           GameManager.Instance.State == GameState.Credits)
        {
            return;
        }

        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game time

        foreach (var gameObj in Objects)
        {
            gameObj.SetActive(false);
        }


        if (GameUtility.GameManagerExists())
        {
            lastgameState = GameManager.Instance.State;
            GameManager.Instance.ChangeGameplayState(GameState.Tutorial);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        //Time.timeScale = 1f; // Resume game time

        foreach (var gameObj in Objects)
        {
            gameObj.SetActive(true);
        }

        if (GameUtility.GameManagerExists())
        {
            GameManager.Instance.ChangeGameplayState(lastgameState);
        }
    }

    public void OnClick()
    {
        if (GameManager.Instance.State == GameState.Pause)
        {
            return;
        }

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void QuitGame()
    {
        // For now, just log and quit
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
