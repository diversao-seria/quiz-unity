using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionNumberController : MonoBehaviour
{

    private int currentQuestion;
    private int maxQuestions;

    private void Start()
    {
        currentQuestion = 1;
    }

    public void SetMaxQuestions(int n)
    {
        maxQuestions = n;
    }

    public void NextQuestion()
    {
        currentQuestion++;
    }

    public void FormatedDisplay()
    {
        GetComponent<Text>().text = currentQuestion.ToString() + "/" + maxQuestions.ToString();
    }
}
