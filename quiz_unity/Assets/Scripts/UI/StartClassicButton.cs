using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartClassicButton : MonoBehaviour
{

    public GameObject quizCodeForm;

    public DataController dataController;

    // Start is called before the first frame update
    public void startQuiz()
    {
        string text = quizCodeForm.GetComponent<Text>().text;        // Loading?
        // IF: everything is ready
        //      changeScene;
        Debug.Log("StartQuizz callback");
    }

    private void checkForQuiz()
    {
        // DataController do yout yhing

    }
}
