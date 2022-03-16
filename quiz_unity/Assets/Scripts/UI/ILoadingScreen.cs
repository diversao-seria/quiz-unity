using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ILoadingScreen 
{
    public void setMask(GameObject gameObject);
    public void setDownloadProgressText(Text gameObject);
    public void setSpinningWheelWrapper(GameObject gameObject);
    public void  UpdateDownloadProgress(string value);

    public void EnableLoadingGUI();
    public void DisableLoadingGUI();

}
