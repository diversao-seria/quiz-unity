using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class AnswerButton : MonoBehaviour
{
	public Text answerText;
	public Alternative alternative;
	public Slider progressSlider;

	private GameController gameController;
	private EventManager eventManager;
	private Color color;
	private int alternativeNumber;

	void Start()
	{
		gameController = FindObjectOfType<GameController>();
		eventManager = gameController.GetComponent<EventManager>();

		color = GetComponent<Image>().color;
	}

	public void HandlePressDown()
    {
		eventManager.buttonTouched(this);
    }

	public void SetUp(Alternative alternative, int alternativeNumber)
	{
		this.alternative = alternative;
		answerText.text = alternative.Content;
		this.alternativeNumber = alternativeNumber;
	}

	// AnswerClick
	public void HandleClick()
	{
		gameController.AnswerButtonClicked(alternative.IsCorrect, alternativeNumber);
	}
}
