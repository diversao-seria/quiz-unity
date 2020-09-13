using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;
using System;

public class DataController : MonoBehaviour 
{
	private RoundData[] allRoundData;
	private PlayerProgress playerProgress;
	private string gameDataFileName = "data.json";
	private Quiz currentQuiz;

	// Só para testes.
	public string path;
	public string filenameJSON = "ExampleJSON.json";

	void Start ()  
	{

		DontDestroyOnLoad (gameObject);

		LoadGameData();
		LoadPlayerProgress();
		
		QuestionData questions = retrieveQuestions();
		currentQuiz = new Quiz(questions, "123456");
		
		
		SceneManager.LoadScene("MenuScreen");
	}
	
	public RoundData GetCurrentRoundData()
	{
		return allRoundData [0];
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
		string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

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

	// Não veio com o projeto:

	public Quiz RetrieveQuiz()
	{
		return currentQuiz;
	}

	private QuestionData IntializationOfAllObjects()
    {
		QuestionData questionData = new QuestionData();
		return questionData;
    }

	private QuestionData retrieveQuestions()
    {
		QuestionData questionsData = IntializationOfAllObjects();
		questionsData = getContentFromFile();
		return questionsData;
	}

	private QuestionData getContentFromFile()
	{
		string json = readFromJson();
		Debug.Log(json);
		return JsonConvert.DeserializeObject<QuestionData>(json);
	}

	private string readFromJson()
	{
		return System.IO.File.ReadAllText(path + filenameJSON);
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