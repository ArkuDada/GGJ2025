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
            float timeInSeconds = GameManager.Instance.TimeManager.GetRemainingTime();
            // Get the minutes and seconds
            int second = Mathf.FloorToInt(timeInSeconds);
            int Milisec = Mathf.FloorToInt((timeInSeconds - (int)(timeInSeconds)) * 100f);
            

            // Format the output as "minutes:seconds" with 2 decimal places
            string formattedTime = string.Format("{0}:{1:D2}", second, Milisec);
            
            if (timeUI != null) 
            {
                timeUI.text = formattedTime;
            }

            if (scoreUI != null)
            {
                scoreUI.text = GameManager.Instance.ScoreManager.GetScore().ToString("000000");
            }
        }
    }
}


