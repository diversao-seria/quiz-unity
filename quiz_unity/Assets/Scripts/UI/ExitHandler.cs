using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitHandler : MonoBehaviour
{
    public GameObject screenController;
    public GameObject m_exitPopup;
    public void ExitGame()
    {
        Application.Quit();
    }

    public void InitialExitBehaviour()
    {
        Button[] buttons = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<Button>();

        // Make anything child of canvas not interactable.
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }

        ToggleObjectsFromParent(m_exitPopup.transform, true);
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

    public void FinishExitBehaviour()
    {
        Button[] buttons = FindObjectOfType<Canvas>().gameObject.GetComponentsInChildren<Button>();

        // Make anything child of canvas interactable.
        foreach (Button button in buttons)
        {
            button.enabled = true;
        }

        ToggleObjectsFromParent(m_exitPopup.transform, false);
    }

}
