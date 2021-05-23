using System.Collections.Generic;
using Newtonsoft.Json;

public class QuestionData
{
    [JsonProperty("QuestionTime")]
    public float QuestionTime { get; set; }

    [JsonProperty("questions")]
    public List<Question> Questions { get; set; }
}