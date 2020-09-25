using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz 
{
    private QuestionData questionData;
    private int _user_id;

    // private bool randomizeQuestions;
    public int Id { get; set; }
    public string Code { get; set; }

    public float Created_at { get; set; }

    public float Updated_at { get; set; }

    public Quiz(QuestionData questionData, string code)
    {
        this.Code = code;

        // Read from processed JSON.
        this.questionData = questionData;
    }

    public QuestionData GetQuestionData()
    {
        return questionData;
    }
}
