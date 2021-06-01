using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using System.IO;

public class GameController : MonoBehaviour
{
	public SimpleObjectPool answerButtonObjectPool;
	public Text questionText;
	public Text scoreDisplay;
	public Text timeRemainingDisplay;
	public Text questionNumberTextController;
	public Transform answerButtonParent;
	public GameObject questionDisplay;
	public GameObject roundEndDisplay;
	public Text highScoreDisplay;
	public Image feedbackImage;
	public Sprite correctAnswerIcon;
	public Sprite wrongAnswerIcon;

	private DataController dataController;
	private RoundData currentRoundData;
	private JsonController jsonController;
	private PowerUpController powerUpController;

	private List<Question> questionPool;  // Question are going here.

	private bool isRoundActive = false;
	private bool isQuestionAnswered = false;

	private int rightAnswers;
	private int playerScore;
	private int questionIndex;
	private List<GameObject> answerButtonGameObjects = new List<GameObject>();

	private QuestionClock questionClock;
	private QuizClock quizClock;

	void Start()
	{
		jsonController = FindObjectOfType<JsonController>();

		dataController = FindObjectOfType<DataController>();        // Store a reference to the DataController so we can request the data we need for this round

		// powerUpController = FindObjectOfType<PowerUpController>();

		currentRoundData = dataController.GetCurrentRoundData();                       // Ask the DataController for the data for the current round. At the moment, we only have one round - but we could extend this
		questionPool = dataController.RetrieveQuiz().GetQuestionData().Questions;      // Take a copy of the questions so we could shuffle the pool or drop questions from it without affecting the original RoundData object
		dataController.TrackQuestionsAnswers(questionPool.Count);

		powerUpController = this.gameObject.GetComponent<PowerUpController>();

		// questionClock = new QuestionClock(dataController.GetComponent<DataController>().RetrieveQuiz().GetQuestionData().QuestionTime);
		questionClock = new QuestionClock(30);

		quizClock = new QuizClock(0);

		UpdateTimeRemainingDisplay(questionClock);
		playerScore = 0;
		questionIndex = 0;
		rightAnswers = 0;

		questionNumberTextController.GetComponent<QuestionNumberController>().SetMaxQuestions(questionPool.Count);

		// TO DO: Colocar na Cena de carregar o quiz (deve vir antes)
		Randomizer.RandomizeAlternatives(questionPool);

		ShowQuestion();
		ShowQuestionNumber();

		isRoundActive = true;
	}

	void Update()
	{
		// what is round active?
		if (isRoundActive && !isQuestionAnswered)
		{
			quizClock.IncreaseTime(Time.deltaTime);
			if (!powerUpController.timeFreeze)
				questionClock.DecreaseTime(Time.deltaTime);
			UpdateTimeRemainingDisplay(questionClock);

			if (questionClock.Time <= 0f)                                                        // If timeRemaining is 0 or less, the round ends
			{
				if (questionIndex == questionPool.Count - 1)
				{
					EndRound();
				}
				else
				{
					AnswerButtonClicked(false, -1, null);
				}
			}
		}
	}

	void ShowQuestion()
	{
		RemoveAnswerButtons();

		Question question = questionPool[questionIndex];                                     // Get the QuestionData for the current question																								 // Update questionText with the correct tex
		questionText.text = question.Text;

		for (int i = 0; i < question.Alternatives.Count; i++)                               // For every AnswerData in the current QuestionData...
		{
			GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();         // Spawn an AnswerButton from the object pool
			answerButtonGameObjects.Add(answerButtonGameObject);
			answerButtonGameObject.transform.SetParent(answerButtonParent);
			answerButtonGameObject.transform.localScale = Vector3.one;

			AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();

			answerButton.SetUp(question.Alternatives[i], i);                           // Pass the AnswerData to the AnswerButton so the AnswerButton knows what text to display and whether it is the correct answer
		}
	}

	public void ShowQuestionNumber()
	{
		questionNumberTextController.GetComponent<QuestionNumberController>().FormatedDisplay();
	}

	void RemoveAnswerButtons()
	{
		while (answerButtonGameObjects.Count > 0)                                           // Return all spawned AnswerButtons to the object pool
		{
			answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
			answerButtonGameObjects.RemoveAt(0);
		}
	}

	public void AnswerButtonClicked(bool isCorrect, int alternativeNumber, EventManager eventManager)
	{
		isQuestionAnswered = true;

		dataController.GetQuestionAnswers().RegisterPlayerAnswer(
				eventManager,
				isQuestionAnswered,
				isCorrect,
				questionIndex,
				alternativeNumber,
				questionClock.Time
			);

		powerUpController.AnswerCount(isCorrect);

		if (isCorrect)
		{
			playerScore += currentRoundData.pointsAddedForCorrectAnswer;                    // If the AnswerButton that was clicked was the correct answer, add points
			scoreDisplay.text = playerScore.ToString();
		}


		StartCoroutine(VisualFeedback(isCorrect));
	}

	private void UpdateTimeRemainingDisplay(QuestionClock clock)
	{
		timeRemainingDisplay.text = Mathf.Round(clock.Time).ToString();
	}

	public void EndRound()
	{
		isRoundActive = false;

		dataController.SubmitNewScore(playerScore);
		highScoreDisplay.text = dataController.GetHighestPlayerScore().ToString();

		questionDisplay.SetActive(false);
		roundEndDisplay.SetActive(true);

		// Creating file with quiz results
		if (File.Exists("QuizAnswerData.json"))

		{
			File.Delete("QuizAnswerData.json"); // Making sure there's only one file at one point in time
		}
		jsonController.score = playerScore;
		jsonController.time = quizClock.HHmmss();
		jsonController.rightAnswers = rightAnswers;
		StreamWriter writer = File.CreateText(Application.streamingAssetsPath + "\\QuizAnswerData.json");
		writer.WriteLine(jsonController.SaveToString());
		writer.Close();

		Debug.Log("Total time: " + quizClock.HHmmss());
		SceneManager.LoadScene("QuizResult");
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene("MenuScreen");
	}

	IEnumerator VisualFeedback(bool isCorrect)
	{
		if (isCorrect)
		{
			feedbackImage.GetComponent<Image>().sprite = correctAnswerIcon;
			rightAnswers++;
		}
		else
		{
			feedbackImage.GetComponent<Image>().sprite = wrongAnswerIcon;
		}

		feedbackImage.gameObject.SetActive(true);
		yield return new WaitForSeconds(3);
		feedbackImage.gameObject.SetActive(false);
		isQuestionAnswered = false;

		if (questionPool.Count > questionIndex + 1)                                         // If there are more questions, show the next question
		{
			questionIndex++;
			questionNumberTextController.GetComponent<QuestionNumberController>().NextQuestion();
			// questionClock.NewCountdown(dataController.GetComponent<DataController>().RetrieveQuiz().GetQuestionData().QuestionTime);
			questionClock.NewCountdown(30);
			ShowQuestion();

			ShowQuestionNumber();
		}
		else                                                                                // If there are no more questions, the round ends
		{
			EndRound();
		}

	}
}