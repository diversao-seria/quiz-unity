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

    void Start()
    {
        Debug.Log("OLA! ME chamo " + this.name + " e fui inicializado com sucesso!");

        if (!Directory.Exists(Path.Combine(Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerDataPath + "Teste" + Path.AltDirectorySeparatorChar)))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerDataPath + "Teste" + Path.AltDirectorySeparatorChar));
        }

        // Scan for quiz answer files
        string[] fileEntries = Directory.GetFiles(Path.Combine(Application.persistentDataPath + Path.AltDirectorySeparatorChar +
                DataManagementConstant.PlayerDataPath + "Teste" + Path.AltDirectorySeparatorChar));

        StartCoroutine(runTests("YZUG", "Teste", fileEntries));
    }


    IEnumerator runTests(string code, string folderName, string[] fileEntries)
    {
        yield return(StartCoroutine(GetFile(code)));
        yield return new WaitForSeconds(5);
        yield return(StartCoroutine(SendForm(folderName, fileEntries)));
        Debug.Log("Testes concluídos.");
    }


    IEnumerator GetFile(string quizCode)
    {
        string url = "https://ds-quiz.herokuapp.com/quiz?code=" + quizCode;

        using (www = UnityWebRequest.Get(url))
        {
            www.timeout = 10;

            www.SendWebRequest();

            while(!www.isDone)
            {
                yield return null;
            }

            // Check if the request was sucessful
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // If the quiz is valid, a json data will be returned.
                if (!www.downloadHandler.text.Equals("null"))
                {
                    // Build path quiz directory
                    string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar
                        + DataManagementConstant.QuizFolderRelativePath + quizCode + ".json";
                    Debug.Log("path: " + path);

                    WriteOnPath(path, www.downloadHandler.text);
                }
                else
                {
                    Debug.Log("Código inválido");
                    yield break;
                }
            }

            // Check if quiz's json is in correct place. 
            if (!www.downloadHandler.text.Equals("null") && isQuizAvailable(quizCode))
            {
                Debug.Log("Quiz Baixado!");
            }
            else
            {
                Debug.Log(errorText);          
            }
        }

        Debug.Log("Teste de download concluído. Iniciando teste de upload...");
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
		}
	}

    IEnumerator SendForm(string quizCode, string[] files)
    {
        // for each file in 
        foreach(string file in files)
        {
            string lastName = file.Split('/').Last();
            string fileName = lastName.Split('.')[0];

            using (UnityWebRequest www = UnityWebRequest.Post(serverURL, "POST"))
            {
                string pathToQuizResult = Path.Combine(Application.persistentDataPath + Path.AltDirectorySeparatorChar +
                    DataManagementConstant.PlayerDataPath + quizCode + Path.AltDirectorySeparatorChar + fileName + ".json");

                string jsonData = null;

                try
                {
                    jsonData = System.IO.File.ReadAllText(pathToQuizResult);
                }
                catch (IOException e)
                {
                    Debug.Log(e);
                }

                www.timeout = 10;

                // Form Data
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                www.SetRequestHeader("Content-Type", "application/json");
                www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
                www.SendWebRequest();

                while (!www.isDone)
                {
                    yield return null;
                }

                Debug.Log("Status Code: " + www.responseCode);

                if (www.result != UnityWebRequest.Result.Success)
                {
                    // Sem internet -> 0
                    Debug.Log(www.error);
                    Debug.Log("Erro ao enviar o arquivo " + fileName);
                }
                else
                {
                    Debug.Log(fileName + " enviado.");
                }

                www.disposeCertificateHandlerOnDispose = true;
                www.disposeDownloadHandlerOnDispose = true;
                www.disposeUploadHandlerOnDispose = true;
                www.Dispose();
            }
            
            yield return new WaitForSeconds(5);
        }
    }
}
