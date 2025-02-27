using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinCountText;
    public TextMeshProUGUI totalScoreText;
    private int score;
    private int coinCount;
    private int coinScoreIncrease = 100;
    private int brickBreakScoreIncrease = 100;
    
    // Update is called once per frame
    void Update()
    {
        int timeLeft = 100 - (int)Time.time;
        timerText.text = $"Time: \n {timeLeft}";

        if (Input.GetMouseButtonDown(0))  
        {
            RaycastHit hit;
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition); 

            if (Physics.Raycast(myRay, out hit))
            {
                GameObject myObject = hit.collider.gameObject;
                if (myObject.CompareTag("Brick"))
                {
                    Destroy(myObject);
                }

                if (myObject.CompareTag("QuestionBox"))
                {
                    coinCount++;
                    ScoreIncrease(coinScoreIncrease);
                    String coinFormat = coinCount.ToString("00");
                    coinCountText.text = "x" + coinFormat;
                }
            }
        }
    }

    public void ScoreIncrease(int points)
    {
        score += points;
        String scoreFormat = score.ToString("000000");
        totalScoreText.text = "Mario\n" + scoreFormat;
    }
}
