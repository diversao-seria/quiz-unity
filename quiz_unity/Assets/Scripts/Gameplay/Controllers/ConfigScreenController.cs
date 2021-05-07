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
        AccessibilityController.Instance.SetHighContrastParameter(m_highcontrast.isOn);
        Debug.Log(AccessibilityController.Instance.HIGH_CONTRAST);
    }

    void HandleSliderScreenReader(float value)
    {
        m_slider_screenreader.value = value;
        SpeechController.Instance.Setup(value);

        // store in memory
        PlayerPrefs.SetFloat("SCREENREADER_SPEED", value);
    }
}
