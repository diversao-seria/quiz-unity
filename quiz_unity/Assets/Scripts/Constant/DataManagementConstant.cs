using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// All paths're relative!
public static class DataManagementConstant 
{
    // Files
    public static readonly string PlayerQuizDataFile = "QuizAnswerData.json";
    public static readonly string UserSettingsFile = "settings.json";

    // Folders
    public static readonly string PlayerDataFolder= "PlayerData";
    public static readonly string QuizFolder = "Quizes"; 

    // >>>> Relative <<<< Paths
    public static readonly string PlayerDataPath = PlayerDataFolder + Path.AltDirectorySeparatorChar;
    public static readonly string QuizFolderRelativePath = QuizFolder + Path.AltDirectorySeparatorChar;
}
