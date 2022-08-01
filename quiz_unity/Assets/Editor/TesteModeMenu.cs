using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

public class TesteModeMenu : EditorWindow
{

    Color defaultColor;
    bool testMode = false;

    // private string testBOTPrefabPath = "Assets" + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar + "TestBOT";

    GameObject testBOTprefab;
    GameObject testBOT;

    [MenuItem("Window/TesteModeMenu")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TesteModeMenu));
    }

    private void OnEnable()
    {
        
    }

    void OnGUI()
    {
        // The actual window code goes here
        // GUI.Box(new Rect(10, 50, 50, 50), "TEST MODE OFF");

        if (GUILayout.Button("Ligar ou Desligar o modo de teste."))
        {
            if(!testMode)
            {
                testMode = true;

                Scene entryPointScene = EditorSceneManager.GetSceneByBuildIndex(0);
                EditorSceneManager.SetActiveScene(entryPointScene);
                testBOTprefab = Resources.Load<GameObject>("TestBOT");
                testBOT = Instantiate(testBOTprefab);
                testBOT.name = "testBOT";
            }
            else
            {
                Scene entryPointScene = EditorSceneManager.GetSceneByBuildIndex(0);
                EditorSceneManager.SetActiveScene(entryPointScene);
                DestroyImmediate(testBOT);
                testMode = false;
            }
        }

        // GUI Update

        if (!testMode)
        {
            GUI.color = Color.grey;
            GUI.Box(new Rect(50, 50, 100, 75), "MODO DE TESTE DESLIGADO");
        }
        else
        {
            GUI.color = Color.red;
            GUI.Box(new Rect(50, 50, 100, 75), "MODO DE TESTE LIGADO");
        }
    }
}
