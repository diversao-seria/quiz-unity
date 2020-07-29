using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;

public class DataController : MonoBehaviour 
{
	private RoundData[] allRoundData;
	private PlayerProgress playerProgress;
	private string gameDataFileName = "data.json";

	private Question[] questionsFromQuiz;

	// Só para testes.
	public string path;
	public string filenameJSON = "ExampleJSON.json";


	void Start ()  
	{
		retrieveQuiz();

		DontDestroyOnLoad (gameObject);
		LoadGameData();
		LoadPlayerProgress();
		
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

	// Não veio como o projeto:

	private void retrieveQuiz()
	{
		List<Question> questionsList;
		questionsList = getContentFromFile();
		questionsFromQuiz = questionsList.ToArray();
	}

	private List<Question> getContentFromFile()
	{
		string json = readFromJson();
		Debug.Log(json);
		return JsonConvert.DeserializeObject<List<Question>>(json);
	}

	private string readFromJson()
	{
		return System.IO.File.ReadAllText(path + filenameJSON);
	}

	public Question[] GetQuestions()
	{
		return questionsFromQuiz;
	}
	// OBS: Requisição de internet deve ficar em um objeto (NetHandler) persistente aparte
}