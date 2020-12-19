using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultGameController : MonoBehaviour
{

    private DataController dataController;
    private QuestionAnswer questionAnswer;
    private ResultPool resultPool;

    // Result Screen
    public GameObject m_prefab_container;
    public GameObject lowerWrapper;
    public GameObject upperWrapper;
    public GameObject starWrapper;
    public Sprite litStar;

    private GameObject leftStar;
    private GameObject centralStar;
    private GameObject rightStar;

    // Specific Feedback
    public GameObject especificFeedbackWrapper;
    public GameObject questionNumberBox;
    public GameObject questionDesctiptionBox;
    public GameObject questionAnswerText;
    public GameObject returnToResultsButton;



    void Awake()
    {

        leftStar = starWrapper.transform.Find("LeftStar").gameObject;
        centralStar = starWrapper.transform.Find("CentralStar").gameObject;
        rightStar = starWrapper.transform.Find("RightStar").gameObject;



        dataController = FindObjectOfType<DataController>();
        questionAnswer = dataController.GetQuestionAnswers();
        resultPool = FindObjectOfType<ResultPool>();
        generateAnswers();

    }

    void Start()
    {
        ShowPlayerAnswers();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player clicks one of the boxes? // TO DO
    }

    void ShowPlayerAnswers()
    {
        GameObject questionsResultText = upperWrapper.transform.Find("QuestionsResultText").gameObject;
        questionsResultText.GetComponent<Text>().text = questionAnswer.NumberOfCorrects.ToString() + "/" + questionAnswer.Answer.Length;

        string resultText = ResultEstimator.getResultEstimate(questionAnswer);
        GameObject calculatedResultText = upperWrapper.transform.Find("CalculatedResultText").gameObject;
        calculatedResultText.GetComponent<Text>().text = resultText;

        updateStars(resultText);
    }

    void updateStars(string resultText)
    {
        int starCount = CalculateNumberOfStars(resultText);

        switch (starCount)
        {
            case 1:
                leftStar.GetComponent<Image>().sprite = litStar;
                break;
            case 2:
                leftStar.GetComponent<Image>().sprite = litStar;
                rightStar.GetComponent<Image>().sprite = litStar;
                break;
            case 3:
                leftStar.GetComponent<Image>().sprite = litStar;
                rightStar.GetComponent<Image>().sprite = litStar;
                centralStar.GetComponent<Image>().sprite = litStar;
                break;
            default:
                break;
        }
    }

    int CalculateNumberOfStars(string resultText)
    {
        int starCount = 0;

        switch (resultText)
        {
            case "Mediano":
                starCount = 1;
                break;
            case "Bom":
                starCount = 2;
                break;
            case "Perfeito":
                starCount = 3;
                break;
            default:
                break;
        }

        return starCount;
    }

    void generateAnswers()
    {
        resultPool.GetComponent<ResultPool>().InstantiateResults(
            dataController.GetComponent<DataController>().RetrieveQuiz().GetQuestionData().Questions
            , questionAnswer, 
            lowerWrapper.transform);
    }

    public void GetSpecificFeedback(GameObject alternativeFeedback)
    {
        AnswerFeedbackData answerFeedback = alternativeFeedback.GetComponent<AnswerFeedbackButton>().AnswerFeedbackData;

        // Set backgroundColor
        especificFeedbackWrapper.GetComponent<Image>().color = answerFeedback.BackgroundColor;

        // SetLabelCorlor (2)
        questionNumberBox.GetComponentInChildren<Text>().text = "Questão " + answerFeedback.QuestionNumber.ToString();
        questionNumberBox.GetComponent<Image>().color = answerFeedback.LabelColor;

        returnToResultsButton.GetComponent<Image>().color = answerFeedback.LabelColor;
        // SetQuestionDescrtiption

        questionDesctiptionBox.GetComponentInChildren<Text>().text = answerFeedback.QuestionDescription;

        //TO DO: SetQuestionExplanation

        SwapToSpecificFeedback();
    }

    public void SwapToSpecificFeedback()
    {        
        ToggleObject(especificFeedbackWrapper.transform,true);
    }

    public void SwapToGeneralsResults()
    {
        ToggleObject(especificFeedbackWrapper.transform, false);
    }

    private void ToggleObject(Transform transform, bool flag)
    {
        transform.gameObject.SetActive(flag);
        foreach(Transform child in transform)
        {
            ToggleObject(child,flag);
        }
    }
}
