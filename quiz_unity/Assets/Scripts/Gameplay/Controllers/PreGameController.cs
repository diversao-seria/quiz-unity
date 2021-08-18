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

    private UnityWebRequest www;

    void Awake()
    {
        m_startButton.onClick.AddListener(CheckForQuiz);
        dataController = FindObjectOfType<DataController>();
        netController = FindObjectOfType<NetController>();      
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
        // TO DO: make user wait
        downloadProgess.text = "0";
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

        EnableLoadingUI();

        using (www = UnityWebRequest.Get(url))
        {
            requestStatus = true;
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                requestStatus = false;
                errorText = "Erro ao baixar o arquivo.\n" + www.error.ToString();
                DisableLoadingUI();
                yield return null;
            }
            else
            {

                // Build path quiz directory
                string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar
                    + DataManagementConstant.QuizFolderRelativePath + quizCode + ".json";
                Debug.Log("path: " + path);

                dataController.GetComponent<DataController>().WriteOnPath(path, www.downloadHandler.text);
            }

            // Check if quiz's json is in correct place. 
            if (isQuizAvailable(quizCode))
            {
                downloadProgess.text = "OK";
                yield return new WaitForSeconds(1);
                // DisableLoadingUI();
                LoadQuiz(quizCode);
            }
            else
            {
                DisableLoadingUI();
                Debug.Log(errorText);
                errorController.GetComponent<PopupHandler>().InitialExitBehaviour("error");               
            }
            requestStatus = false;
        }
    }

    public UnityWebRequest GetRequestConnectionInstance()
    {
        return www;
    }

    public bool GetRequestStatus()
    {
        return requestStatus;
    }

    private void EnableLoadingUI()
    {
        spinner.transform.gameObject.SetActive(true);
        spinner.transform.GetComponentInChildren<Transform>();
        foreach (Transform spinnerPart in spinner.transform.GetComponentInChildren<Transform>())
        {
            Debug.Log("nome do objeto: " + spinnerPart.gameObject.name);
            spinnerPart.gameObject.SetActive(true);
        }

        downloadProgess.gameObject.SetActive(true);
        fadeMask.gameObject.SetActive(true);
    }

    private void DisableLoadingUI()
    {
        foreach (Transform spinnerPart in spinner.transform.GetComponentInChildren<Transform>())
        {
            Debug.Log("nome do objeto: " + spinnerPart.gameObject.name);
            spinnerPart.gameObject.SetActive(false);
        }
        spinner.transform.gameObject.SetActive(false);


        downloadProgess.gameObject.SetActive(false);
        fadeMask.gameObject.SetActive(false);
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


    // db: unityTest
    // table: playerruns
    // ID (P)(int), TemponoQuiz (string - varchar), RespostasCorretas (int)

    IEnumerator SendForm(string jsonData)
    {

        string url = "http://ds-quiz.herokuapp.com/matches";

        // List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // formData.Add(new MultipartFormDataSection("ID=" + exampleID.ToString() + "&" + "TemponoQuiz=" + exampleTime + "&" + "RespostasCorretas=" + exampleCorrect.ToString()));
        // formData.Add(new MultipartFormFileSection("my file data", pathToMatchData));

        UnityWebRequest www = UnityWebRequest.Post(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        Debug.Log("Status Code: " + www.responseCode);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }

        
    }
}
