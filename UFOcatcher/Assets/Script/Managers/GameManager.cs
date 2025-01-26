using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;
using Utility;
using UnityEngine.SceneManagement;

public enum GameState
{
    Start,
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

    [SerializeField]
    GameObject startUI;

    [SerializeField]
    GameObject endUI;

    // Initialize the game manager
    protected override void Init()
    {
        // Set initial values for your game manager
        ChangeGameplayState(GameState.Start);
    }

    // Might Refactor Later
    public void ChangeGameplayState(GameState newState)
    {
        state = newState;

        if (state == GameState.Start)
        {
            startUI.SetActive(true);
            Time.timeScale = 0;
            endUI.SetActive(false);
            feverMeterManager.Init();
            timeManager.ResetTimer();
            scoreManager.ResetScore();
        }
        else if (state == GameState.Prepare)
        {
            startUI.SetActive(false);
            Time.timeScale = 1;
        }
        else if (state == GameState.Play)
        {
            debrisSpawner.StartSpawning();
            timeManager.ResumeTimer();
        }
        else if (state == GameState.Pause)
        {
            timeManager.PauseTimer();
        } 
        else if (state == GameState.GameOver) 
        {
            debrisSpawner.StopSpawning();
            scoreManager.SaveHighScore();
            Time.timeScale = 0;
            endUI.SetActive(true);

        }
    }

    private void Update()
    {
        if (state == GameState.Start)
        {
            if (Input.anyKeyDown)
            {
                ChangeGameplayState(GameState.Prepare);
            }
        } 
        else if (state == GameState.Prepare) 
        {
            ChangeGameplayState(GameState.Play);
        }
        else
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
    }

    // You can add more game logic and features as needed


}
