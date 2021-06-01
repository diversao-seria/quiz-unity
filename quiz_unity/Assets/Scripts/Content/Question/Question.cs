using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Question
{
    [JsonProperty("id")]
    public int ID { get; set; }

    [JsonProperty("title")]
    public string Text { get; set; }

    [JsonProperty("alternatives")]
    public List<Alternative> Alternatives { get; set; }

}
