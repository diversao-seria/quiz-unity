using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alternative : Question
{
    private QuestionType type;

    // Property

    public QuestionType Type { get; set; }

    public Alternative(int ID, string text, int alternativesNumber)
    {
        this.ID = ID;
        this.Text = text;
        this.AlternativesNumber = alternativesNumber;
        this.Type = QuestionType.Text;
    }



}
