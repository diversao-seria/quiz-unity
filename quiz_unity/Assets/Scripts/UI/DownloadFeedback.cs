using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownloadFeedback : MonoBehaviour
{

    public Text text;

    private PreGameController preGameController;

    // Start is called before the first frame update
    void Start()
    {
        preGameController = FindObjectOfType<PreGameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(preGameController.GetRequestStatus())
        {
            StartCoroutine(updatePercentage());
        }
    }

    public IEnumerator updatePercentage()
    {
        preGameController.downloadProgess.text = (preGameController.GetRequestConnectionInstance().downloadProgress * 100).ToString() + "%";
        yield return new WaitForSeconds(0.4f);
    }
}
