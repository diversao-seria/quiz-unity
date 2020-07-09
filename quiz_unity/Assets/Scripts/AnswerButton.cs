using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnswerButton : MonoBehaviour 
{
	public Text answerText;

	private GameController gameController;
	private AnswerData answerData;

	void Start()
	{
		gameController = FindObjectOfType<GameController>();
	}

	public void SetUp(AnswerData data)
	{
		answerData = data;
		answerText.text = answerData.answerText;
	}

	public void HandleClick()
	{
		gameController.AnswerButtonClicked(answerData.isCorrect);
	}
}
