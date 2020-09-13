using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz 
{
    private string code;
    private QuestionData questionData;
    // private bool randomizeQuestions;
    public string Code { get; set; }

    public Quiz(QuestionData questionData, string code)
    {
        this.code = code;

        // Read from processed JSON.
        this.questionData = questionData;
    }

    public QuestionData GetQuestionData()
    {
        return questionData;
    }
}
