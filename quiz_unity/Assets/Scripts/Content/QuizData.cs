using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizData 
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("questions")]
    public List<Question> QuestionsData { get; set; }


    private QuestionData questionData { get; set; }

    public QuestionData GetQuestionData()
    {
        return questionData;
    }

    public void SetQuestionData(QuestionData questionData)
    {
        this.questionData = questionData;
    }
}
