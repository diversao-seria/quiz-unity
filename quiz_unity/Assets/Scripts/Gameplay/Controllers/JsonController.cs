using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonController : MonoBehaviour
{
    private string[] RegisteredHabilityList { get; set; }

    private string jsonResult;

    // Root object for JSON's player data
    QuizResultData quizResultData = null;

    // Objetct for with overall data
    SessionResult sessionResult = null;

    // Each question data.
    List<QuestionAnswerData> questionAnswerDataList = null;

    public void SetNewQuizResultData()
    {

        RegisteredHabilityList = new string[GameMechanicsConstant.PowerUpCount];
        ResetFieldsForNextQuestion();

        if(quizResultData == null)
        {
            //handler;
            quizResultData = new QuizResultData();

            quizResultData.SessionResult = new SessionResult();
            // Handler
            sessionResult = quizResultData.SessionResult;

            quizResultData.SessionResult.QuestionAnswerDatas = new List<QuestionAnswerData>();
            // Handler
            questionAnswerDataList = quizResultData.SessionResult.QuestionAnswerDatas;
        }
    }

    public void UsedPowerUp(GameMechanicsConstant.PowerUpNames powerUp)
    {
        RegisteredHabilityList[(int)powerUp] = "1";
    }

    public void UpdateScore(int points)
    {
        sessionResult.Score = sessionResult.Score + points;
    }

    public void UpdateNumberOfCorrectQuestions()
    {
        sessionResult.TotalCorrect++;
    }

    public void UpdateTotalTime(int timeElapsed)
    {
        sessionResult.TotalTime += timeElapsed; 
    }

    public void SetTotalNumberOfQuestions(int totalQuestions)
    {
        sessionResult.TotalQuestions = totalQuestions;
    }

    public void SetQuizID(int id)
    {
        quizResultData.QuizID = id;
    }

    public void RegisterStartTime()
    {
        quizResultData.StartTime = System.DateTime.Now.ToString();
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    private void ResetFieldsForNextQuestion()
    {
        for (int i = 0; i < RegisteredHabilityList.Length; i++)
        {
            RegisteredHabilityList[i] = "0";
        }
    }

    public void FinishSetQuizData()
    {
        quizResultData = null;
    }

    public string SerializeAnswerData()
    {
        return JsonConvert.SerializeObject(quizResultData, Formatting.Indented);
    }

    public void AddNewAnsweredQuestion(int questionID, int selectedAlternative, bool isCorrect, int responseTimeInSeconds)
    {
        questionAnswerDataList.Add(new QuestionAnswerData());
        questionAnswerDataList[questionAnswerDataList.Count - 1].QuestionID = questionID;
        questionAnswerDataList[questionAnswerDataList.Count - 1].SelectedAlternative = selectedAlternative;
        questionAnswerDataList[questionAnswerDataList.Count - 1].IsCorrect = isCorrect;
        questionAnswerDataList[questionAnswerDataList.Count - 1].ResponseTime = responseTimeInSeconds;
        questionAnswerDataList[questionAnswerDataList.Count - 1].PowerUpUsed = 
            RegisteredHabilityList[(int)GameMechanicsConstant.PowerUpNames.Water] +
            RegisteredHabilityList[(int)GameMechanicsConstant.PowerUpNames.Air] +
            RegisteredHabilityList[(int)GameMechanicsConstant.PowerUpNames.Earth];
        ResetFieldsForNextQuestion();
    }

    public void DEBUGPlayerJSONData()
    {
        Debug.Log("--------- DEBUG JSON PLAYER DATA ---------");
        Debug.Log("quiz_id: " + quizResultData.QuizID);
        Debug.Log("StartTime: " + quizResultData.StartTime);
        Debug.Log(">> score: " + sessionResult.Score);
        Debug.Log(">> total_questions: " + sessionResult.TotalQuestions);
        Debug.Log(">> total_correct: " + sessionResult.TotalCorrect);
        Debug.Log(">> total_TotalTime: " + sessionResult.TotalTime);
        Debug.Log("<-->");
        foreach ( QuestionAnswerData qSD in questionAnswerDataList) 
        {
            Debug.Log(">>>> question_id" + qSD.QuestionID);
            Debug.Log(">>>> selected_alterantive" + qSD.SelectedAlternative);
            Debug.Log(">>>> question_id" + qSD.IsCorrect);
            Debug.Log(">>>> ResposneTime" + qSD.ResponseTime);
            Debug.Log(">>>> question_id" + qSD.PowerUpUsed);
            Debug.Log("<-->");
        }
    }
}
