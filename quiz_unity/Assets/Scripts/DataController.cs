using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class DataController : MonoBehaviour 
{
	private RoundData[] allRoundData;
	private PlayerProgress playerProgress;
	private string gameDataFileName = "data.json";
	
	void Start ()  
	{
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
}