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

    public Button m_startButton;
    public Text inputText;

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
        string quizCode = inputText.GetComponent<Text>().text;
        StartCoroutine(GetFile(quizCode));

        // Net Controller Search routine.
        netController.GetComponent<NetController>().RequestQuiz(quizCode);

        // Check if quiz's json is in correct place. 
        if (isQuizAvailable(quizCode))
        {
            LoadQuiz(quizCode);
        }
        else
        {
            // Display a popup message informing the user (Goes here).
        }
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

    IEnumerator GetFile(string file_name)
    {
        string url = "http://localhost/api/" + file_name + ".json"; 
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string savePath = string.Format("{0}/{1}.json", Application.streamingAssetsPath, file_name); //set file path
                System.IO.File.WriteAllText(savePath, www.downloadHandler.text); //download file
            }
        }
    }
}
