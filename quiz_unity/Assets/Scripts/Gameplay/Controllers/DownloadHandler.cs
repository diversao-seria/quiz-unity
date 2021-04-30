using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadHandler : MonoBehaviour
{
    const String serverAdress = "http://127.0.0.1";
    const String serverPort = "80";

    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        // return  testConnection();
        return testFileDownload("hello.txt");
    }

    IEnumerator testConnection()
    {
        UnityWebRequest www = UnityWebRequest.Get(serverAdress + ":" + serverPort);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) // Deprecated
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    IEnumerator testFileDownload(String fileName)
    {
        UnityWebRequest www = UnityWebRequest.Get(serverAdress + "/" + fileName);
        yield return www.SendWebRequest();

        if (www.isNetworkError) // Deprecated
        {
            Debug.Log(www.error);
        }
        else
        {
            // BODY
            Debug.Log(www.downloadHandler.text);
            // Or retrieve results as binary data
            // byte[] results = www.downloadHandler.data;
            // saveFile()
        }
    }
}

    

