using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Juve 
{
    public class GameplayUIManagerr : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI timeUI;

        [SerializeField]
        TextMeshProUGUI scoreUI;

        [SerializeField]
        Slider feverMeterUI;
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
                scoreUI.text = GameManager.Instance.ScoreManager.GetScore().ToString("00000000");
            }
        }
    }
}


