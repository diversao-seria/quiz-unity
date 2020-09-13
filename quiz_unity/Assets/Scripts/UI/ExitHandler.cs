using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitHandler : MonoBehaviour
{
    MenuScreenController menuScreenController;

    private void Start()
    {
        menuScreenController = FindObjectOfType<MenuScreenController>();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void FinishExitBehaviour()
    {
        Button[] buttons = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<Button>();

        // Make anything child of canvas not interactable.
        foreach (Button button in buttons)
        {
            button.enabled = true;
        }

        menuScreenController.ToggleObjectsFromParent(menuScreenController.m_exitPopup.transform, false);
    }

}
