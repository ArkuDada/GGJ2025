using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;
using Utility;
using UnityEngine.SceneManagement;

public enum GameState
{
    Start,
    Tutorial,
    Prepare,
    Play,
    Pause,
    GameOver,
    Credits,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameState state;
    public GameState State => state;

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

    [SerializeField]
    GameObject tutorialUI;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initialize the game manager
    void Start()
    {
        // Set initial values for your game manager
        ChangeGameplayState(GameState.Start);
    }

    // Might Refactor Later
    public void ChangeGameplayState(GameState newState)
    {
        state = newState;

        if(state == GameState.Start)
        {
            startUI.SetActive(true);
            Time.timeScale = 0;
            endUI.SetActive(false);
            feverMeterManager.Init();
            timeManager.ResetTimer();
            scoreManager.ResetScore();
        }
        else if(state == GameState.Tutorial)
        {
            startUI.SetActive(false);
            tutorialUI.SetActive(true);
        }
        else if(state == GameState.Prepare)
        {
            tutorialUI.SetActive(false);
            startUI.SetActive(false);
            Time.timeScale = 1;
        }
        else if(state == GameState.Play)
        {
            Time.timeScale = 1f;
            debrisSpawner.StartSpawning();
            timeManager.ResumeTimer();
        }
        else if(state == GameState.Pause)
        {
            timeManager.PauseTimer();
        }
        else if(state == GameState.GameOver)
        {
            StartCoroutine(EndSequence());
        }
       
    }

    private IEnumerator EndSequence()
    {
        debrisSpawner.StopSpawning();
        scoreManager.SaveHighScore();
        Time.timeScale = 0;
        
        var ufo = GameObject.FindGameObjectWithTag("Player").GetComponent<UFOController>();
        ufo.IsGoAway = true;

        yield return new WaitForSecondsRealtime(2.5f);
        var screen = GameObject.Find("MainArcadeScreen").GetComponent<ArcadeScreenMask>();
        screen.ChangeAnimationMat();
        screen.PlayReflectionAnimation();
        yield return new WaitForSecondsRealtime(2.5f);

        endUI.SetActive(true);
    }

    private void Update()
    {
        if(state == GameState.Start)
        {
            if(Input.anyKeyDown)
            {
                ChangeGameplayState(GameState.Tutorial);
            }
        }
        else if(state == GameState.Tutorial)
        {
            if(Input.anyKeyDown)
            {
                ChangeGameplayState(GameState.Prepare);
            }
        }
        else if(state == GameState.Prepare)
        {
            ChangeGameplayState(GameState.Play);
        }
        else
        {
            //if(Input.GetKeyDown(KeyCode.Escape))
            //{
            //    if(state == GameState.Play)
            //    {
            //        ChangeGameplayState(GameState.Pause);
            //    }
            //    else if(state == GameState.Pause)
            //    {
            //        ChangeGameplayState(GameState.Play);
            //    }
            //}
        }
    }

    // You can add more game logic and features as needed
}