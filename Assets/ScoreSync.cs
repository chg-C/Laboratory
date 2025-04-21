using System.Collections;
using System.Collections.Generic;
using CHG.Lab;
using TMPro;
using UnityEngine;

public class ScoreSync : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public LabGameManager temp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        scoreText.text = "";

        var scores = temp.scores;

        foreach(var s in scores)
        {
            scoreText.text += s.id + " - " + s.score + "\n";
        }

        scoreText.text += "총 점수:" + temp.TotalScore;
    }
}
