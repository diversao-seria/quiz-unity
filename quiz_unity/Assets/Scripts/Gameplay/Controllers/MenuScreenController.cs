using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;

public class MenuScreenController : MonoBehaviour
{
    public Button m_classic, m_survival, m_competition, m_config;

    public Image m_profile, m_exit;

    public Image m_exitPopup;


    public GameObject TransitionCanvas;
    private Animator animator;
    private float transitionDelay = 1f;
    public EventSystem eventSystem;

    //experimental variables
    private int volume = 0;
    private int brightness = 0;
    private bool yn = true;


    void Awake()
    {
        m_classic.onClick.AddListener(ClassicBehaviour);
        m_survival.onClick.AddListener(SurvivalBehaviour);
        m_competition.onClick.AddListener(CompetitionBehaviour);
        m_config.onClick.AddListener(ConfigurationBehaviour);
        //Simulating data loading
        Load();
    }

    void Update()
    {

    }

    void ClassicBehaviour()
    {
        SpeechController.Instance.StartSpeaking(m_classic.GetComponentInChildren<Text>().text);
        StartCoroutine(TransitionAnimation("PreGame"));
        //Simulating data saving
        volume++;
        brightness++;
        yn = !yn;
        Save();
    }

    IEnumerator TransitionAnimation(string scene)
    {
        animator = TransitionCanvas.GetComponent<Animator>();
        animator.SetTrigger("TransitionTrigger");
        yield return new WaitForSeconds(transitionDelay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    void SurvivalBehaviour()
    {
        SpeechController.Instance.StartSpeaking(m_survival.GetComponentInChildren<Text>().text);
        Debug.Log(" Carregue Survival");
    }

    void CompetitionBehaviour()
    {
        SpeechController.Instance.StartSpeaking(m_competition.GetComponentInChildren<Text>().text);
        Debug.Log(" Carregue Competição");
    }

    void ConfigurationBehaviour()
    {
        SpeechController.Instance.StartSpeaking(m_config.GetComponentInChildren<Text>().text);
        StartCoroutine(TransitionAnimation("ConfigScreen"));
    }


    public void InitialExitBehaviour()
    {
        Button[] buttons = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<Button>();

        // Make anything child of canvas not interactable.
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }


        ToggleObjectsFromParent(m_exitPopup.transform, true);
    }

    public void ToggleObjectsFromParent(Transform transform, bool boolean)
    {
        transform.gameObject.SetActive(boolean);
        foreach(Transform child in transform)
        {
            ToggleObjectsFromParent(child, boolean);
        }
    }

    void ProfileBehaviour()
    {
        Debug.Log("Profile creation/edit");
    }

    void Save()
    {
        string path = Application.streamingAssetsPath + "\\settings.json";

        Settings settings = new Settings(); 
        settings.volume = volume;
        settings.brightness = brightness;
        settings.yn = yn;

        string[] Lines = File.ReadAllLines(path); // Replacing the first line of the settings file
        Lines[0] = settings.ReturnSettings();
        File.WriteAllLines(path, Lines);
    }
    void Load()
    {
        string path = Application.streamingAssetsPath + "\\settings.json";

        if (File.Exists(path))
        {
            Settings settingsData = new Settings();
            StreamReader reader = new StreamReader(path);
            settingsData = JsonUtility.FromJson<Settings>(reader.ReadLine());
            reader.Close();
            volume = settingsData.volume;
            brightness = settingsData.brightness;
            yn = settingsData.yn;
        }
        else
        {
            StreamWriter writer = File.CreateText(path);
            writer.WriteLine("Placeholder Text");
            writer.Close();
        }
        
    }
}

public class Settings
{
    //save class with experimental variables

    public int volume = 0;
    public int brightness = 0;
    public bool yn = true;
    
   
    public string ReturnSettings()
    {
        return JsonUtility.ToJson(this);
    }
}

