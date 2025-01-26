using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    private bool isPaused = false;

    void Start()
    {
        // Set up button listeners
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);

        // Ensure the pause menu is hidden at start
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
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
            GameManager.Instance.ChangeGameplayState(GameState.Pause);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time

        if (GameUtility.GameManagerExists())
        {
            GameManager.Instance.ChangeGameplayState(GameState.Play);
        }
    }

    public void QuitGame()
    {
        // For now, just log and quit
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
