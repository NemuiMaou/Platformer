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
    public int coinScoreIncrease = 100;
    public int brickBreakScoreIncrease = 100;
    public int winningScoreIncrease = 10000;
    private bool winOrLose = true;
    
    
    
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
                    CoinIncrease();
                    ScoreIncrease(coinScoreIncrease);
                    
                }
            }
        }

        if (timeLeft <= 0 && winOrLose)
        {
            winOrLose = false;
            Debug.Log("You Lose!");
        }
    }

    public void CoinIncrease()
    {
        coinCount++;
        String coinFormat = coinCount.ToString("00");
        coinCountText.text = "x" + coinFormat;
    }

    public void ScoreIncrease(int points)
    {
        score += points;
        String scoreFormat = score.ToString("000000");
        totalScoreText.text = "Mario\n" + scoreFormat;
    }

    public void winCheck()
    {
        if (winOrLose)
        {
            winOrLose = false;
            Debug.Log("You Win!!");
            ScoreIncrease(winningScoreIncrease);
        }
    }
}
