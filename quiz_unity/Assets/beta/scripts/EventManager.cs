using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{

    //
    // ENQUANTO Enquanto botão está apertado :
    // -- aumenta o valor do slider
    // -- if(slider >= 1.0) próxima pergunta (cena)
    // FIM-ENQUANTO
    // slider = 0.0;

    private Clock holdTouchClock;
    private bool wasTouched;
    private bool answerRegistred;
    public float answerConfirmation = 3.0f;   // in seconds
    public Slider progressSlider;

    public void Awake()
    {
        wasTouched = false;
        answerRegistred = false;
        holdTouchClock = new Clock(0);
    }

    public void Update()
    {
        if(wasTouched && !answerRegistred)
        {

            holdTouchClock.IncreaseTime(Time.deltaTime);
            progressSlider.value = Math.Min(holdTouchClock.Time / answerConfirmation, 1);

            if(holdTouchClock.Time >= answerConfirmation)
            {
                // ans
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }
            else
            {
                // TO DO-> CONSIDERAR POSIÇÂO DO TOUCH COM RELAÇÂO Ao tamanho do butão.
                if (Input.touchCount <= 0)
                {
                    wasTouched = false;
                    progressSlider.value = 0;
                    progressSlider.gameObject.SetActive(false);
                    holdTouchClock.Reset();
                }
            }
        }
    }

    public void buttonTouched()
    {
        wasTouched = true;
        progressSlider.gameObject.SetActive(true);
    }
}
