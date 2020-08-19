using System.Collections.Generic;
using Newtonsoft.Json;

public class QuestionData
{
    [JsonProperty("questions")]
    public List<Question> Questions { get; set; }
}