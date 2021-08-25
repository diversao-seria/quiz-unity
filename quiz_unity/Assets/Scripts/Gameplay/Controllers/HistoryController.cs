using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Newtonsoft.Json;

public class HistoryController : MonoBehaviour
{
    public GameObject matchPrefab;
    public GameObject history;
    private historyInfo currentQuiz;
    private List<Text> historyTexts = new List<Text>();
    
    void Start()
    {
        string path = Application.persistentDataPath + @"/PlayerData";
        string[] matches = Directory.GetDirectories(path);
        foreach (var match in matches)
        {

            string json = File.ReadAllText(match + "\\QuizAnswerData.json");

            historyInfo currentQuiz = Newtonsoft.Json.JsonConvert.DeserializeObject<historyInfo>(json);

            GameObject historyEntry = Instantiate(matchPrefab, new Vector3(0, 0, 0), Quaternion.identity, history.transform);
            historyEntry.GetComponent<MatchHistoryHandler>().setText(match.Split(new char[] { '/', '\\' })[match.Split(new char[] { '/', '\\' }).Length - 1].ToString(), currentQuiz.start_time, currentQuiz.results.total_correct_questions.ToString() + " / " + currentQuiz.results.total_questions.ToString());
        }

    }  
}

[System.Serializable]
public class historyInfo
{
    public string start_time;
    public int quiz_id;
    public Result results;
}

[System.Serializable]
public class Result
{
    public int score;
    public int total_questions;
    public int total_correct_questions;
    public int total_time;
    public List<Answered> questions_answered;
}

[System.Serializable]
public class Answered
{
    public int question_id;
    public int selected_alternative;
    public bool correct;
    public int response_time;
    public string help_used;
}




