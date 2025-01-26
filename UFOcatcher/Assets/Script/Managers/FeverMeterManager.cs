using UnityEngine;

public class FeverMeterManager : MonoBehaviour
{
	// From indices 0-1, it's FeverPhase.Beginning. From 1-2, it's FeverPhase.Almost. From 2-3, it's FeverPhase.Max
	private readonly float[] feverPercentages = { 0f, 1f / 3f, 2f / 3f, 1f };
	private bool inFever = false;
	public Material[] feverMaterials;

	
	const float MAX_FEVER = 90.0f;
	public float feverIncreaseRate = 10.0f;
	public float feverDecreaseRate = 10.0f;

    [Header("0-33 Settings")]
    [Tooltip("Spawn Rate between 33 -> 66 Percent")]
    [SerializeField]
    float defaultSpawnRate = 2.0f;
    [SerializeField]
    float defaultWaitTime = 3f;

    [Header("33-66 Settings")]
    [Tooltip("Spawn Rate between 33 -> 66 Percent")]
    [SerializeField]
    float earlierSpawnRate = 2.0f;
    [SerializeField]
    float earlierWaitTime = 3f;

    [Header("66-99 Settings")]
    [Tooltip("Spawn Rate between 66 -> 99 Percent.")]
    [SerializeField]
    float lateGameSpawnRate = 2.0f;
    [SerializeField]
    float lateGameWaitTime = 3f;

    [Header("Apocalypse Settings")]
    [Tooltip("Spawn Rate between >100 Percent.")]
    [SerializeField]
	float apocalypseSpawnRate = 0.1f;
    [SerializeField]
    float apocalypseWaitTime = 1f;


    [SerializeField]
	[Range(0.0f, MAX_FEVER)]
	private float currentFeverValue;
	public float CurrentFeverValue => currentFeverValue;

	public Arcade arcade;
	private Vector3 barStartingLocalPosition;
	private Vector3 barStartingLocalScale;
	private MeshRenderer feverBarMeshRenderer;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		Init();
    }

	// Update is called once per frame
	void Update()
	{
		UpdateFeverPercentage(Time.deltaTime);
	}

    public void Init()
    {
        currentFeverValue = 0;
        if (arcade == null)
            arcade = GameObject.Find("Arcade").GetComponent<Arcade>();
        barStartingLocalPosition = arcade.FeverMeter.transform.localPosition;
        barStartingLocalScale = arcade.FeverMeter.transform.localScale;
        feverBarMeshRenderer = arcade.FeverMeter.GetComponent<MeshRenderer>();
    }

    public void IncreaseFever(float increaseAmount)
	{
		currentFeverValue += increaseAmount;
	}

	public float GetFeverPercentage()
	{
		return currentFeverValue / MAX_FEVER;
	}

	private void UpdateFeverPercentage(float dt)
	{
		if (inFever)
		{
			dt *= -feverDecreaseRate;
		}
		else
		{
			dt *= feverIncreaseRate;
		}

		currentFeverValue = Mathf.Clamp(currentFeverValue + dt, 0, MAX_FEVER);
		OnFeverChanged();
	}

	private void OnFeverChanged()
	{
		float currentFeverPercentage = GetFeverPercentage();
		for (int i = 0; i < feverPercentages.Length - 1; i++)
		{
			// If it's within this fever range
			if (feverPercentages[i] < currentFeverPercentage && currentFeverPercentage < feverPercentages[i + 1])
			{
				feverBarMeshRenderer.material = feverMaterials[i];
			}
		}

		if (inFever)
		{
			if (currentFeverPercentage <= 0)
			{
                GameManager.Instance.DebrisSpawner.spawnWaitTime = defaultWaitTime;
                GameManager.Instance.DebrisSpawner.SpawnRate = defaultSpawnRate;
				inFever = false;
			}
		}
		else if (currentFeverPercentage >= 0.99f)
		{
            SoundManager.instance.PlaySFX("Meteor Start");
			GameManager.Instance.DebrisSpawner.spawnWaitTime = apocalypseWaitTime;
            GameManager.Instance.DebrisSpawner.SpawnRate = apocalypseSpawnRate;
            CameraShaker.Instance.DoScreenShake();
            inFever = true;
        }
        else if (currentFeverPercentage >= 2f / 3f)
        {
            GameManager.Instance.DebrisSpawner.spawnWaitTime = lateGameWaitTime;
            GameManager.Instance.DebrisSpawner.SpawnRate = lateGameSpawnRate;

        }
        else if (currentFeverPercentage >= 1f / 3f)
        {
            GameManager.Instance.DebrisSpawner.spawnWaitTime = earlierWaitTime;
            GameManager.Instance.DebrisSpawner.SpawnRate = earlierSpawnRate;
        }

        // Update arcade visuals
        Vector3 newScale = barStartingLocalScale;
		newScale.y *= currentFeverPercentage;
		arcade.FeverMeter.localScale = newScale;
	}
}
