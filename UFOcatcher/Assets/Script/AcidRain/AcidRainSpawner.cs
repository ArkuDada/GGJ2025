using UnityEngine;

public class AcidRainSpawner : MonoBehaviour
{
    // Screen bounds for spawning AcidRain objects
    [SerializeField] private Vector2 _screenBoundsMin;
    [SerializeField] private Vector2 _screenBoundsMax;

    // Prefab for the AcidRain object
    [SerializeField] private GameObject acidRainPrefab;

    // Spawn rate (seconds between spawns)
    [SerializeField] private float spawnRate = 2f;

    public float SpawnRate
    {
        get => spawnRate;
        set
        {
            if (Mathf.Approximately(spawnRate, value)) return; // Ignore if the rate hasn't changed
            spawnRate = Mathf.Max(value, 0.1f); // Prevent too low values
            RestartSpawning(); // Restart the spawning process with the new rate
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start spawning AcidRain objects repeatedly
        InvokeRepeating(nameof(SpawnAcidRain), 0f, spawnRate);
    }

    // Start the spawning process
    private void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnAcidRain), 0f, spawnRate);
    }

    // Stop the spawning process
    private void StopSpawning()
    {
        CancelInvoke(nameof(SpawnAcidRain));
    }

    // Restart the spawning process with the new rate
    private void RestartSpawning()
    {
        StopSpawning();
        StartSpawning();
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
