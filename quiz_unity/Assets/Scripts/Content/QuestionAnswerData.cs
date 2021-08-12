using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionAnswerData
{
    [JsonProperty("question_id")]
    public int QuestionID { get; set; }

    [JsonProperty("selected_alternative")]
    public int SelectedAlternative { get; set; }

    [JsonProperty("correct")]
    public bool IsCorrect { get; set; }

    [JsonProperty("response_time")]
    public int ResponseTime { get; set; }

    [JsonProperty("help_used")]
    public string PowerUpUsed { get; set; }
}
