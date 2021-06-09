using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;

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
            Application.streamingAssetsPath, quizCode + ".json"
            ));
    }

    private void LoadQuiz(string quizCode)
    {
        // TO DO: make user wait
        dataController.GetComponent<DataController>().PreLoadQuiz(quizCode);
        StartQuiz();
    }

    private void StartQuiz()
    {
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
                string savePath = string.Format("{0}/{1}.json", Application.streamingAssetsPath, quizCode); //set file path
                System.IO.File.WriteAllText(savePath, www.downloadHandler.text); //download file
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
