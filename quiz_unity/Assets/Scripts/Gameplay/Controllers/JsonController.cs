using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonController : MonoBehaviour
{
    public int score;
    public string time;
    public int rightAnswers;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
