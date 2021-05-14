using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TextSpeech;

public class ConfigScreenController : MonoBehaviour
{
    public Toggle m_screenreader, m_highcontrast;
    public Slider m_slider_screenreader;
    public Button m_confirmButton, m_reportButton, m_restoreButton;

    private void Awake()
    {
        // get values from memory
        m_screenreader.isOn = PlayerPrefs.GetInt("ACCESSIBILITY") == 1 ? true : false;
        m_highcontrast.isOn = PlayerPrefs.GetInt("HIGH_CONTRAST") == 1 ? true : false;
        m_slider_screenreader.value = PlayerPrefs.GetFloat("SCREENREADER_SPEED");
    }

    private void Start()
    {
        m_screenreader.onValueChanged.AddListener((value) => {
            HandleScreenReader(value);
        });

        m_highcontrast.onValueChanged.AddListener((value) => {
            HandleHighContrast(value);
        });

        m_slider_screenreader.onValueChanged.AddListener((value) => {
            HandleSliderScreenReader(value);
        });

        m_confirmButton.onClick.AddListener(ReturnToMenu);
        m_reportButton.onClick.AddListener(ReturnToMenu);
        m_restoreButton.onClick.AddListener(ReturnToMenu);
    }

    void HandleScreenReader(bool value)
    {
        m_screenreader.isOn = value;
        AccessibilityController.Instance.SetAccessibilityParameter(m_screenreader.isOn);
        Debug.Log(AccessibilityController.Instance.ACCESSIBILITY);
    }

    void HandleHighContrast(bool value)
    {
        m_highcontrast.isOn = value;
        var isOn = value ? "Ativado" : "Desativado";
        
        AccessibilityController.Instance.SetHighContrastParameter(m_highcontrast.isOn);
        Debug.Log(AccessibilityController.Instance.HIGH_CONTRAST);
        SpeechController.Instance.StartSpeaking("O Alto contraste está " + isOn);
    }

    void HandleSliderScreenReader(float value)
    {
        m_slider_screenreader.value = value;

        SpeechController.Instance.Setup(value);
        SpeechController.Instance.StartSpeaking("A leitura está na velocidade " + value);

        // store in memory
        PlayerPrefs.SetFloat("SCREENREADER_SPEED", value);
    }

    void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScreen");
    }
}
