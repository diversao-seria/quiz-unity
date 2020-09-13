using System.Collections;
using System.Collections.Generic;
using System;

public static class Randomizer 
{
    public static void RandomizeAlternatives(IList<Question> questionList)
    {

        Random rng = new Random();

        for (int i = 0; i < questionList.Count; i++)
        {
            // Fisher-Yates Shuffle.
            int n = questionList[i].Alternatives.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Alternative temp = questionList[i].Alternatives[k];
                questionList[i].Alternatives[k] = questionList[i].Alternatives[n];
                questionList[i].Alternatives[n] = temp;
            }
        }
    }

    public static void RandomizeAll(Quiz selectedQuiz)
    {
        // TO DO.
    }
}
