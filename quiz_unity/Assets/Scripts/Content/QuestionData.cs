using System.Collections.Generic;
using Newtonsoft.Json;

public class QuestionData
{
    [JsonProperty("id")]
    public int ID { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("QuestionTime")]
    public float QuestionTime { get; set; }

    [JsonProperty("questions")]
    public List<Question> Questions { get; set; }
}