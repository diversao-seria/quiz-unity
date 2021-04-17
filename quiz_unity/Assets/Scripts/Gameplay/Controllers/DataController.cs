﻿using UnityEngine;
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
	private string filenameJSON;
	private char slash;

	public NetController netController;

	void Start()
	{
		setFilenamePathChar();
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
		string filePath = Application.streamingAssetsPath + "/" + gameDataFileName;
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

	// Não veio com o projeto:

	public void PreLoadQuiz(string quizCode)
    {
		filenameJSON = quizCode + ".json";
		QuestionData questions = retrieveQuestions(quizCode);
		currentQuiz = new Quiz(questions, quizCode);
	}

	public Quiz RetrieveQuiz()
	{
		return currentQuiz;
	}

	private QuestionData retrieveQuestions(string quizCode)
	{
		QuestionData questionsData = IntializationOfAllObjects();
		questionsData = getContentFromFile();
		return questionsData;
	}

	private QuestionData IntializationOfAllObjects()
    {
		QuestionData questionData = new QuestionData();
		return questionData;
    }

	private QuestionData getContentFromFile()
	{
		string json = readFromJson();
		return JsonConvert.DeserializeObject<QuestionData>(json);
	}

	private string readFromJson()
	{
		return System.IO.File.ReadAllText(Path.Combine(
			Application.streamingAssetsPath, filenameJSON
			));
	}

	public void TrackQuestionsAnswers(int n)
    {
		playerProgress.questionAnswers = new QuestionAnswer(n);
    }

	public QuestionAnswer GetQuestionAnswers()
    {
		return playerProgress.GetQuestionAnswers();
    }

	private void setFilenamePathChar()
    {
		slash = '/';
		if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
			slash = '\\';
        }
    }
}