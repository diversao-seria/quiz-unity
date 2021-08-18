using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

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
    public GameObject questionNumberText;
    public GameObject questionDescriptionText;
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
        SendPlayerData(dataController.QuizCode);
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
                centralStar.GetComponent<Image>().sprite = litStar;
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
            dataController.GetComponent<DataController>().RetrieveQuiz().Questions
            , questionAnswer, 
            lowerWrapper.transform);


    }

    public void GetSpecificFeedback(GameObject alternativeFeedback)
    {
        if (alternativeFeedback == null)
            Debug.Log("Square colored object isnull!");
        AnswerFeedbackData answerFeedback = alternativeFeedback.GetComponent<AnswerFeedbackButton>().AnswerFeedbackData;

        // Set backgroundColor
        especificFeedbackWrapper.GetComponent<Image>().color = answerFeedback.BackgroundColor;

        // SetLabelCorlor (2)
        questionNumberText.GetComponent<Text>().text = "Questão " + answerFeedback.QuestionNumber.ToString();

        questionNumberBox.GetComponent<Image>().color = answerFeedback.LabelColor;

        returnToResultsButton.GetComponent<Image>().color = answerFeedback.LabelColor;
        // SetQuestionDescrtiption

        questionDescriptionText.GetComponent<Text>().text = answerFeedback.QuestionDescription;

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

    public void SendPlayerData(string quizCode)
    {
        StartCoroutine(SendForm(quizCode));
    }

    IEnumerator SendForm(string quizCode)
    {

        string url = "http://ds-quiz.herokuapp.com/matches";

        // List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // formData.Add(new MultipartFormDataSection("ID=" + exampleID.ToString() + "&" + "TemponoQuiz=" + exampleTime + "&" + "RespostasCorretas=" + exampleCorrect.ToString()));
        // formData.Add(new MultipartFormFileSection("my file data", pathToMatchData));

        string pathToQuizResult = Path.Combine(Application.persistentDataPath + Path.AltDirectorySeparatorChar +
            DataManagementConstant.PlayerDataPath + quizCode + Path.AltDirectorySeparatorChar + "QuizAnswerData" + ".json");

        string jsonData = null;

        try 
        {
            jsonData = System.IO.File.ReadAllText(pathToQuizResult);
        }
        catch(IOException e)
        {
            Debug.Log(e);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            yield return www.SendWebRequest();
            Debug.Log("Status Code: " + www.responseCode);

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
