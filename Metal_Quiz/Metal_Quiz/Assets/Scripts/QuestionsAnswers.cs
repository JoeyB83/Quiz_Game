using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionsAnswers : MonoBehaviour
{
    enum GameState {FillInQuestions, WaitForChoice, Check, Wait, StillInTheGame, ShowResults, End}
    GameState state;

    [SerializeField] MetalQuestions[] questions;
    [SerializeField] TextMeshProUGUI question;
    [SerializeField] TextMeshProUGUI[] options;
    [SerializeField] TextMeshProUGUI score;   

    [SerializeField] string questionScene;
    [SerializeField] string finalScene;
    [SerializeField] private Color correctColor;
    [SerializeField] private Color wrongColor;
    [SerializeField] private Color normalColor;
    [SerializeField] Image[] buttonColors;

    public Button[] buttonArray;
    
    //Score
    public int playerScore;
    int scoreQuestion = 100;
    int scoreBadAnswer = 25;
    public int totalScore;    

    //Numer of questions
    int numberOfQuestions;
    int maxQuestions = 10;

    //Player choice
    int chooseButton;
    string chooseOption;
    string playerChoice;
    int rightOption;

    //Timer between answers
    float timeRef = 0.5f;
    float timeCounter;   

    MetalQuestions currentQuestion;

    // Aqui genero las preguntas y opciones de respuesta
    void GenerateQuestions()
    {
        foreach (Button myButton in buttonArray)
        {
            myButton.interactable = true;
        }
        do
        {
            currentQuestion = questions[Random.Range(0, questions.Length)];
        }
        while (currentQuestion.askedQuestion);

        currentQuestion.askedQuestion = true;

        for (int i=0; i < options.Length; i++)
        {
            options[i].text = currentQuestion.options[i];
        }

        question.text = currentQuestion.question;       
        
    }

    //Aqui guardo la elección del jugador
    void PlayerAnswer(int buttonNumber)
    {
        chooseButton = buttonNumber;
        chooseOption = currentQuestion.options[chooseButton];        
        playerChoice = chooseOption;
        state = GameState.Check;
    }

    //Funcionabilidad de los botones de opciones
    public void PlayerInput(int buttonNumber)
    {
        PlayerAnswer(buttonNumber);

        if (playerChoice == currentQuestion.correctAnswer)
        {
            buttonColors[buttonNumber].color = correctColor;            
        }
        else
        {
            buttonColors[buttonNumber].color = wrongColor;
            rightOption = Array.IndexOf(currentQuestion.options, currentQuestion.correctAnswer);
            buttonColors[rightOption].color = correctColor;            
        }

        foreach (Button myButton in buttonArray)
        {
            myButton.interactable= false;
        }

        state = GameState.Check;
    }

    //Tiempo entre la elección de la respuesta y la siguiente pregunta
    bool Timer()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter > timeRef) 
        {
            timeCounter = 0;
            return true;            
        }

        return false;
    }    

    //Reviso la respuesta es correcto o erronea
    void CheckAnswer()
    {
        if (playerChoice == currentQuestion.correctAnswer)
        {
            playerScore += scoreQuestion;
            score.text = playerScore.ToString();            
        }        

        PlayerPrefs.SetInt("total", playerScore);               
    }

    //Volver los botones a su color normal
    void BackToNormalColor()
    {
        buttonColors[chooseButton].color = normalColor;
        buttonColors[rightOption].color = normalColor;
    }

    
    //Booleano para la cantidad de preguntas
    bool QuestionsCount()
    {
        numberOfQuestions++;
        return numberOfQuestions < maxQuestions;
    }

    //Funcionabilidad del boton para saltar la pregunta
    public void SkipQuestion()
    {
        if (QuestionsCount())
        {
            GenerateQuestions();
        }
        else
        {
            ShowResults();
        }
        
        //SceneManager.LoadScene(questionScene);          
    }

    //Metodo para ir a la escena final
    void ShowResults()
    {        
        SceneManager.LoadScene(finalScene);
    }    
        
    private void Start()
    {
        score.text = "0";
        PlayerPrefs.SetInt("total", 0);
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.FillInQuestions:
                GenerateQuestions();
                state = GameState.WaitForChoice;
                break;
            case GameState.WaitForChoice:                
                break;
            case GameState.Check:
                CheckAnswer();
                state = GameState.Wait; 
                break;
            case GameState.Wait:
                if (Timer())
                {
                    BackToNormalColor();
                    state = GameState.StillInTheGame;
                };                
                break;
            case GameState.StillInTheGame:
                if (QuestionsCount())
                {
                    state = GameState.FillInQuestions;
                }
                else
                {
                    state = GameState.ShowResults;
                }
                break;
            case GameState.ShowResults:
                ShowResults();
                state = GameState.End;
                break;
            case GameState.End:
                break;
        }
    }
}

[System.Serializable]
public class MetalQuestions
{
    public string question;
    public string correctAnswer;
    public string[] options;
    [HideInInspector]
    public bool askedQuestion;

}

