using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStamp
{
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }

    public TimeStamp(int hour, int minutes, int seconds)
    {
        Hours = hour;
        Minutes = minutes;
        Seconds = seconds;
    }

    public string RetrieveStringFormat()
    {
        return Hours.ToString() + ":" + Minutes.ToString() + ":" + Seconds.ToString();
    }
}
