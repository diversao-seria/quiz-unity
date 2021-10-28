using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizResultData 
{
    [JsonProperty("quiz_id")]
    public int QuizID { get; set; }

    [JsonProperty("player_id")]
    public int PlayerID { get; set; }

    [JsonProperty("start_time")]
    public string StartTime { get; set; }

    [JsonProperty("results")]
    public SessionResult SessionResult { get; set; }

}
