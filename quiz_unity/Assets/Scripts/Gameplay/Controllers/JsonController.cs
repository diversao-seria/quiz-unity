using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonController : MonoBehaviour
{
    public int score;
    public string startTime;
    public List<string> sequencia_atuacao = new List<string>();
    public int wrongAnswers;
    public int rightAnswers;
    public int hab1 = 0, hab2 = 0, hab3 = 0;
    public int streak;

    public int exit = 0;




    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
