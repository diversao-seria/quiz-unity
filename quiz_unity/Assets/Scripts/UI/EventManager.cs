using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    private PressClock holdTouchClock;

    // Boolean that represents if an answerbutton was touched.
    private bool wasTouched;

    // Boolean that represents if it's possible to select an answerButton
    private bool idleState;

    private List<GameObject> answerButtonList;

    public float answerConfirmationTime = GameMechanicsConstant.AnswerConfirmationTimeinSeconds;   

    private AnswerButton selectedAnswerButton;
    private Slider progressSlider;

    private AudioSource audioSource;
    public AudioClip touchingClip;

    public QuestionClock QuestionClock { get; set; }

    public void Awake()
    {
        wasTouched = false;
        LetAnswerQuestion();
        holdTouchClock = new PressClock(0);

        audioSource = GetComponent<AudioSource>();

        SliderFunctionValue.SetCalculationMode(GameMechanicsConstant.VisualSliderChange.Linear); // linear or polynomial or exponential
    }

    public void Update()
    {
        if(wasTouched && !idleState)
        {
            Touch touch = Input.GetTouch(0);

            // Update of the clock touching.
            holdTouchClock.IncreaseTime(Time.deltaTime);

            if(!audioSource.isPlaying) audioSource.PlayOneShot(touchingClip);

            // Update selection change (Orange bar - visual only)
            progressSlider.value = Math.Min((float) SliderFunctionValue.ProgressCalculation(holdTouchClock.Time), 1.0f);

            Debug.Log("BEFORE F(X) -> " + progressSlider.value);
            if (holdTouchClock.Time >= answerConfirmationTime)
            {
                Debug.Log("Holdtouch Time -> " + holdTouchClock.Time);
                Debug.Log("AFTER F(X) -> " + progressSlider.value);
                resetSlider();
                resetTouchClock();
                selectedAnswerButton.GetComponent<AnswerButton>().HandleClick(QuestionClock);
            }
            else
            {
                if (Input.touchCount <= 0 || touch.phase == TouchPhase.Ended)
                {
                    LetAnswerQuestion();
                    resetSlider();
                    resetTouchClock();
                }
            }
        }
    }

    // SliderCalculation from Interface

    public float ProgressCalculation(float totalTimeElapsed)
    {
        // (8/21) * (X^3 - X^2 + X)

        return (
            ((totalTimeElapsed) * (totalTimeElapsed) * (totalTimeElapsed)) -
            ((totalTimeElapsed) * (totalTimeElapsed)) +
             (totalTimeElapsed)
            ) * (8.0f / 21.0f);
            
    }
    public float UpdateSliderValue(float totalTimeElapsed)
    {
        return ProgressCalculation(totalTimeElapsed);
    }



    // Called from AnswerButton object as pointdown event.
    public void buttonTouched(AnswerButton answerButton)
    {

        if(AllowedToAnswer())
        {
            selectedAnswerButton = answerButton;
            progressSlider = (Slider)answerButton.transform.Find("Slider").gameObject.GetComponent<Slider>();
            wasTouched = true;
            progressSlider.gameObject.SetActive(true);
            LockAnswer();
        }
    }

    public void resetSlider()
    {
        audioSource.Stop();
        wasTouched = false;
        if(progressSlider)
        {
            progressSlider.value = 0;
            progressSlider.gameObject.SetActive(false);
        }
    }

    // Flag to allow the user to answer question
    public void LetAnswerQuestion()
    {
        idleState = true;
    }

    // Flag for locking user's answer
    public void LockAnswer()
    {
        idleState = false;
    }

    // Acces to touch status
    public bool TouchLastStatus()
    {
        return wasTouched;
    }

    // Access to the status of the possibility of answer
    public bool AllowedToAnswer()
    {
        return idleState;
    }

    public void SetAlternativesReference(List <GameObject> answerButtonList)
    {
        this.answerButtonList = answerButtonList;
    }

    public void resetTouchClock()
    {
        holdTouchClock.Reset();
    }

    public AnswerButton GetAnswerButton()
    {
        return selectedAnswerButton;
    }

    public void ResetLastAnswerButton()
    {
        selectedAnswerButton = null;
    }
}
