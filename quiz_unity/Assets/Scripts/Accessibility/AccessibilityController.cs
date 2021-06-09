using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessibilityController : MonoBehaviour
{

    static AccessibilityController _instance;
    public static AccessibilityController Instance
    {
        get
        {
            if (_instance == null)
            {
                Init();
            }
            return _instance;
        }
    }

    private bool _accessibility;
    public bool ACCESSIBILITY
    {
        get
        {
            return _accessibility;
        }
        private set
        {
            _accessibility = value;
        }
    }

    private bool _high_contrast;
    public bool HIGH_CONTRAST
    {
        get
        {
            return _high_contrast;
        }
        private set
        {
            _high_contrast = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // debug
        //DontDestroyOnLoad(gameObject);

        // load values from memory
        ACCESSIBILITY = PlayerPrefs.GetInt("ACCESSIBILITY") == 1 ? true : false;
        HIGH_CONTRAST = PlayerPrefs.GetInt("HIGH_CONTRAST") == 1 ? true : false;


        Init();
    }

    public static void Init()
    {
        if (_instance == null)
        {
            GameObject container = new GameObject("AccessibilityController");
            _instance = container.AddComponent<AccessibilityController>();
            DontDestroyOnLoad(container);
        }
    }

    public void SetAccessibilityParameter(bool value)
    {
        ACCESSIBILITY = value;
        
        // also store value on memory
        var value_int = value == true ? 1 : 0;
        PlayerPrefs.SetInt("ACESSIBILITY", value_int);
    }

    public void SetHighContrastParameter(bool value)
    {
        HIGH_CONTRAST = value;

        // also store value on memory
        var value_int = value == true ? 1 : 0;
        PlayerPrefs.SetInt("HIGH_CONTRAST", value_int);
    }
}
