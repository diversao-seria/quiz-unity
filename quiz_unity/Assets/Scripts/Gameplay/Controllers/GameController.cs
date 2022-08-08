using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using System.IO;
using System;
using UnityEngine.Networking;
using System.Text;

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
	public AudioClip[] audioClips;
	public Image blockerImage;
	public GameObject exitPopUp;

	public GameObject spinner;
	public GameObject fadeMask;
	public Text textProgressObject;

	private DataController dataController;
	private RoundData currentRoundData;
	private JsonController jsonController;
	private PowerUpController powerUpController;
	private AudioSource audioSource;
	private EventManager eventManager;
	private InterruptSubController interruptSubController;

	private LoadingScreenGame loadingScreenGame;

	private List<Question> questionPool;  // Question are going here.

	private bool isRoundActive = false;
	private bool isSendingDataFinished = false;
	public bool isQuestionAnswered = false;

	private List<string> sequencia_atuacao = new List<string>();
	private int playerScore;
	private int questionIndex;
	private List<GameObject> answerButtonGameObjects = new List<GameObject>();
	private int streak = 0;
	private int numberOfCorrectAnswers = 0;
	private string gameSessionKey;

	private QuestionClock questionClock;
	private QuizClock quizClock;
	private Clock freezeClock;
	private string serverURL = "http://ds-quiz.herokuapp.com/matches";

	//public RuntimeAnimatorController certo,errado;
	//private Animator animator;

	DateTime beforeInterruptionTimeStamp;
	DateTime backToForeground;

	// Variaveis para o popup de erro ao enviar o formulário
	public GameObject errorPopup;
	public GameObject certezaPopup;
	public bool clicou = false;
	public bool reenviar = false;
	public bool enviou = false;

	enum Clip : int
    {
		correct,
		wrong,
		// power ups
    }

	void Start()
	{
		jsonController = new JsonController();
		jsonController.SetNewQuizResultData();

		dataController = FindObjectOfType<DataController>();        // Store a reference to the DataController so we can request the data we need for this round

		audioSource = gameObject.GetComponent<AudioSource>();
		eventManager = GetComponent<EventManager>();
		interruptSubController = GetComponent<InterruptSubController>();


		currentRoundData = dataController.CurrentRoundData;                      // Ask the DataController for the data for the current round. At the moment, we only have one round - but we could extend this
		questionPool = dataController.RetrieveQuiz().Questions;      // Take a copy of the questions so we could shuffle the pool or drop questions from it without affecting the original RoundData object
		dataController.TrackQuestionsAnswers(questionPool.Count);
		
		jsonController.SetTotalNumberOfQuestions(questionPool.Count);
		jsonController.SetQuizID(dataController.RetrieveQuiz().Id);

		
		jsonController.SetPlayerID(1); // Annonymous

		powerUpController = this.gameObject.GetComponent<PowerUpController>();
		powerUpController.SetJsonControllerReference(jsonController);

		// questionClock = new QuestionClock(dataController.GetComponent<DataController>().RetrieveQuiz().GetQuestionData().QuestionTime);
		eventManager.QuestionClock = new QuestionClock(GameMechanicsConstant.TimeToAnswerQuestion);
		questionClock = eventManager.QuestionClock;

		quizClock = new QuizClock(0);

		UpdateTimeRemainingDisplay(questionClock);
		playerScore = 0;
		questionIndex = 0;
		// rightAnswers = 0;

		questionNumberTextController.GetComponent<QuestionNumberController>().SetMaxQuestions(questionPool.Count);

		// TO DO: Colocar na Cena de carregar o quiz (deve vir antes)
		Randomizer.RandomizeAlternatives(questionPool);

		ShowQuestion();
		ShowQuestionNumber();

		jsonController.RegisterStartTime(); // records the current system time and date

		string quizPlayerDataPath = Application.persistentDataPath +
									  Path.AltDirectorySeparatorChar +
									  DataManagementConstant.PlayerDataPath +
									  dataController.QuizCode + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerQuizDataFile;

		dataController.SetCurrentSessionPath(quizPlayerDataPath);
		dataController.UpdateJSONPlayerData(jsonController.SerializeAnswerData());
		dataController.CloseJSONFile();

		isRoundActive = true;
	}

	void Update()
	{
		if (isRoundActive && !isQuestionAnswered)
		{
			quizClock.IncreaseTime(Time.deltaTime);

			if (!powerUpController.timeFreeze)
				questionClock.DecreaseTime(Time.deltaTime);
			else
            {
				powerUpController.FreezeClock.IncreaseTime(Time.deltaTime);
				if(powerUpController.FreezeClock.Time >= 7)
                {
					powerUpController.WaterPowerExpired();
                }
            }

			UpdateTimeRemainingDisplay(questionClock);

			// Time is up!
			if (questionClock.Time <= 0.0f)                                                        
			{
				isQuestionAnswered = true;
				eventManager.LockAnswer();

				if(eventManager.GetAnswerButton() && eventManager.TouchLastStatus())
                {
					if (powerUpController.leafImmunity)
                    {
						 eventManager.resetTouchClock();
						 eventManager.resetSlider();
                    }
					eventManager.GetAnswerButton().HandleClick(questionClock);
				}
				else
                {
					if(powerUpController.leafImmunity)
                    {
							// questionClock.NewCountdown(dataController.GetComponent<DataController>().RetrieveQuiz().GetQuestionData().QuestionTime);
						StartCoroutine(powerUpController.PowerUpFolhaAnim());
						questionClock.NewCountdown(GameMechanicsConstant.TimeToAnswerQuestion);
						eventManager.LetAnswerQuestion();
						eventManager.ResetLastAnswerButton();
						powerUpController.LeafPowerExpired();
						isQuestionAnswered = false;
					}
					else
                    {
						AnswerButtonClicked(false, -1);
					}
				}
			}
		}
	}

	void ShowQuestion()
	{
		RemoveAnswerButtons();

		Question question = questionPool[questionIndex];                                     // Get the QuestionData for the current question

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

		eventManager.SetAlternativesReference(answerButtonGameObjects);
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

	public void AnswerButtonClicked(bool isCorrect, int alternativeNumber)
	{
		// Block all UI interactions.
		blockerImage.transform.gameObject.SetActive(true);

		// Close any popups
		if (exitPopUp.activeInHierarchy) exitPopUp.GetComponent<PopupHandler>().FinishExitBehaviour("exit");

		// To be shown to player later 
		dataController.GetQuestionAnswers().RegisterPlayerAnswer(
				eventManager,
				isCorrect,
				questionIndex,
				alternativeNumber,
				questionClock.Time
			);

		// For data collection
		jsonController.AddNewAnsweredQuestion(
				dataController.RetrieveQuiz().Questions[questionIndex].ID,
				alternativeNumber,
				isCorrect,
				(int)(GameMechanicsConstant.TimeToAnswerQuestion - questionClock.Time)
			);


		dataController.UpdateJSONPlayerData(jsonController.SerializeAnswerData());
		dataController.CloseJSONFile();


		// Set Up for Next question;
		jsonController.UpdateTotalTime((int)(GameMechanicsConstant.TimeToAnswerQuestion - questionClock.Time));

		eventManager.resetTouchClock();
		eventManager.resetSlider();
		eventManager.ResetLastAnswerButton();

		powerUpController.AnswerCount(isCorrect);
		powerUpController.WaterPowerExpired();
		powerUpController.LeafPowerExpired();
		powerUpController.AirPowerExpired();

		if (isCorrect)
		{
			jsonController.UpdateScore(playerScore);
			jsonController.UpdateNumberOfCorrectQuestions();
			playerScore += currentRoundData.CurrentPoints;                    // If the AnswerButton that was clicked was the correct answer, add points
			scoreDisplay.text = playerScore.ToString();
			streak++;
			// Add correct feedback audio
			audioSource.PlayOneShot(audioClips[(int)Clip.correct]);
		}
		else
		{
			// Add wrong feedback audio
			audioSource.PlayOneShot(audioClips[(int)Clip.wrong]);
		}

		/*
		if (streak > jsonController.streak)
		{
			jsonController.streak = streak;
		}
		*/


		// sequencia_atuacao.Add(string.Join("", parts));     // This puts every part of the separated source string together, creating a new string which will be saved in the .json file.

		// TO DO: Save Partial Answers around here. How?
		// Finished Answering. Register and Reset.



		StartCoroutine(VisualFeedback(isCorrect));
	}

	private void UpdateTimeRemainingDisplay(QuestionClock clock)
	{
		timeRemainingDisplay.text = Mathf.Round(clock.Time).ToString();
	}

	public void EndRound()
	{
		jsonController.FlagSessionInterrupt(true);
		isRoundActive = false;

		dataController.SubmitNewScore(playerScore);
		highScoreDisplay.text = dataController.GetHighestPlayerScore().ToString();

		questionDisplay.SetActive(false);
		roundEndDisplay.SetActive(true);

		jsonController.UpdateScore(playerScore);




		// darken screen
		// SEnd data
		// success or failure

		PlayerPrefs.SetString("dataControllerQuizCode", dataController.QuizCode);
		StartCoroutine(SendForm(dataController.QuizCode));


		// TO DO: success or failure
		Debug.Log("isRoundactive: " + isRoundActive + " e isQuestionAnswered: " + isQuestionAnswered);
		// isROundActivemustBefalse Here
		Debug.Log("Total time: " + quizClock.HHmmss());
	}



	public void ReturnToMenu()
	{
		SceneManager.LoadScene("MenuScreen");
	}

	IEnumerator SendForm(string quizCode)
	{

		Debug.Log("Antes FOrm");

		enviou = false;
		//int count = 0;
		while(!enviou /*|| count < 10*/){
			//count++;

			using (UnityWebRequest www = UnityWebRequest.Post(serverURL, "POST"))
			{

				// List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
				// formData.Add(new MultipartFormDataSection("ID=" + exampleID.ToString() + "&" + "TemponoQuiz=" + exampleTime + "&" + "RespostasCorretas=" + exampleCorrect.ToString()));
				// formData.Add(new MultipartFormFileSection("my file data", pathToMatchData));

				string pathToQuizResult = Path.Combine(Application.persistentDataPath + Path.AltDirectorySeparatorChar +
					DataManagementConstant.PlayerDataPath + quizCode + Path.AltDirectorySeparatorChar + "QuizAnswerData" + ".json");

				string jsonData = null;

				loadingScreenGame = new LoadingScreenGame(spinner, fadeMask, textProgressObject);

				try
				{
					jsonData = System.IO.File.ReadAllText(pathToQuizResult);
				}
				catch (IOException e)
				{
					Debug.Log(e);
				}

				www.timeout = 10;

				loadingScreenGame.EnableLoadingGUI();

				// Form Data
				byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
				www.SetRequestHeader("Content-Type", "application/json");
				www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
				www.SendWebRequest();

				while (!www.isDone)
				{
					if(www.result == UnityWebRequest.Result.ConnectionError){
						Debug.Log("Connection Error");
						break;
					}
					loadingScreenGame.UpdateDownloadProgress(www.downloadProgress * 100 + "%");
					yield return null;
				}
				
				Debug.Log("Status Code: " + www.responseCode);
				loadingScreenGame.DisableLoadingGUI();

				if (www.result != UnityWebRequest.Result.Success)
				{
					Debug.Log("Entrou no if");
					
					errorPopup.SetActive(true);

					while(!clicou){
						yield return null;
					}
					clicou = false;
					
					if(reenviar){
						Debug.Log("Tentando Enviar Novamente");
					}
				}
				else
				{
					loadingScreenGame.UpdateDownloadProgress("100%");
					Debug.Log("Form upload complete!");
					enviou = true;
				}

				// www.downloadHandler.Dispose();
				www.disposeCertificateHandlerOnDispose = true;
				www.disposeDownloadHandlerOnDispose = true;
				www.disposeUploadHandlerOnDispose = true;
				// www.uploadHandler.Dispose();
				www.Dispose();
			}

		}
		Debug.Log("Depois Form");
		SceneManager.LoadScene("QuizResult");
	}

	IEnumerator VisualFeedback(bool isCorrect)
	{
		if (isCorrect)
		{
			feedbackImage.GetComponent<Image>().sprite = correctAnswerIcon;
			//animator.runtimeAnimatorController = certo;
			// rightAnswers++;
		}
		else
		{
			feedbackImage.GetComponent<Image>().sprite = wrongAnswerIcon;
			//animator.runtimeAnimatorController = errado;
			streak = 0;
		}

		feedbackImage.gameObject.SetActive(true);
		yield return new WaitForSeconds(3);
		feedbackImage.gameObject.SetActive(false);

		if (questionPool.Count > questionIndex + 1)                                         // If there are more questions, show the next question
		{
			// Load Next Question to be answered.
			questionIndex++;
			questionNumberTextController.GetComponent<QuestionNumberController>().NextQuestion();
			ShowQuestion();
			ShowQuestionNumber();
			isQuestionAnswered = false;
			eventManager.LetAnswerQuestion();

			// New Countdown
			questionClock.NewCountdown(GameMechanicsConstant.TimeToAnswerQuestion);

			// FOR FUTURE FEATURE: questionClock.NewCountdown(dataController.GetComponent<DataController>().RetrieveQuiz().GetQuestionData().QuestionTime);

			// Allow all UI interactions.
			blockerImage.transform.gameObject.SetActive(false);
		}
		else                                                                                // If there are no more questions, the round ends
		{
			EndRound();
		}
	}

	public void OnApplicationPause(bool ScreenBackgroundStatus)
	{
		// Only relevant when a Quiz is being played.
		if (!FindObjectOfType<GameController>()) return;

		// Exiting to background
		if (ScreenBackgroundStatus)
		{
			// Get CUrrent System Time
			// beforeInterruptionTimeStamp = new TimeStamp(System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);
			interruptSubController.BeforeInterruptionTimeStamp = DateTime.Now;
			Debug.Log("BI" + interruptSubController.BeforeInterruptionTimeStamp.ToString());

			// Paths for folder and file;
			string interruptDataFolder = Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.InterruptFolderPath;
			string sessionKeyFilePath = interruptDataFolder + Path.AltDirectorySeparatorChar + dataController.QuizCode + ".txt";

			if (!File.Exists(sessionKeyFilePath))
			{
				// 1st insertion.
				dataController.WriteOnPath(sessionKeyFilePath, interruptSubController.BeforeInterruptionTimeStamp.ToString() + " " + gameSessionKey);
			}

			interruptSubController.RegisterInterrupt(GameMechanicsConstant.InterruptTypes.BackgroundToForegroud, true);
		}
		else
		{
			// Game is back from backgroud. Obs: This block of code is never executed if user kills the game in background.

			interruptSubController.BackToForegroundTimeStamp = DateTime.Now;
			Debug.Log("BtF" + interruptSubController.BackToForegroundTimeStamp.ToString());

			TimeSpan stampDifference = interruptSubController.BackToForegroundTimeStamp - interruptSubController.BeforeInterruptionTimeStamp;

			Debug.Log("DIFF " + stampDifference.TotalSeconds.ToString()); ;
			Debug.Log("DIFF (STAMP) " + stampDifference.ToString()); ;

			if (stampDifference.Hours > 0 || stampDifference.Minutes > 0 || stampDifference.TotalSeconds > GameMechanicsConstant.TimeToAnswerQuestion - 1)
			{
				// Time's over;
				questionClock.Reset();
			}
			else
			{
				// Decrease the time while in background.
				questionClock.DecreaseTime(System.Math.Max(1.0f, (float)stampDifference.TotalSeconds));
			}

			// Register Interruption Type
			interruptSubController.RegisterInterrupt(GameMechanicsConstant.InterruptTypes.BackgroundToForegroud, false);
		}
	}

	// Função que controla os popups relacionados com erro ao enviar o formulário ao final do Quiz.
	public void RetryFormSubmission(int sel)
	{	
		switch (sel)
		{
			case 0:
				// Enviar Novamente? Sim.
				Debug.Log("Enviar Novamente? Sim.");
				//SendForm(PlayerPrefs.GetString("dataControllerQuizCode"));
				clicou = true;
				reenviar = true;
				errorPopup.SetActive(false);
				break;
			case 1:
				// Enviar Novamente? Não.
				Debug.Log("Enviar Novamente? Não.");
				certezaPopup.SetActive(true);
				errorPopup.SetActive(false);
				break;
			case 2:
				// As informações podem ser perdidas. Deseja Continuar? Sim.
				Debug.Log("As informações podem ser perdidas. Deseja Continuar? Sim.");
				clicou = true;
				enviou = true;
				certezaPopup.SetActive(false);
				break;
			case 3:
				// As informações podem ser perdidas. Deseja Continuar? Não.
				Debug.Log("As informações podem ser perdidas. Deseja Continuar? Não.");
				certezaPopup.SetActive(false);
				errorPopup.SetActive(true);
				break;
		}
	}

	// "4" FLAGS
	// 1- IN GAME - BTackTOBeggining |
	// 2 - SUSPENDED ACTIVITY        |  
	// 3 - CLOSED THE GAME           | QUIZ CODE


	// 1-> INTERRUPT IN GAME (DONE)

	// 2-> BACKGROUND BUT DOESN'T CLOSE 
	// true, keySaved | false,samekey, interruption
	// Pegar o tempo e calcular -> descontar.
	// 3-> BACKGROUNBD + CLOSE
	// true, keysaved
	// 1. Create SessionKeyFile if it doesn exist. (FOR POSSIBLE INTERRUPTS)
	// QUIZ CODE | SessionKey
	// 2. For data.
	// QUIZ CODE | QuestionNumber -> How Many Interruptions TIL finish | TYPE (NO-INTERRUPT, INSIDE, EXTERNAL-NF, EXTERNAL)| 



}
