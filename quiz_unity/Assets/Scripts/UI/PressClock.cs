using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressClock : IClock
{
    public float Time { get; set; }

    public PressClock(float initialTimeInSeconds)
    {
        this.Time = initialTimeInSeconds;
    }

    public void IncreaseTime(float deltaTimeInSeconds)
    {
        Time = Time + deltaTimeInSeconds;
    }

    public void DecreaseTime(float deltaTimeInSeconds)
    {
        Time = Time - deltaTimeInSeconds;
    }

    public void Reset()
    {
        Time = 0.0f;
    }

    public void NewCountdown(float initialTimeInSeconds)
    {
        Time = initialTimeInSeconds;
    }

    public string HHmmss()
    {
        return System.TimeSpan.FromSeconds((int)Time).ToString();
    }
}
