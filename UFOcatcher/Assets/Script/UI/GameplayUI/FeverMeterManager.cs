using UnityEngine;
using static UnityEditor.Rendering.ShadowCascadeGUI;

public class FeverMeterManager : MonoBehaviour
{
	// From indices 0-1, it's FeverPhase.Beginning. From 1-2, it's FeverPhase.Almost. From 2-3, it's FeverPhase.Max
	private readonly float[] feverPercentages = { 0f, 1f / 3f, 2f / 3f, 1f };
	private bool inFever = false;
	public Material[] feverMaterials;

	const float MAX_FEVER = 120.0f;
	const float FEVER_INCREASE_RATE = 10.0f;
	const float FEVER_DECREASE_RATE = 10.0f;

	[SerializeField]
	[Range(0.0f, MAX_FEVER)]
	private float currentFeverValue;
	public float CurrentFeverValue => currentFeverValue;

	public Arcade arcade;
	private Vector3 localBarStartingPosition;
	private MeshRenderer feverBarMeshRenderer;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		currentFeverValue = 0;
		if (arcade == null)
			arcade = GameObject.Find("Arcade").GetComponent<Arcade>();
		localBarStartingPosition = arcade.FeverMeter.transform.localPosition;
		feverBarMeshRenderer = arcade.FeverMeter.GetComponent<MeshRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		UpdateFeverPercentage(Time.deltaTime);
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
			dt *= -FEVER_DECREASE_RATE;
		}
		else
		{
			dt *= FEVER_INCREASE_RATE;
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
				GameManager.Instance.DebrisSpawner.SpawnRate = 0;
				inFever = false;
			}
		}
		else if (currentFeverPercentage >= 0.99f)
		{
			GameManager.Instance.DebrisSpawner.SpawnRate = 0.1f;
			inFever = true;
		}

		// Update arcade visuals
		arcade.FeverMeter.localScale = new Vector3(1, currentFeverPercentage, 1);
	}
}
