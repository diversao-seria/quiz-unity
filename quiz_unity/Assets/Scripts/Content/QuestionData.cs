using System.Collections.Generic;
using Newtonsoft.Json;

public class QuestionData
{
    [JsonProperty("id")]
    public int ID { get; set; }

    [JsonProperty("title")]
    public string Text { get; set; }

    [JsonProperty("alternatives")]
    public List<Alternative> Alternatives { get; set; }
}