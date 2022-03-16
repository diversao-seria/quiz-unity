using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenGame : MonoBehaviour, ILoadingScreen
{

    private GameObject m_spinningWheelWrapper;

    private Transform[] m_spinners;

    private GameObject m_mask;

    private Text m_downloadProgressText;

    public LoadingScreenGame()
    {

    }

    public LoadingScreenGame(GameObject spinningWheel, GameObject mask, Text text)
    {
        setSpinningWheelWrapper(spinningWheel);
        setMask(mask);
        setDownloadProgressText(text);
    }

    public void setMask(GameObject gameObject)
    {
        m_mask = gameObject;
    }

    public void setSpinningWheelWrapper(GameObject gameObject)
    {
        m_spinningWheelWrapper = gameObject;
        m_spinners = m_spinningWheelWrapper.transform.GetComponentsInChildren<Transform>(true);
    }

    public void setDownloadProgressText(Text gameObject)
    {
        m_downloadProgressText = gameObject;
    }

    public void UpdateDownloadProgress(string value)
    {
        m_downloadProgressText.GetComponent<Text>().text = value;
    }

    public void EnableLoadingGUI()
    {
        // Activate Wrapper
        m_spinningWheelWrapper.gameObject.SetActive(true);

        // Activate Mask
        m_mask.gameObject.SetActive(true);

        // Activate Spinners
        foreach(Transform spiinerPart in m_spinners)
        {
            spiinerPart.gameObject.SetActive(true);
        }

        // Activate Text UI
        m_downloadProgressText.gameObject.SetActive(true);
    }

    public void DisableLoadingGUI()
    {
        // Activate Wrapper
        m_spinningWheelWrapper.gameObject.SetActive(false);

        // Activate Mask
        m_mask.gameObject.SetActive(false);

        foreach (Transform spiinerPart in m_spinners)
        {
            spiinerPart.gameObject.SetActive(false);
        }

        m_downloadProgressText.gameObject.SetActive(false);
    }
}
