using UnityEngine;
using TMPro;
namespace Juve 
{
    public class GameplayUIManagerr : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI timeUI;

        [SerializeField]
        TextMeshProUGUI scoreUI;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (timeUI != null) 
            {
                timeUI.text = "Time: " + GameManager.Instance.TimeManager.GetRemainingTime().ToString();
            }

            if (scoreUI != null)
            {
                scoreUI.text = "Score: " +  GameManager.Instance.ScoreManager.GetScore().ToString();
            }
        }
    }
}


