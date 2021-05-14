﻿using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TextSpeech;

public class MenuScreenController : MonoBehaviour
{
    public Button m_classic, m_survival, m_competition, m_configIcon;

    public Image m_profile, m_exit;

    public Image m_exitPopup;


    public GameObject TransitionCanvas;
    private Animator animator;
    private float transitionDelay = 1f;
    public EventSystem eventSystem;



    void Awake()
    {
        m_classic.onClick.AddListener(ClassicBehaviour);
        m_survival.onClick.AddListener(SurvivalBehaviour);
        m_competition.onClick.AddListener(CompetitionBehaviour);
        m_configIcon.onClick.AddListener(ConfigurationBehaviour);
    }

    void Update()
    {

    }

    void ClassicBehaviour()
    {
        SpeechController.Instance.StartSpeaking(m_classic.GetComponentInChildren<Text>().text);
        TextToSpeech.Instance.StartSpeak("Pre Game");
        StartCoroutine(TransitionAnimation("PreGame"));
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
        Debug.Log("Carregue Survival");
    }

    void CompetitionBehaviour()
    {
        SpeechController.Instance.StartSpeaking(m_competition.GetComponentInChildren<Text>().text);
        Debug.Log(" Carregue Competição");
    }

    void ConfigurationBehaviour()
    {
        SpeechController.Instance.StartSpeaking(m_configIcon.GetComponentInChildren<Text>().text);
        TextToSpeech.Instance.StartSpeak("Configurações");
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
}