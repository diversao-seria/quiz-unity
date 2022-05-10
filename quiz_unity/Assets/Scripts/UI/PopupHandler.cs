using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupHandler : MonoBehaviour
{
    public GameObject screenController;
    public GameObject m_exitPopup;
    public GameObject m_errorPopup;

    // Relevant in-game purpose
    public GameController gameController;

    public void Start()
    {
 
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void InitialExitBehaviour(string type, string message = "Erro ao acessar o quiz")
    {
        Button[] buttons = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<Button>();

        // Make anything child of canvas not interactable.
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }

        if (type == "exit")
        {
            ToggleObjectsFromParent(m_exitPopup.transform, true);
        }
        else
        {
            GameObject newError = Instantiate(m_errorPopup, new Vector3(0, 0, 0), transform.rotation) as GameObject;
            newError.transform.SetParent(GameObject.Find("PanelSafeArea").transform, false);
            newError.transform.GetChild(0).gameObject.GetComponent<Text>().text = message;

        }

    }

    public void InitialExitBehaviourFlagInterrupt(string type)
    {
        Button[] buttons = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<Button>();
        //

        // Make anything child of canvas not interactable.
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }

        if (type == "exit")
        {   
            ToggleObjectsFromParent(m_exitPopup.transform, true);
            
        }
        else
        {
            GameObject newError = Instantiate(m_errorPopup, transform.position, transform.rotation) as GameObject;             
            newError.transform.SetParent(GameObject.Find("PanelSafeArea").transform, false);
            // ToggleObjectsFromParent(m_errorPopup.transform, true);
            // m_errorPopup.GetComponent<Text>().text = message;

        }

    }


    private void ToggleObjectsFromParent(Transform transform, bool boolean)
    {
        transform.gameObject.SetActive(boolean);
        foreach (Transform child in transform)
        {
            ToggleObjectsFromParent(child, boolean);
        }
    }

    public void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ReturnToMainMenuInterruptSafe()
    {
        gameController.GetComponent<InterruptSubController>().RegisterInterrupt(GameMechanicsConstant.InterruptTypes.BackToMenu, false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ReturnToMainWebSafe()
    {
        // Check if web was sucessfull

        while (!this.GetComponent<ResultTransferCheck>().isPOSTRequestDone())
        {
            Debug.Log("waiting... rquest status");// Loading UI
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void FinishExitBehaviour(string type)
    {
        Button[] buttons = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<Button>();

        // Make anything child of canvas interactable.
        foreach (Button button in buttons)
        {
            button.enabled = true;
        }

        if (type == "exit")
        {
            ToggleObjectsFromParent(m_exitPopup.transform, false);
        }     
        else
        {
            ToggleObjectsFromParent(m_errorPopup.transform, false);
        }
    }

    public void FinishExitBehaviourWebSafe(string type)
    {
        Button[] buttons = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<Button>();

        // Make anything child of canvas interactable.
        foreach (Button button in buttons)
        {
            button.enabled = true;
        }

        // Enabled UI

        while(!this.GetComponent<ResultTransferCheck>().isPOSTRequestDone())
        {
            Debug.Log("waiting... rquest status");// Loading UI
        }

        // IF -> connection failst...

        // Disable UI

        if (type == "exit")
        {
            ToggleObjectsFromParent(m_exitPopup.transform, false);
        }
        else
        {
            ToggleObjectsFromParent(m_errorPopup.transform, false);
        }
    }

    public void DestroyPopUp(GameObject obj){
        Button[] buttons = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<Button>();

        // Make anything child of canvas interactable.
        foreach (Button button in buttons)
        {
            button.enabled = true;
        }

        Destroy(obj);
    }
}
