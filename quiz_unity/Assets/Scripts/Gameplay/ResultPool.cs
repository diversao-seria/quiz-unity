using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPool : MonoBehaviour
{
    public GameObject resultPrefab;
    public Transform parentObject;

    public void InstantiateResults(List<Question> questions, QuestionAnswer questionAnswer, Transform transform)
    {
        for (int i = 0; i < questionAnswer.Answer.Length; i++)
        {
            GameObject gameObject = Instantiate(resultPrefab, transform);
            gameObject.transform.SetParent(parentObject.transform, false);
            SetDataOnResult(gameObject, questions[i].Text, questionAnswer, i);

        }           
    }

    public void SetDataOnResult(GameObject gameObject, string questionDescription, QuestionAnswer questionAnswer, int i)
    {

        Color sBackgroundColor;
        Color sLabelColor;

        if (questionAnswer.Result[i] == 'C')
        {
            sLabelColor = Color.green;
            gameObject.GetComponent<Image>().color = sLabelColor;
            sBackgroundColor = new Color(0.8f, 1.0f, 0,8f);
            
        }
        else if (questionAnswer.Result[i] == 'E')
        {
            Debug.Log("Definindo cores");
            sLabelColor = Color.red;
            gameObject.GetComponent<Image>().color = sLabelColor;
            sBackgroundColor = new Color(1.0f, 0.8f, 1.0f);
        }
        else
        {
            sLabelColor = Color.gray;
            gameObject.GetComponent<Image>().color = sLabelColor;
            sBackgroundColor = new Color(0.9f, 0.9f, 0.9f);
        }

        gameObject.GetComponent<AnswerFeedbackButton>().AnswerFeedbackData = new AnswerFeedbackData(
            sBackgroundColor,
            sLabelColor,
            i + 1,
            questionDescription,
            "Explicação da resposta ainda está em desenvolvimento."
            );

        gameObject.GetComponentInChildren<Text>().text = questionAnswer.Answer[i].ToString();
    }
}
