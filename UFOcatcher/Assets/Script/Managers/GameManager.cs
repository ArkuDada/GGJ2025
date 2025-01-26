using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;
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

    [FormerlySerializedAs("acidRainSpawner")]
    [SerializeField]
    DebrisSpawner debrisSpawner;
    public DebrisSpawner DebrisSpawner => debrisSpawner;

	[SerializeField]
	QuestManager questManager;
	public QuestManager QuestManager => questManager;


    // Initialize the game manager
    protected override void Init()
    {
        // Set initial values for your game manager
        ChangeGameplayState(GameState.Play);
    }

    // Might Refactor Later
    public void ChangeGameplayState(GameState newState)
    {
        state = newState;

        if (state == GameState.Play) 
        {
            timeManager.ResumeTimer();
        }
        else if (state == GameState.Pause)
        {
            timeManager.StopTimer();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == GameState.Play)
            {
                ChangeGameplayState(GameState.Pause);
            }
            else if (state == GameState.Pause) 
            {
                ChangeGameplayState(GameState.Play);
            }
        }
    }

    // You can add more game logic and features as needed


}
