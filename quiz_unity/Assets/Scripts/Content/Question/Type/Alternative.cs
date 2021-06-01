using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Alternative
{
    [JsonProperty("id")]
    public int ID { get; set; }

    [JsonProperty("text")]
    public string Content { get; set; }

    [JsonProperty("QuestionType")]
    public int QuestionType { get; set; }

    [JsonProperty("correct")]
    public bool IsCorrect { get; set; }
}
