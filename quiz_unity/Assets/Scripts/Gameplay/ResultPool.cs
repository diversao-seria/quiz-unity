using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPool : MonoBehaviour
{
    public GameObject resultPrefab;
    public Transform parentObject;

    public Sprite correctImage;
    public Sprite wrongImage;
    public Sprite timesUpImage;

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
            sLabelColor = new Color(0.06f, 0.56f, 0.23f);
            //gameObject.GetComponent<Image>().color = sLabelColor;
            sBackgroundColor = new Color(0.93f, 0.99f, 0.78f);
            gameObject.GetComponent<Image>().sprite = correctImage;
            
        }
        else if (questionAnswer.Result[i] == 'E')
        {
            //Debug.Log("Definindo cores");
            sLabelColor = new Color(0.89f, 0.18f, 0.14f);
            //gameObject.GetComponent<Image>().color = sLabelColor;
            sBackgroundColor = new Color(0.98f, 0.86f, 0.83f);
            gameObject.GetComponent<Image>().sprite = wrongImage;
        }
        else
        {
            sLabelColor = new Color(0.41f, 0.52f, 0.57f);
            //gameObject.GetComponent<Image>().color = sLabelColor;
            sBackgroundColor = new Color(0.89f, 0.96f, 0.96f);
            gameObject.GetComponent<Image>().sprite = timesUpImage;
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
