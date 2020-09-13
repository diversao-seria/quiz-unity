using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultGameController : MonoBehaviour
{

    private DataController dataController;
    private QuestionAnswer questionAnswer;
    public GameObject m_prefab_container;
    public GameObject lowerWrapper;

    // Start is called before the first frame update

    void Awake()
    {
        dataController = FindObjectOfType<DataController>();
        questionAnswer = dataController.GetQuestionAnswers();
        generateAnswers();
    }

    void Start()
    {
        ShowPlayerAnswers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowPlayerAnswers()
    {

    }

    void generateAnswers()
    {
        for(int i = 0; i < dataController.RetrieveQuiz().GetQuestionData().Questions.Count; i++)
        {
            GameObject newFeedback = Instantiate(m_prefab_container);
            newFeedback.transform.SetParent(lowerWrapper.transform, false);
            
            if (questionAnswer.Result[i] == 'C')
            {
                newFeedback.GetComponent<Image>().color = Color.green;
            }
            else if(questionAnswer.Result[i] == 'E')
            {
                newFeedback.GetComponent<Image>().color = Color.red;
            }
            else
            {
                newFeedback.GetComponent<Image>().color = Color.gray;
            }

            newFeedback.gameObject.GetComponentInChildren<Text>().text = questionAnswer.Answer[i].ToString();
        }
    }

}
