using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress
{
    public int highestScore;
    public QuestionAnswer questionAnswers;

    public QuestionAnswer GetQuestionAnswers()
    {
        return questionAnswers;
    }

}

