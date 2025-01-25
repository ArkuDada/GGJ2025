using UnityEngine;

public class AcidRainSpawner : MonoBehaviour
{
    // Screen bounds for spawning AcidRain objects
    public Vector2 _screenBoundsMin;
    public Vector2 _screenBoundsMax;

    // Prefab for the AcidRain object
    public GameObject acidRainPrefab;

    // Spawn rate (seconds between spawns)
    public float spawnRate = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // Start spawning AcidRain objects repeatedly
        InvokeRepeating(nameof(SpawnAcidRain), 0f, spawnRate);
    }

    // Spawn an AcidRain object at a random position within bounds
    void SpawnAcidRain()
    {
        // Check if the prefab is assigned
        if (acidRainPrefab == null)
        {
            Debug.LogWarning("AcidRain prefab is not assigned!");
            return;
        }

        // Randomly determine the spawn position within the bounds
        float randomX = Random.Range(_screenBoundsMin.x, _screenBoundsMax.x);
        float randomY = Random.Range(_screenBoundsMin.y, _screenBoundsMax.y);
        Vector3 spawnPosition = new Vector3(randomX, 10, randomY);

        // Instantiate the AcidRain prefab at the spawn position
        Instantiate(acidRainPrefab, spawnPosition, Quaternion.identity);
    }
}
