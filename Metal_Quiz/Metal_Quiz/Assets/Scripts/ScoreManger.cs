using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManger : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScore;
    [SerializeField] TextMeshProUGUI finalMessage;

    int totalScore;

    string perfectScore = "Eres un verdadero Rocker";
    string midScore = "Solo te falta escuhar un poco más de rock";
    string badScore = "Que estas haciendo con tu vida?";
    // Start is called before the first frame update
    void Start()
    {    
       totalScore = PlayerPrefs.GetInt("total");
       Debug.Log(totalScore);
    }

    void FinalScore()
    {
        finalScore.text = totalScore.ToString();        
    }

    void FinalMessage()
    {
        if (totalScore <= 300)
        {
            finalMessage.text = badScore;
        }
        else if(totalScore > 300 && totalScore <= 800)
        {
            finalMessage.text = midScore;
        }
        else
        {
            finalMessage.text = perfectScore;
        }
    }

    private void Update()
    {
        FinalScore();
        FinalMessage();
    }
}
