using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEngine.Networking;

public class DataController : MonoBehaviour
{
	private RoundData[] allRoundData;
	private PlayerProgress playerProgress;
	private string gameDataFileName = "data.json";
	private Quiz currentQuiz;
	private string filenameJSON;

	public NetController netController;

	

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		LoadGameData();
		LoadPlayerProgress();
		SceneManager.LoadScene("MenuScreen");
		
	}

	public RoundData GetCurrentRoundData()
	{
		return allRoundData[0];
	}

	public void SubmitNewScore(int newScore)
	{
		if (newScore > playerProgress.highestScore)
		{
			playerProgress.highestScore = newScore;
			SavePlayerProgress();
		}
	}

	public int GetHighestPlayerScore()
	{
		return playerProgress.highestScore;
	}

	private void LoadPlayerProgress()
	{
		playerProgress = new PlayerProgress();

		if (PlayerPrefs.HasKey("highestScore"))
		{
			playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
		}
	}

	private void SavePlayerProgress()
	{
		PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
	}

	private void LoadGameData()
	{
		//string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
		string filePath = Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + gameDataFileName;


		Debug.Log("path: " + filePath);

		if(File.Exists(filePath))
		{
			string dataAsJson = File.ReadAllText(filePath);
			GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

			allRoundData = loadedData.allRoundData;
		}
		else
		{
			Debug.LogError("Cannot load game data!");
		}
	}

	// Didn't come with the example.

	// Populate all relevant classes with the JSON provided.
	public void PreLoadQuiz(string quizCode)
    {
		filenameJSON = quizCode + ".json";

		QuestionData questionData = new QuestionData();

		questionData = getContentFromFile(quizCode);
		Debug.Log("QuestionTime: " + questionData.QuestionTime);
		currentQuiz = new Quiz(questionData, quizCode);
	}

	private QuestionData getContentFromFile(string quizCode)
	{
		string jsonData = readFromJson(quizCode);
		return JsonConvert.DeserializeObject<QuestionData>(jsonData);
	}

	private string readFromJson(string quizCode)
	{
		return System.IO.File.ReadAllText(Path.Combine(
			Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.QuizFolderRelativePath, quizCode + ".json"
			));
	}

	// Access Handlers

	public Quiz RetrieveQuiz()
	{
		return currentQuiz;
	}

	public void TrackQuestionsAnswers(int n)
    {
		playerProgress.questionAnswers = new QuestionAnswer(n);
    }

	public QuestionAnswer GetQuestionAnswers()
    {
		return playerProgress.GetQuestionAnswers();
    }
}