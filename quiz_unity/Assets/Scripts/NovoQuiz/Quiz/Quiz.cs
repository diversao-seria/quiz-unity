using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz
{
    private string code;
    private Question[] questions;

    public string Code { get; set; }

    public Quiz(Question[] questions)
    {
        if (questions != null)
        {
            this.questions = questions;
        }
        else 
        {
            this.questions = null;
        }
       // Read from processed JSON.
    }

    public Question[] GetQuestions()
    {
        return questions;
    }
}
