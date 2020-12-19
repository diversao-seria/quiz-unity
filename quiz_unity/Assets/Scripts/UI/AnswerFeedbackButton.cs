using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerFeedbackButton : MonoBehaviour
{
    // Start is called before the first frame update
    private ResultGameController resultGameController;

    public AnswerFeedbackData AnswerFeedbackData { get; set; }

    void Start()
    {
        resultGameController = FindObjectOfType<ResultGameController>();
    }

    public void HandleFeedbackClick()
    {
        resultGameController.GetComponent<ResultGameController>().GetSpecificFeedback(transform.gameObject);
        
    }
}
