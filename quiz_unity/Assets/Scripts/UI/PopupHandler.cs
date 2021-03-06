﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitHandler : MonoBehaviour
{
    public GameObject screenController;
    public GameObject m_exitPopup;
    public GameObject m_errorPopup;
    public void ExitGame()
    {
        Application.Quit();
    }

    public void InitialExitBehaviour(string type)
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
            ToggleObjectsFromParent(m_errorPopup.transform, true);
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

}
