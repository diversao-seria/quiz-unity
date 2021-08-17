using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontController : MonoBehaviour
{
    [SerializeField]
    private Font[] fonts;

    private Text[] texts;
    private bool foundText = false;

    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        foundText = false;
        Debug.Log(foundText);
        Debug.Log(fonts.Length);
    }


    public void ChangeFont()
    {
        foreach (Text t in texts)
        {
            t.font = fonts[counter];
        }
    }

    public void ChangeFontSize(bool increase)
    {
        // nao eh tao simples assim, necessario alterar o RectTransform para acomocar o aumento da fonte.
        foreach (Text t in texts)
        {
            if (increase)
                t.fontSize++;
            else
                t.fontSize--;
        }
    }

    public void FindAllTexts()
    {
        texts = FindObjectsOfType<Text>();
        foreach (Text t in texts)
            Debug.Log(t.text);

        foundText = true;
    }

    private void Update()
    {
        if (!foundText) FindAllTexts();

        if (Input.GetMouseButtonDown(0))
        {
            counter++;
            Debug.Log(fonts.Length);
            if (counter > fonts.Length - 1)
            {
                Debug.Log(counter);
                counter = 0;
            }

            ChangeFont();
            ChangeFontSize(true);
        }
    }
}