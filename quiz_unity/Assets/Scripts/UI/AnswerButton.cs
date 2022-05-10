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
	private PowerUpController powerUpController;
	private Color color;
	private int alternativeNumber;

	void Start()
	{
		gameController = FindObjectOfType<GameController>();
		eventManager = gameController.GetComponent<EventManager>();
		powerUpController = gameController.GetComponent<PowerUpController>();

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
	public void HandleClick(QuestionClock questionClock)
	{

		if (!eventManager.GetAnswerButton().alternative.IsCorrect && powerUpController.leafImmunity)
		{
			// questionClock.NewCountdown(dataController.GetComponent<DataController>().RetrieveQuiz().GetQuestionData().QuestionTime);
			StartCoroutine(powerUpController.PowerUpFolhaAnim());
			questionClock.NewCountdown(GameMechanicsConstant.TimeToAnswerQuestionAfterLeafPowerUp);
			eventManager.LetAnswerQuestion();
			eventManager.ResetLastAnswerButton();
			powerUpController.LeafPowerExpired();
			gameController.isQuestionAnswered = false;
		}
		else
        {
			gameController.isQuestionAnswered = true;
			gameController.AnswerButtonClicked(alternative.IsCorrect, alternativeNumber);
		}
	}
}
