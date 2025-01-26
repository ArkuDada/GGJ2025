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

        [SerializeField]
        ScoreFeedback scoreFeedbackPrefab;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            GameManager.Instance.ScoreManager.OnScoreChanged += SpawnScoreFeedback;
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
                scoreUI.text = GameManager.Instance.ScoreManager.GetScore().ToString("00000");
            }
        }

        void SpawnScoreFeedback(int scoreIncrease)
        {
            if (scoreFeedbackPrefab != null && scoreUI != null)
            {
                // Instantiate the score feedback prefab at the position of scoreUI
                Vector3 spawnPosition = scoreUI.transform.position + new Vector3(5.0f,0.0f,0.0f);
                ScoreFeedback feedbackInstance = Instantiate(scoreFeedbackPrefab, spawnPosition, Quaternion.identity, this.transform);

                // Optionally, initialize the feedback with the score increase value
                feedbackInstance.SetScore(scoreIncrease);
            }
        }
    }
}


