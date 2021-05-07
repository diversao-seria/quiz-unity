using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.Android;


public class SpeechController : MonoBehaviour
{
    //const string LANG_CODE = "en-US";
    const string LANG_CODE = "pt-BR";
    float RATE = 1;

    public static SpeechController Instance
    {
        get
        {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SpeechController>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("SpeechController");
                        _instance = container.AddComponent<SpeechController>();
                        DontDestroyOnLoad(container);
                    }
                }
                return _instance;
        }
    }

    static SpeechController _instance;

    void Start()
    {
        Setup(LANG_CODE, RATE);

        TextToSpeech.Instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.Instance.onDoneCallback = OnSpeakStop;

        CheckPermission();
    }

    void CheckPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }

    void Setup(string code, float rate)
    {
        TextToSpeech.Instance.Setting(code, 1, rate);
        //StartSpeaking("Inicialização realizada para a localização " + code);
    }

    public void Setup(float rate)
    {
        TextToSpeech.Instance.Setting(LANG_CODE, 1, rate);
    }

    #region Text To Speech

    public void StartSpeaking(string message)
    {
        //Debug.Log("Started speaking: " + message);
        if (AccessibilityController.Instance.ACCESSIBILITY)
            TextToSpeech.Instance.StartSpeak(message);
    }

    public void StopSpeaking()
    {
        if (AccessibilityController.Instance.ACCESSIBILITY)
            TextToSpeech.Instance.StopSpeak();
    }

    void OnSpeakStart()
    {
        Debug.Log("Talking started...");
    }

    void OnSpeakStop()
    {
        Debug.Log("Talking stopped.");
    }

    #endregion
}
