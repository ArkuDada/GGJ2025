using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using Utility;

public enum GameState
{
    Prepare,
    Play,
    Pause,
    GameOver
}

public class GameManager : MonoSingleton<GameManager>
{
    private GameState state;

    // Add your game-related variables
    [SerializeField]
    ScoreManager scoreManager;
    public ScoreManager ScoreManager => scoreManager;

    [SerializeField]
    TimeManager timeManager;
    public TimeManager TimeManager => timeManager;

    [SerializeField]
    FeverMeterManager feverMeterManager;
    public FeverMeterManager FeverMeterManager => feverMeterManager;

    // Initialize the game manager
    protected override void Init()
    {
        // Set initial values for your game manager
        ChangeGameplayState(GameState.Play);
    }

    // Might Refactor Later
    private void ChangeGameplayState(GameState newState)
    {
        state = newState;

        if (state == GameState.Play) 
        {
            timeManager.ResumeTimer();
        }
    }

    // You can add more game logic and features as needed
}
