using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Alternative
{
    [JsonProperty("text")]
    public string Content { get; set; }

    [JsonProperty("correct")]
    public bool IsCorrect { get; set; }
}
