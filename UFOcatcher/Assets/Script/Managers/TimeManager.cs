using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // Timer variables
    private float elapsedTime;
    [SerializeField]
    private float countdownTime;
    private bool isTimerActive;

    // Time settings
    public float startingCountdownTime = 30f; // Starting countdown time in seconds

    // Start is called before the first frame update
    void Start()
    {
        // Initialize elapsed time and countdown time
        elapsedTime = 0f;
        countdownTime = startingCountdownTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Track elapsed time for the game
        elapsedTime += Time.deltaTime;

        // Handle countdown timer if active
        if (isTimerActive)
        {
            countdownTime -= Time.deltaTime;
            if (countdownTime <= 0f)
            {
                countdownTime = 0f;
                TimerEnd();
            }
        }
    }

    // Function to start the countdown timer
    public void StartCountdown(float time)
    {
        countdownTime = time;
        isTimerActive = true;
    }

    // Function to pause the countdown timer
    public void PauseTimer()
    {
        isTimerActive = false;
    }

    // Function to resume the countdown timer
    public void ResumeTimer()
    {
        isTimerActive = true;
    }

    // Function to reset the countdown timer
    public void ResetTimer()
    {
        countdownTime = startingCountdownTime;
        isTimerActive = false;
    }

    // Function to stop the countdown timer
    public void StopTimer()
    {
        countdownTime = 0f;
        isTimerActive = false;
    }

    // Function to get the current elapsed time
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // Function to get the remaining countdown time
    public float GetRemainingTime()
    {
        return countdownTime;
    }

    // Function to handle when the countdown reaches zero
    private void TimerEnd()
    {
        Debug.Log("Countdown Timer Ended");
        GameManager.Instance.ChangeGameplayState(GameState.GameOver);
        // Additional logic when the countdown ends (e.g., game over, restart level, etc.)
    }

    // Function to display the time in the Unity console
    public void DisplayTime()
    {
        Debug.Log("Elapsed Time: " + elapsedTime.ToString("F2"));
        Debug.Log("Remaining Time: " + countdownTime.ToString("F2"));
    }
}
