using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event Action<int> OnScoreChanged;

    // Variable to hold the current score
    private int score;

    // Variable to hold the high score
    private int highScore;

    public UFOController ufo;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the score at the start of the game
        score = 0;

        // Load the high score from PlayerPrefs, defaulting to 0 if not found
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (ufo == null) {
            GameObject ufoGameObject = GameObject.Find("UFO");
            if (ufoGameObject == null)
                Debug.LogError("Failed to find UFO GameObject");
            
            ufo = ufoGameObject.GetComponent<UFOController>();
			if (ufo == null)
				Debug.LogError("Failed to find UFO component");
		}
    }

    // Function to increment the score
    public void IncrementScore(int points)
    {

        if (points == 0) 
        {
            return;
        }

        ufo.TriggerHappy();
        score += points;
        OnScoreChanged?.Invoke(points);
        DisplayScore();
    }

	// Function to increment the score
	public void CompletedQuest()
	{
		ufo.TriggerCelebration();
		IncrementScore(1000); // Magic number: I asked designers for this number, and they said 1000 :)
	}

	// Function to decrement the score
	public void DecrementScore(int points)
    {
        Debug.Log("de points: " + points);
        if (points == 0)
        {
            return;
        }
        ufo.TriggerSad();
        score -= points;
        OnScoreChanged?.Invoke(-points);
		DisplayScore();
    }

    // Function to reset the score to zero
    public void ResetScore()
    {
        score = 0;
        DisplayScore();
    }

    // Function to get the current score
    public int GetScore()
    {
        return score;
    }

    // Function to get the high score
    public int GetHighScore()
    {
        return highScore;
    }

    // Function to save the high score
    public void SaveHighScore()
    {
        // If the current score is greater than the high score, update the high score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); // Save high score to PlayerPrefs
            PlayerPrefs.Save(); // Ensure data is written to disk
        }
    }

    // Function to display the current score and high score in the Unity console
    public void DisplayScore()
    {
        Debug.Log("Current Score: " + score);
        Debug.Log("High Score: " + highScore);
    }
}
