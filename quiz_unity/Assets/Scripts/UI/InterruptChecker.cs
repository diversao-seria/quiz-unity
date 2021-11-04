using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InterruptChecker : MonoBehaviour
{

    private GameObject dataControllerObject;
    public GameObject jsonControllerObject;

    private DataController dataController;
    private JsonController jsonController;

    private string interruptQuizListFileName = "interruptedQuiz.txt";

    public void Start()
    {
        dataController = FindObjectOfType<DataController>();
        jsonController = jsonControllerObject.GetComponent<JsonController>();
    }

    public void RegisterInterrupt(string eventName)
    {
        // Paths for file and folder.
        string pathToInterruptListFile = Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerDataPath + Path.AltDirectorySeparatorChar + interruptQuizListFileName;
        string playerDataFolder = Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerDataPath;

        // Check if folder where the file will be stored exists.
        try
        {
            if (!Directory.Exists(playerDataFolder))
            {
                System.IO.Directory.CreateDirectory(Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.PlayerDataFolder);
            }
        }
        catch (IOException e)
        {
            Debug.Log(e + ". Caminho é arquivou ou armazenamento cheio.");
        }

        // Check if InterruptListFile Exists
        if (File.Exists(pathToInterruptListFile))
        {
            // Get All quiz codes in the file.
            string[] AllQuizCodes = File.ReadAllLines(pathToInterruptListFile);

            // Check for repeatable code.
            bool isAlreadyInFile = false;

            // 
            foreach (string quizCode in AllQuizCodes)
            {
                if (string.Equals(dataController.GetComponent<DataController>().QuizCode, quizCode))
                {
                    isAlreadyInFile = true;
                    break;
                }
            }

            // If the quizCode wasn't in the file, include it.
            if (!isAlreadyInFile)
            {
                using (StreamWriter writer = File.AppendText(pathToInterruptListFile))
                {
                    writer.WriteLine(dataController.GetComponent<DataController>().QuizCode + "\n");
                }
            }
            else
            {
                Debug.Log("Quizcode ja incluido.");
                // Do something?
            }
        }
        else
        {
            // First Quiz code on File, just create the file and write on it.
            dataController.GetComponent<DataController>().WriteOnPath(pathToInterruptListFile, dataController.GetComponent<DataController>().QuizCode);
        }

        this.GetComponent<PopupHandler>().ReturnToMainMenu();

        // dataController.WriteOnPath(pathToInterruptListFile + Application.persistentDataPath + interruptQuizListFileName, dataController.QuizCode);
    }
}
