using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public GameObject m_exitPopup;

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

    public void ToggleObjectsFromParent(Transform transform, bool boolean)
    {
        transform.gameObject.SetActive(boolean);
        foreach (Transform child in transform)
        {
            ToggleObjectsFromParent(child, boolean);
        }
    }
}
