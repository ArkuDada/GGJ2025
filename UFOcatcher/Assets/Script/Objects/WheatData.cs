using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(GrowingObject))]
public class WheatData : MonoBehaviour
{
    public bool beingEaten => cowEating != null;
    public CowData cowEating = null; // Cow that's eating this wheat
    
    public GameObject[] growthStages;
    
    [FormerlySerializedAs("_currentStage")]
    public int CurrentStage = 0;

    public void ChangeState(int state)
    {
        CurrentStage = state;
        foreach (var stage in growthStages)
        {
            stage.SetActive(false);
        }
        growthStages[state].SetActive(true);
    }
}
