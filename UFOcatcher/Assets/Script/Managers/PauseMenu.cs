using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    GameState lastgameState;
    private bool isPaused = false;

    void Start()
    {
        // Ensure the pause menu is hidden at start
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Instance.State == GameState.Tutorial || GameManager.Instance.State == GameState.Credits) 
        {
            return;
        }

        // Toggle pause menu when pressing Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game time


        if (GameUtility.GameManagerExists())
        {
            lastgameState = GameManager.Instance.State;
            GameManager.Instance.ChangeGameplayState(GameState.Pause);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);

        if (GameUtility.GameManagerExists())
        {
            if (lastgameState == GameState.Pause)
            {
                GameManager.Instance.ChangeGameplayState(GameState.Play);
            }
            else 
            {
                GameManager.Instance.ChangeGameplayState(lastgameState);
            }

        }
    }

    public void QuitGame()
    {
        // For now, just log and quit
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
