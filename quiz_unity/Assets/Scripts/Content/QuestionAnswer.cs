using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionAnswer
{
    public char[] Answer { get; set; }
    public char[] Result { get; set; }

    public QuestionAnswer(int n)
    {
        Answer = new char[n];
        Result = new char[n];
        InitializeResults(n);
    }

    private void InitializeResults(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Answer[i] = '-';
            Result[i] = 'N';
        }
    }

    private void SetPlayerAnswer(char answer, char result, int i)
    {
        Answer[i] = answer;
        Result[i] = result;
    }

    private char GetAnswerFromInt(int x)
    {
        switch (x)
        {
            case 0:
                return 'A';
            case 1:
                return 'B';
            case 2:
                return 'C';
            case 3:
                return 'D';
            default:
                return '-';
        }

    }

    private bool OutOfTimeWhileAnswering(EventManager eventManager)
    {
        return eventManager.TouchLastStatus();
    }


    public void RegisterPlayerAnswer(EventManager eventManager, bool isAnswered, bool isCorrect, int q, int a, float timeLeft)
    {

        if (!eventManager) return;

        char answer = GetAnswerFromInt(a);

        if(isAnswered)
        {
            if(isCorrect)
            {
                SetPlayerAnswer(answer, 'C', q);
            }
            else
            {
                SetPlayerAnswer(answer, 'E', q);
            }
        }
        else
        {
          if(OutOfTimeWhileAnswering(eventManager))
            {
                if (isCorrect)
                {
                    SetPlayerAnswer(answer, 'C', q);
                }
                else
                {
                    SetPlayerAnswer(answer, 'E', q);
                }
            }
        }
    }
}
