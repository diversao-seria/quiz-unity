using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Alternative
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("QuestionType")]
    public int QuestionType { get; set; }

    [JsonProperty("isCorrect")]
    public bool IsCorrect { get; set; }
}
