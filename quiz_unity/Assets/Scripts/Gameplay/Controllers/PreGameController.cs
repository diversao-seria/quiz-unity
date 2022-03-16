using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;
using System.Text;

public class PreGameController : MonoBehaviour
{
    private DataController dataController;
    private NetController netController;

    public GameObject errorController;
    public Button m_startButton;
    public Text inputText;
    public string errorText = "Erro ao baixar o arquivo!";
    private bool requestStatus = false;

    public Text downloadProgess;
    public GameObject spinner;
    public GameObject fadeMask;
    public GameObject textProgressObject;

    public LoadingScreenGame loadingScreenGame;

    private UnityWebRequest www;

    void Awake()
    {
        m_startButton.onClick.AddListener(CheckForQuiz);
        dataController = FindObjectOfType<DataController>();
        netController = FindObjectOfType<NetController>();

        loadingScreenGame = new LoadingScreenGame(spinner,fadeMask,downloadProgess);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckForQuiz()
    {
        // put quizcode on upper case
        string quizCode = inputText.GetComponent<Text>().text.ToUpper();

        StartCoroutine(GetFile(quizCode));

        // Net Controller Search routine.
        netController.GetComponent<NetController>().RequestQuiz(quizCode);
    }

    private bool isQuizAvailable(string quizCode)
    {
        return File.Exists(Path.Combine(
            Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.QuizFolderRelativePath, quizCode + ".json"
            ));
    }

    private void LoadQuiz(string quizCode)
    {
        dataController.GetComponent<DataController>().PreLoadQuiz(quizCode);
        StartQuiz(quizCode);
    }

    private void StartQuiz(string quizCode)
    {
        SetUpRoundData(quizCode);
        
        //If it's the first time opening the game, run tutorial
        if (PlayerPrefs.GetInt("SkipTutorial") != 1)
        {
            PlayerPrefs.SetInt("SkipTutorial", 1);
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }

    IEnumerator GetFile(string quizCode)
    {
        string url = "https://ds-quiz.herokuapp.com/quiz?code=" + quizCode;

        using (www = UnityWebRequest.Get(url))
        {
            www.timeout = 10;
            loadingScreenGame.EnableLoadingGUI();

            //yield return www.SendWebRequest();
            www.SendWebRequest();

            while(!www.isDone)
            {
                // DOwnload PRogess
                loadingScreenGame.UpdateDownloadProgress(www.downloadProgress * 100 + "%");
                //textProgressObject
                yield return null;
            }

            // Check if the request was sucessful
            if (www.result != UnityWebRequest.Result.Success)
            {
                errorText = "Erro ao baixar o arquivo.\n" + www.error.ToString();
                //DisableLoadingUI();
            }
            else
            {
                // If the quiz is valid, a json data will be returned.
                if (!www.downloadHandler.text.Equals("null"))
                {
                    loadingScreenGame.UpdateDownloadProgress("100%");
                    // Build path quiz directory
                    string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar
                        + DataManagementConstant.QuizFolderRelativePath + quizCode + ".json";
                    Debug.Log("path: " + path);

                    dataController.GetComponent<DataController>().WriteOnPath(path, www.downloadHandler.text);
                }
            }

            // DisableLoadingUI();

            // Check if quiz's json is in correct place. 
            if (!www.downloadHandler.text.Equals("null") && isQuizAvailable(quizCode))
            {
                // downloadProgess.text = "OK";
                LoadQuiz(quizCode);
            }
            else
            {
                Debug.Log(errorText);
                loadingScreenGame.DisableLoadingGUI();
                errorController.GetComponent<PopupHandler>().InitialExitBehaviour("error");               
            }
        }
    }

    void SetUpRoundData(string quizCode)
    {

        RoundData roundData = new RoundData();

        string folderPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar +
            DataManagementConstant.PlayerDataFolder + Path.AltDirectorySeparatorChar + quizCode;

        // Crate Directory for quiz player data.
        if (!Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        string filePath = folderPath + Path.AltDirectorySeparatorChar + "data.json";

        roundData.FilePath = filePath;
        roundData.FolderPath = folderPath;

        dataController.CurrentRoundData = roundData;
    }
}
