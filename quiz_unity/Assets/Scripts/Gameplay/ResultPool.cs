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
            sLabelColor = new Color(0.21f, 0.71f, 0.30f);
            gameObject.GetComponent<Image>().color = sLabelColor;
            sBackgroundColor = new Color(0.21f, 0.71f, 0.30f);
            
        }
        else if (questionAnswer.Result[i] == 'E')
        {
            Debug.Log("Definindo cores");
            sLabelColor = new Color(0.73f, 0.21f, 0.21f);
            gameObject.GetComponent<Image>().color = sLabelColor;
            sBackgroundColor = new Color(0.73f, 0.21f, 0.21f);
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

        //gameObject.GetComponentInChildren<Text>().text = questionAnswer.Answer[i].ToString();
        gameObject.GetComponentInChildren<Text>().text = (i+1).ToString();
    }
}
