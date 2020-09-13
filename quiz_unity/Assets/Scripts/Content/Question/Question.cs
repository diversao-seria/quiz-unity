using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Question
{
    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("alternativesNumber")]
    public int AlternativesNumber { get; set; }

    [JsonProperty("alternatives")]
    public List<Alternative> Alternatives { get; set; }

}
