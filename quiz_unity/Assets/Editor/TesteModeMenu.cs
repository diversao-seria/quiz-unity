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

    string sceneFolderPath = "Assets" + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar;

    Scene firstTestScene;
    string firstTestScene_name = "CenaTesteInicial";

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
                // Test mode is on
                testMode = true;

                // Scene Creation
                firstTestScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
                firstTestScene.name = firstTestScene_name;
                
                // Set as active scene and put stuff...
                EditorSceneManager.SetActiveScene(firstTestScene);
                testBOTprefab = Resources.Load<GameObject>("TestBOT"); // Load prehab
                testBOT = Instantiate(testBOTprefab);
                testBOT.name = "testBOT";

                // Save it.
                EditorSceneManager.SaveScene(firstTestScene, sceneFolderPath + firstTestScene_name + ".unity");

            }
            else
            {
                testMode = false;

                EditorSceneManager.OpenScene("Assets" + Path.AltDirectorySeparatorChar + "Scenes" + Path.AltDirectorySeparatorChar + "Persistent" + ".unity");
                EditorSceneManager.CloseScene(firstTestScene, true);

                // DO NOT CHANGE THIS.
                AssetDatabase.DeleteAsset(sceneFolderPath + firstTestScene_name + ".unity");
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
