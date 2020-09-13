using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IClock
{
    void IncreaseTime(float deltaTimeInSeconds);
    void DecreaseTime(float deltaTimeInSeconds);
    void Reset();
    void NewCountdown(float initialTimeInSeconds);
    string HHmmss();
}
