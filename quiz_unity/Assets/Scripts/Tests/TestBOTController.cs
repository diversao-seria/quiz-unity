using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Text;
using System.Linq;
using System.ComponentModel;

public class TestBOTController : MonoBehaviour
{
    public GameObject errorController;
    public LoadingScreenGame loadingScreenGame;
    private DataController dataController;
    private UnityWebRequest www;
    private string serverURL = "http://ds-quiz.herokuapp.com/matches";

    public string errorText = "Erro ao baixar o arquivo!";

    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("OLA! ME chamo " + this.name + " e fui inicializado com sucesso!");
        // StartCoroutine(GetFile("YZUG"));
        // StartCoroutine(SendForm("YZUG"));
        string [] fileEntries = Directory.GetFiles(Path.Combine(Application.persistentDataPath + Path.AltDirectorySeparatorChar +
				DataManagementConstant.PlayerDataPath + "Teste" + Path.AltDirectorySeparatorChar));

        StartCoroutine(SendForm("Teste", fileEntries));
        // foreach(string fileName in fileEntries)
        // {
        //     // string [] parts = fileName.Split(); 
        //     string lastName = fileName.Split('/').Last();
            
        //     StartCoroutine(SendForm("Teste", lastName.Split('.')[0]));
        //     Debug.Log(lastName + " enviado.");
        // }

            // ProcessFile(fileName);

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
                // Debug.Log(www.downloadProgress);
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

    IEnumerator SendForm(string quizCode, string[] files)
    {
        foreach(string file in files)
        {
            string lastName = file.Split('/').Last();
            string fileName = lastName.Split('.')[0];
            Debug.Log("Antes FOrm");

            using (UnityWebRequest www = UnityWebRequest.Post(serverURL, "POST"))
            {


                // List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
                // formData.Add(new MultipartFormDataSection("ID=" + exampleID.ToString() + "&" + "TemponoQuiz=" + exampleTime + "&" + "RespostasCorretas=" + exampleCorrect.ToString()));
                // formData.Add(new MultipartFormFileSection("my file data", pathToMatchData));

                string pathToQuizResult = Path.Combine(Application.persistentDataPath + Path.AltDirectorySeparatorChar +
                    DataManagementConstant.PlayerDataPath + quizCode + Path.AltDirectorySeparatorChar + fileName + ".json");

                string jsonData = null;

                // loadingScreenGame = new LoadingScreenGame(spinner, fadeMask, textProgressObject);

                try
                {
                    jsonData = System.IO.File.ReadAllText(pathToQuizResult);
                }
                catch (IOException e)
                {
                    Debug.Log(e);
                }

                www.timeout = 10;


                // loadingScreenGame.EnableLoadingGUI();

                // Form Data
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                www.SetRequestHeader("Content-Type", "application/json");
                www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
                www.SendWebRequest();

                while (!www.isDone)
                {
                    // loadingScreenGame.UpdateDownloadProgress(www.downloadProgress * 100 + "%");
                    yield return null;
                }

                Debug.Log("Status Code: " + www.responseCode);
                // loadingScreenGame.DisableLoadingGUI();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    // Sem internet -> 0


                    Debug.Log(www.error);
                    Debug.Log(www.downloadHandler.text);
                    // TO DO - JANELA.

                }
                else
                {
                    // loadingScreenGame.UpdateDownloadProgress("100%");
                    Debug.Log("Form upload complete!");
                }

                // www.downloadHandler.Dispose();
                www.disposeCertificateHandlerOnDispose = true;
                www.disposeDownloadHandlerOnDispose = true;
                www.disposeUploadHandlerOnDispose = true;
                // www.uploadHandler.Dispose();
                www.Dispose();
                // Debug.Log("Rodou");
            }
            Debug.Log("Depois Form");
            // SceneManager.LoadScene("QuizResult");
            yield return new WaitForSeconds(5);
            Debug.Log("Rodando o próximo...");
        }
    }
}
