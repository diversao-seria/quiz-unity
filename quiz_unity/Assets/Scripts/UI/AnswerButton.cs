using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class AnswerButton : MonoBehaviour
{
	public Text answerText;
	public AnswerButton answerButton;

	private GameController gameController;
	private Alternative alternative;
	private Color color;
	private int alternativeNumber;

	void Start()
	{
		gameController = FindObjectOfType<GameController>();
		color = GetComponent<Image>().color;
	}

	public void SetUp(Alternative alternative, int alternativeNumber)
	{
		this.alternative = alternative;
		answerText.text = alternative.Content;
		this.alternativeNumber = alternativeNumber;
	}

	public void HandleClick()
	{
		gameController.AnswerButtonClicked(alternative.IsCorrect, alternativeNumber, GetComponent<EventManager>());
	}
}
