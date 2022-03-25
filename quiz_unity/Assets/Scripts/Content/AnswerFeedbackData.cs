using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerFeedbackData 
{
    public Color BackgroundColor { get; set; }
    public Color LabelColor { get; set; }

    public int QuestionNumber { get; set; }

    public string QuestionDescription { get; set; }

    public string QuestionAnswer { get; set; }

    public AnswerFeedbackData(Color backGroundColor, Color labelColor, int questionNumber, string questionDescription, string questionAnswer)
    {
        BackgroundColor = backGroundColor;
        LabelColor = labelColor;
        QuestionNumber = questionNumber;
        QuestionDescription = questionDescription;
        QuestionAnswer = questionAnswer;
    }

}
