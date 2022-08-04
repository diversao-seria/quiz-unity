using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Text;

public class TestBOTController : MonoBehaviour
{
    public GameObject errorController;
    public LoadingScreenGame loadingScreenGame;
    private DataController dataController;
    private UnityWebRequest www;

    public string errorText = "Erro ao baixar o arquivo!";

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("OLA! ME chamo " + this.name + " e fui inicializado com sucesso!");
        StartCoroutine(GetFile("YZUG"));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetFile(string quizCode)
    {
        string url = "https://ds-quiz.herokuapp.com/quiz?code=" + quizCode;

        using (www = UnityWebRequest.Get(url))
        {
            www.timeout = 10;
            // loadingScreenGame.EnableLoadingGUI();

            //yield return www.SendWebRequest();
            
            www.SendWebRequest();

            while(!www.isDone)
            {
                // DOwnload PRogess
                // loadingScreenGame.UpdateDownloadProgress(www.downloadProgress * 100 + "%");
                //textProgressObject
                yield return null;
            }

            // Check if the request was sucessful
            if (www.result != UnityWebRequest.Result.Success)
            {
                // loadingScreenGame.DisableLoadingGUI();
                // errorController.GetComponent<PopupHandler>().InitialExitBehaviour("error");
                Debug.Log(www.error);
                //DisableLoadingUI();
            }
            else
            {
                // If the quiz is valid, a json data will be returned.
                if (!www.downloadHandler.text.Equals("null"))
                {
                    // loadingScreenGame.UpdateDownloadProgress("100%");
                    // Build path quiz directory
                    string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar
                        + DataManagementConstant.QuizFolderRelativePath + quizCode + ".json";
                    Debug.Log("path: " + path);

                    WriteOnPath(path, www.downloadHandler.text);
                }
                else
                {
                    // loadingScreenGame.DisableLoadingGUI();
                    // errorController.GetComponent<PopupHandler>().InitialExitBehaviour("error", "Código inválido");
                    Debug.Log("Código inválido");
                    yield break;
                }
            }

            // DisableLoadingUI();

            // Check if quiz's json is in correct place. 
            if (!www.downloadHandler.text.Equals("null") && isQuizAvailable(quizCode))
            {
                // downloadProgess.text = "OK";
                // LoadQuiz(quizCode);
            }
            else
            {
                Debug.Log(errorText);
                // loadingScreenGame.DisableLoadingGUI();
                // errorController.GetComponent<PopupHandler>().InitialExitBehaviour("error");               
            }
        }
    }

    private bool isQuizAvailable(string quizCode)
    {
        return File.Exists(Path.Combine(
            Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.QuizFolderRelativePath, quizCode + ".json"
            ));
    }

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

}
