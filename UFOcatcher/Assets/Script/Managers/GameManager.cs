using UnityEngine;
using Utility;

public class GameManager : MonoSingleton<GameManager>
{
    // Add your game-related variables
    [SerializeField]
    ScoreManager scoreManager;
    ScoreManager ScoreManager => scoreManager;

    [SerializeField]
    TimeManager timeManager;
    TimeManager TimeManager => timeManager;

    // Initialize the game manager
    protected override void Init()
    {
        // Set initial values for your game manager
    }

    // You can add more game logic and features as needed
}
