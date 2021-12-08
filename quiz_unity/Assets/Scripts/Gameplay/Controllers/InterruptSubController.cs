using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InterruptSubController : MonoBehaviour
{

    private DataController dataController;

    private string gameSessionKey;
    private string interruptDataFolder;
    private string sessionKeyFilePath;
    private int interruptTypeCount = Enum.GetNames(typeof(GameMechanicsConstant.InterruptTypes)).Length;
    private int[] interruptTypeActivation;

    DateTime beforeInterruptionTimeStamp;
    DateTime backToForeground;

    public GameObject popUpHandler;

    public DateTime BeforeInterruptionTimeStamp 
    {
        get { return beforeInterruptionTimeStamp; }
        set { beforeInterruptionTimeStamp = value; }
    }

    public DateTime BackToForegroundTimeStamp
    {
        get { return backToForeground; }
        set { backToForeground = value; }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        interruptTypeActivation = new int[interruptTypeCount];
        dataController = FindObjectOfType<DataController>();
        gameSessionKey = dataController.generateSessionKey();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    public void RegisterInterrupt(GameMechanicsConstant.InterruptTypes interruptType, bool isGoingToBackground)
    {
        // Paths for file and folder.
        interruptDataFolder = Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.InterruptFolderPath;
        sessionKeyFilePath = interruptDataFolder + Path.AltDirectorySeparatorChar + dataController.QuizCode + ".txt";

        // BackToForeground or BackGroundAndKill
        if (!isGoingToBackground)
        {
            switch (interruptType)
            {
                case GameMechanicsConstant.InterruptTypes.BackToMenu:
                    BackToMenuStamp();
                    break;
                case GameMechanicsConstant.InterruptTypes.BackgroundToForegroud:
                    BackToForegroundStamp();
                    break;
            }
        }
        else
        {
            switch (interruptType)
            {
                case GameMechanicsConstant.InterruptTypes.BackgroundToForegroud:
                    RegisterFormatedStamp("000");
                    break;
                /*
                case GameMechanicsConstant.InterruptTypes.BackgroundAndKill:
                    BackgroundAndKillStamp();
                    break;
                */
            }
        }
    }

    private void BackToMenuStamp()
    {
        // Paths for file and folder.
        string interruptDataFolder = Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.InterruptFolderPath;
        string sessionKeyFilePath = interruptDataFolder + Path.AltDirectorySeparatorChar + dataController.QuizCode + ".txt";

        if (!File.Exists(sessionKeyFilePath))
        {
            // 1st insertion.
            dataController.WriteOnPath(sessionKeyFilePath, System.DateTime.Now.ToString("HH:mm:ss") + " " + dataController.DataSessionKey + " " + "100");
        }
        else
        {
            int[] interruptionStatus = new int[interruptTypeCount];

            // Set Up for reading strings
            string[] sessionData = new string[Enum.GetNames(typeof(DataManagementConstant.SessionDataInterruptInformation)).Length];
            string[] sessionDataLine = File.ReadAllLines(sessionKeyFilePath);

            sessionData = sessionDataLine[0].Split(' '); 

            // Save activated interrupts.
            for (int i = 0; i <interruptTypeCount; i++)
            {
                interruptionStatus[i] = int.Parse(sessionData[sessionData.Length - 1][i].ToString());
            }

            // 1xx
            string updatedInterruptStatus = 
                "1" + 
                interruptionStatus[(int) GameMechanicsConstant.InterruptTypes.BackgroundToForegroud].ToString() + 
                interruptionStatus[(int) GameMechanicsConstant.InterruptTypes.BackgroundAndKill].ToString();

            // FInally write the updated status.
            RegisterFormatedStamp(updatedInterruptStatus);
        }
    }

    private void BackToForegroundStamp()
    {
        // Paths for file and folder.
        string interruptDataFolder = Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.InterruptFolderPath;
        string sessionKeyFilePath = interruptDataFolder + Path.AltDirectorySeparatorChar + dataController.QuizCode + ".txt";

        int[] interruptionStatus = new int[interruptTypeCount];

        // Set Up for reading strings
        string[] sessionData = new string[Enum.GetNames(typeof(DataManagementConstant.SessionDataInterruptInformation)).Length];
        string[] sessionDataLine = File.ReadAllLines(sessionKeyFilePath);

        sessionData = sessionDataLine[0].Split(' ');

        // Save activated interrupts.
        for (int i = 0; i < interruptTypeCount; i++)
        {
            interruptionStatus[i] = int.Parse(sessionData[sessionData.Length - 1][i].ToString());
        }

        // x1x
        string updatedInterruptStatus = 
        interruptionStatus[(int)GameMechanicsConstant.InterruptTypes.BackToMenu].ToString() +
        "1" +
        interruptionStatus[(int)GameMechanicsConstant.InterruptTypes.BackgroundAndKill].ToString();

        // FInally write the updated status.
        RegisterFormatedStamp(updatedInterruptStatus);
    }

    private void BackgroundAndKillStamp()
    {
        // Paths for file and folder.
        string interruptDataFolder = Application.persistentDataPath + Path.AltDirectorySeparatorChar + DataManagementConstant.InterruptFolderPath;
        string sessionKeyFilePath = interruptDataFolder + Path.AltDirectorySeparatorChar + dataController.QuizCode + ".txt";

        int[] interruptionStatus = new int[interruptTypeCount];

        // Set Up for reading strings
        string[] sessionData = new string[Enum.GetNames(typeof(DataManagementConstant.SessionDataInterruptInformation)).Length];
        string[] sessionDataLine = File.ReadAllLines(sessionKeyFilePath);

        sessionData = sessionDataLine[0].Split(' ');

        // Save activated interrupts.
        for (int i = 0; i < interruptTypeCount; i++)
        {
            interruptionStatus[i] = int.Parse(sessionData[sessionData.Length - 1][i].ToString());
        }

        // x1x
        string updatedInterruptStatus =
        interruptionStatus[(int)GameMechanicsConstant.InterruptTypes.BackToMenu].ToString() +
        interruptionStatus[(int)GameMechanicsConstant.InterruptTypes.BackgroundToForegroud].ToString() +
        "1";

         // FInally write the updated status.
         RegisterFormatedStamp(updatedInterruptStatus);
    }

    private void RegisterFormatedStamp(string interruptStatus)
    {
        dataController.WriteOnPath(sessionKeyFilePath, System.DateTime.Now.ToString("HH:mm:ss") + " " + dataController.DataSessionKey + " " + interruptStatus);
    }

}
