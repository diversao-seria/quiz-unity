using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
	public RoundData CurrentRoundData { get; set; }
	public NetController netController;
	public InterruptSubController interruptSubController;

	public string QuizCode { get; set; }

	private PlayerProgress playerProgress;
	private string gameDataFileName = "data.json";
	public string SessionKeyFilename { get { return "sessionKey.txt"; } set { } }

	private Quiz currentQuiz;
	private string filenameJSON;

	private FileStream activeStream = null;

	private string currentQuizPlayerDataPath = null;

	private Button exitButton;

	public StreamWriter activeQuizDataWriter = null;

	private string _dataSessionKey;

	public string DataSessionKey
	{
		get
		{
			return _dataSessionKey;
		}
	}


	void Start()
	{
		DontDestroyOnLoad(gameObject);
		_dataSessionKey = generateSessionKey();
		interruptSubController = GetComponent<InterruptSubController>();
		LoadGameData();
		LoadPlayerProgress();
		CreateInterruptionFolder();

		SceneManager.LoadScene("MenuScreen");

	}

	private void CreateInterruptionFolder()
	{
		string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.InterruptFolderPath;

		Debug.Log("MyPath:" + path);

		try
		{
			if (!Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}
		}
		catch (IOException e)
		{
			Debug.Log(e + ". Caminho é arquivo ou armazenamento cheio.");
		}

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

		try
		{
			if (!Directory.Exists(playerDataFolder))
			{
				System.IO.Directory.CreateDirectory(Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerDataFolder);
			}
		}
		catch (IOException e)
		{
			Debug.Log(e + ". Caminho é arquivou ou armazenamento cheio.");
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

			CreateDirectoryFromPath(path, text);
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

	public string generateSessionKey()
	{
		return Guid.NewGuid().ToString();
	}

	public void OnApplicationQuit()
	{
		Debug.Log("Jogador saiu no meio jogo!");
	}

	public int TimeDifferenceInSeconds(TimeStamp oldTimeStamp, TimeStamp newTimeStamp)
	{
		int oldTimeInSeconds = oldTimeStamp.Hours * 60 * 60 + oldTimeStamp.Minutes * 60 + oldTimeStamp.Seconds;
		int newTimeInSeconds = newTimeStamp.Hours * 60 * 60 + newTimeStamp.Minutes * 60 + newTimeStamp.Seconds;

		return newTimeInSeconds - oldTimeInSeconds;
	}
	void Update()
	{
		CheckEscape();
	}

	void CheckEscape()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			exitButton = GameObject.Find("ExitIcon").GetComponent<Button>();
			exitButton.onClick.Invoke();
		}
	}
}

