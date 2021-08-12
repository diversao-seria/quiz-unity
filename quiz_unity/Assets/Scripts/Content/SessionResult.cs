using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionResult 
{
    [JsonProperty("score")]
    public int Score { get; set; }

    [JsonProperty("total_questions")]
    public int TotalQuestions { get; set; }

    [JsonProperty("total_correct_questions")]
    public int TotalCorrect { get; set; }

    [JsonProperty("total_time")]
    public int TotalTime { get; set; }

    [JsonProperty("questions_answered")]
    public List<QuestionAnswerData> QuestionAnswerDatas { get; set; } 
}
