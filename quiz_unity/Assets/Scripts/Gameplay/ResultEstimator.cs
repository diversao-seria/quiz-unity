using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class ResultEstimator
{
    // All estimate of the student's perfomance should go on this class.
    public enum TextualResult
    {
        Ruim,
        Mediano,
        Bom,
        Perfeito
    }

    /* This is the function that is called in other scripts.
     * Use of pre-defined percentage values as to validate screen,
     * but further discussion on how to estimate the minimum to be considered
     * each result needs to be discussed. 
     */
     
    public static string getResultEstimate(QuestionAnswer questionAnswer)
    {
        return AverageEstimate(questionAnswer);
    }

    private static string AverageEstimate(QuestionAnswer questionAnswer)
    {
        int correctNumber = questionAnswer.NumberOfCorrects;
        int numberOfQuestions = questionAnswer.Answer.Length;

        float[] minimumPercentage = new float[Enum.GetNames(typeof(TextualResult)).Length];

        // Pre-defined values. if any estimate needs to be performed, change defineMinimumForResult
        SetMinimumPercentage(minimumPercentage, DefineMinimumforResult());


        float playerPercentage = (float) correctNumber / numberOfQuestions;
        string resultString = ((TextualResult) 0).ToString(); // Ruim


        for (int i = 1; i < Enum.GetValues(typeof(TextualResult)).Length; i++)
        {
            if(playerPercentage < minimumPercentage[i])
            {
                break;
            }
            resultString = ((TextualResult) i).ToString();
        }

        return resultString;
    }

    /*
     * Definition of minimum percentage of correct questions for textual result
     */
    private static float[] DefineMinimumforResult()
    {
        float[] values = { 0.0f, 0.4f, 0.6f, 0.9f };
        return values;
    }

    private static void SetMinimumPercentage(float[] minimumPercentage, float[] values)
    {
        for(int i = 0; i < minimumPercentage.Length; i++)
        {
            minimumPercentage[i] = values[i];
        }
    }
}
