using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchHistoryHandler : MonoBehaviour
{
    public Text code, date, performance;
    public void setText(string codetxt, string datetxt, string performancetxt)
    {
        code.text = codetxt;
        date.text = datetxt;
        performance.text = performancetxt;
    }
}
