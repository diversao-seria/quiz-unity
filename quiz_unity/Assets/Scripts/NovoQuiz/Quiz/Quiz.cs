using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz
{
    private string code;
    private QuestionData questionData;

    public string Code { get; set; }

    public Quiz(QuestionData questionData, string code)
    {
        this.code = code;
        this.questionData = questionData;
       // Read from processed JSON.
    }

    public QuestionData GetQuestionData()
    {
        return questionData;
    }
}
