using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class AnswerButton : MonoBehaviour
{
	public Text answerText;

	private GameController gameController;
	private Alternative alternative;
	private Color color;

	void Start()
	{
		gameController = FindObjectOfType<GameController>();
		color = GetComponent<Image>().color;
	}

	public void SetUp(Alternative alternative)
	{
		this.alternative = alternative;
		answerText.text = alternative.Content;
	}

	public void HandleClick()
	{
		gameController.AnswerButtonClicked(alternative.IsCorrect);
		color = Color.Lerp(color, Color.green, Mathf.PingPong(Time.time, 1));
	}
}
