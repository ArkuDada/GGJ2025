using UnityEngine;

public enum FeverPhase 
{
    Beginning,
    EarlyFever,
    AlmostMaxFever,
    Max,
    OverLimit
}

public class FeverMeterManager : MonoBehaviour
{
    private FeverPhase feverPhase = FeverPhase.Beginning;

    [SerializeField]
    float maxFever = 120.0f;

    [SerializeField]
    [Range(0.0f, 120.0f)]
    float currentFeverValue;
    public float CurrentFeverValue => currentFeverValue;

    [SerializeField]
    float earlyFeverPercentage = 33.33f;

    [SerializeField]
    float almostMaxPercentage = 66.66f;

    [SerializeField]
    float maxPercentage = 100.0f;

    [SerializeField]
    float overlimitPercentage = 101.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentFeverValue = 0;
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
        return currentFeverValue / maxFever;
    }

    private void UpdateFeverPercentage(float updateAmount)
    {
        if (feverPhase == FeverPhase.Beginning || feverPhase == FeverPhase.EarlyFever 
            || feverPhase == FeverPhase.AlmostMaxFever || feverPhase == FeverPhase.Max)
        {
            currentFeverValue += updateAmount;
        }
        else if (feverPhase == FeverPhase.OverLimit) 
        {
            currentFeverValue -= updateAmount;
        }
        
        OnFeverChanged();
    }

    private void OnFeverChanged() 
    {
        if (feverPhase == FeverPhase.Beginning && currentFeverValue >= earlyFeverPercentage)
        {
            feverPhase = FeverPhase.EarlyFever;
        }
        else if (feverPhase == FeverPhase.EarlyFever && currentFeverValue >= almostMaxPercentage)
        {
            feverPhase = FeverPhase.AlmostMaxFever;
        }
        else if (feverPhase == FeverPhase.AlmostMaxFever && currentFeverValue >= maxPercentage)
        {
            feverPhase = FeverPhase.Max;
        }
        else if (feverPhase == FeverPhase.Max && currentFeverValue >= overlimitPercentage)
        {
            GameManager.Instance.DebrisSpawner.SpawnRate = 0.1f;
            feverPhase = FeverPhase.OverLimit;
        } 
        else if (feverPhase == FeverPhase.OverLimit && currentFeverValue <= 0.0f) 
        {
            feverPhase = FeverPhase.Beginning;
            currentFeverValue = 0.0f;
        }
    }
}
