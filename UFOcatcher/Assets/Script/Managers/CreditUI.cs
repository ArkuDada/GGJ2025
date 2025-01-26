using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreditUI : MonoBehaviour
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
            GameManager.Instance.ChangeGameplayState(GameState.Credits);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        
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
