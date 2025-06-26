using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    int score=0;

    public TMP_Text scoreText;
    public float scoreChangeDuration=0.3f;
    public Color scoreIncreaseColor;
    public Color scoreDecreaseColor;

    Color scoreInitialColor;

    Color initialColor;
    List<int> scoreChanges;

    int currentScoreChange=0;
    float lastScoreChangeTime=0f;



    // Start is called before the first frame update
    void Start()
    {
        scoreInitialColor = scoreText.faceColor;
        scoreChanges=new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        //scoreText.faceColor = scoreInitialColor;

        if (currentScoreChange!=0) {
            if (Time.time > (lastScoreChangeTime+scoreChangeDuration)) {
                //Debug.Log("scoreChangeTime: "+lastScoreChangeTime+" scoreChangeDuration: "+scoreChangeDuration+" Current Time: "+Time.time);
                currentScoreChange=0;
                scoreText.text = score.ToString();
                return;
            }
            // figure out where you are in the score change
            float timeSinceLastScoreChange = Time.time-lastScoreChangeTime;
            float pctComplete = timeSinceLastScoreChange/scoreChangeDuration;
            int theNumber = score+Mathf.FloorToInt(pctComplete*(float)currentScoreChange);
            //Debug.Log("Setting score to: "+theNumber);
            scoreText.text = theNumber.ToString();
            /*
            if (currentScoreChange > 0) {
                scoreText.faceColor = scoreIncreaseColor*6f;
            }
            else {
                scoreText.faceColor = scoreDecreaseColor*6f;
            }
            */
            return;
        }
        else if (scoreChanges.Count > 0 ) {
            currentScoreChange=scoreChanges[0];
            scoreChanges.RemoveAt(0);
            scoreText.faceColor = scoreInitialColor;
        }
    }

    public void AddScore(int newScore) {
        score+=newScore;
        scoreChanges.Add(newScore);
        lastScoreChangeTime=Time.time;
    }
}
