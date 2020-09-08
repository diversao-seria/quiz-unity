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
    public float answerConfirmation = 3.0f;   // in seconds
    public Slider progressSlider;

    public void Awake()
    {
        wasTouched = false;
        holdTouchClock = new Clock(0);
    }

    public void Update()
    {
        if(wasTouched)
        {

            holdTouchClock.IncreaseTime(Time.deltaTime);
            progressSlider.value = Math.Min(holdTouchClock.Time / answerConfirmation, 1);

            if(holdTouchClock.Time >= answerConfirmation)
            {
                resetSlider();
                holdTouchClock.Reset();
                gameObject.GetComponent<AnswerButton>().HandleClick();
            }
            else
            {
                // TO DO-> CONSIDERAR POSIÇÂO DO TOUCH COM RELAÇÂO Ao tamanho do butão.
                // BUG: Aperte o botão e algum lugar da tela ao mesmo tempo. Solte apenas o botao
                if (Input.touchCount <= 0)
                {
                    resetSlider();
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

    private void resetSlider()
    {
        wasTouched = false;
        progressSlider.value = 0;
        progressSlider.gameObject.SetActive(false);
    }
}
