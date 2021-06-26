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
        dataController.GetComponent<DataController>().PreLoadQuiz(quizCode);
        StartQuiz(quizCode);
    }

    private void StartQuiz(string quizCode)
    {
        SetUpRoundData(quizCode);
        SceneManager.LoadScene("Game");
    }

    IEnumerator GetFile(string quizCode)
    {
        string url = "https://ds-quiz.herokuapp.com/quiz?code=" + quizCode;
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                errorText = "Erro ao baixar o arquivo.\n" + www.error.ToString();
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
                LoadQuiz(quizCode);
            }
            else
            {
                Debug.Log(errorText);
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

    // db: unityTest
    // table: playerruns
    // ID (P)(int), TemponoQuiz (string - varchar), RespostasCorretas (int)

    /* 
    IEnumerator SendForm()
    {

        int exampleID = 111;
        string exampleTime = "00:00:30";
        int exampleCorrect = 5;

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // formData.Add(new MultipartFormDataSection("ID=" + exampleID.ToString() + "&" + "TemponoQuiz=" + exampleTime + "&" + "RespostasCorretas=" + exampleCorrect.ToString()));
        formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", formData);
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
    */
}
