﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEngine.Networking;
using System.Text;

public class DataController : MonoBehaviour
{
	public RoundData CurrentRoundData { get; set; }
	public NetController netController;

	public string QuizCode { get; set; }

	private PlayerProgress playerProgress;
	private string gameDataFileName = "data.json";
	private Quiz currentQuiz;
	private string filenameJSON;

	private FileStream activeStream = null;

	private string currentQuizPlayerDataPath = null;


	public StreamWriter activeQuizDataWriter = null;

	

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		LoadGameData();
		LoadPlayerProgress();
		SceneManager.LoadScene("MenuScreen");
		
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
		string filePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerDataPath + gameDataFileName;
		string playerDataFolder = Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerDataPath;

		if (!Directory.Exists(playerDataFolder))
        {
			System.IO.Directory.CreateDirectory(Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerDataFolder);
		}

		Debug.Log("path: " + filePath);
	}

	// Didn't come with the example.

	public void CreateDirectoryFromPath(string path, string text)
    {
		// Get the current path for application
		string currentPath = Application.persistentDataPath;

		// Break relative path to separate the necessary folders for creation.
		string[] folderList = DataManagementConstant.QuizFolderRelativePath.Split(Path.AltDirectorySeparatorChar);

		try
		{
			// For each folder candidate, add in the current path and create folder. Stops when empty string (after the last separator)
			foreach (string folderCandidate in folderList)
			{
				if (string.Equals(folderCandidate, "")) break;
				currentPath = currentPath + Path.AltDirectorySeparatorChar + folderCandidate;
				System.IO.Directory.CreateDirectory(currentPath);
			}

			// With the path sorted out, create the file and write it.
			FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
			StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
			streamWriter.Write(text);
			streamWriter.Close();
		}
		catch (IOException ioE)
		{
			// Exception when problems related to writting occurs (like disk is full)
			Debug.Log(ioE.GetType().Name + "Erro ao escrever pastas/arquivos. Tentar de novo ou armazenamento está cheio.");
			// TO DO:  Feedback Visual.
		}
	}

	// For web related files.
	public void WriteOnPath(string path, string text)
	{

		FileStream fileStream;

		try
		{
			using (fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
				streamWriter.Write(text);
				streamWriter.Close();
				Debug.Log("Arquivo armazenado com sucesso");
			}

		}
		catch (DirectoryNotFoundException dirE)
		{
			// This exception is thrown if there is no quiz folder.
			Debug.Log(dirE.GetType().Name + ".\n Caminho não existente. Criando...");

			CreateDirectoryFromPath(path,text);
		}
		catch (IOException ioE)
		{
			// Exception when problems related to writting occurs (like disk is full)
			Debug.Log(ioE.GetType().Name + "Erro ao escrever pastas/arquivos. Tentar de novo ou armazenamento está cheio.");
			// TO DO:  Feedback Visual.
		}
	}



	// Singleton
	public void SetJSONWriter(string path, string text)
    {

        try 
		{
			if (activeStream == null)
			{
				activeStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
			}
			if (activeQuizDataWriter == null)
			{
				activeQuizDataWriter = new StreamWriter(activeStream, Encoding.UTF8); ;
			}
		// TO DO:  Feedback Visual.
		}
		catch (DirectoryNotFoundException dirE)
		{
			// This exception is thrown if there is no quiz folder.
			Debug.Log(dirE.GetType().Name + ".\n Caminho não existente. Criando...");
			CreateDirectoryFromPath(path, text);
		}
		catch (IOException ioE)
		{
			// Exception when problems related to writting occurs (like disk is full)
			Debug.Log(ioE.GetType().Name + "Erro ao escrever pastas/arquivos. Tentar de novo ou armazenamento está cheio.");
			// TO DO:  Feedback Visual.
		}
	}

	public void SetCurrentSessionPath(string currentQuizPlayerDataPath)
    {
		this.currentQuizPlayerDataPath = currentQuizPlayerDataPath;
    }

	public void UpdateJSONPlayerData(string text)
    {
		try
		{
				activeStream = new FileStream(currentQuizPlayerDataPath, FileMode.Create, FileAccess.Write, FileShare.None);
				activeQuizDataWriter = new StreamWriter(activeStream, Encoding.UTF8); 
			
			// TO DO:  Feedback Visual.
		}
		catch (DirectoryNotFoundException dirE)
		{
			// This exception is thrown if there is no quiz folder.
			Debug.Log(dirE.GetType().Name + ".\n Caminho não existente. Criando...");
			CreateDirectoryFromPath(currentQuizPlayerDataPath, "");
		}
		catch (IOException ioE)
		{
			// Exception when problems related to writting occurs (like disk is full)
			Debug.Log(ioE.GetType().Name + "Erro ao escrever pastas/arquivos. Tentar de novo ou armazenamento está cheio.");
			// TO DO:  Feedback Visual.
		}

		activeQuizDataWriter.Write(text);
	}

	// Singleton
	public FileStream GetStream(string path)
    {
		if(activeStream == null)
        {
			return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
		}
		return activeStream;
    }

	public void CloseJSONFile()
    {
		activeQuizDataWriter.Close();
		activeStream.Close();
		activeQuizDataWriter = null;
		activeStream = null;
	}

	// Populate all relevant classes with the JSON provided.
	public void PreLoadQuiz(string quizCode)
    {
		QuizCode = quizCode;
		filenameJSON = quizCode + ".json";

		currentQuiz = getContentFromFile(QuizCode);
		Debug.Log("QUIZ_ID: " + currentQuiz.Id);
		// Debug.Log("QuestionTime: " + questionData.QuestionTime);
	}

	private Quiz getContentFromFile(string quizCode)
	{
		string jsonData = readFromJson(quizCode);
		return JsonConvert.DeserializeObject<Quiz>(jsonData);
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