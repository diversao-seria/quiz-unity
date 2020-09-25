using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPool : MonoBehaviour
{
    public GameObject resultPrefab;
    public Transform parentObject;

    public void InstantiateResults(QuestionAnswer questionAnswer, Transform transform)
    {
        for (int i = 0; i < questionAnswer.Answer.Length; i++)
        {
            GameObject gameObject = Instantiate(resultPrefab, transform);
            gameObject.transform.SetParent(parentObject.transform, false);
            SetDataOnResult(gameObject, questionAnswer, i);
        }
            
    }

    public void SetDataOnResult(GameObject gameObject, QuestionAnswer questionAnswer, int i)
    {

        if (questionAnswer.Result[i] == 'C')
        {
            gameObject.GetComponent<Image>().color = Color.green;
        }
        else if (questionAnswer.Result[i] == 'E')
        {
            gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.gray;
        }

        gameObject.GetComponentInChildren<Text>().text = questionAnswer.Answer[i].ToString();
    }
}
