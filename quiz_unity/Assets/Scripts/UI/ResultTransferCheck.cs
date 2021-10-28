using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultTransferCheck : MonoBehaviour
{
    public GameObject resultGameController;

    public bool isPOSTRequestDone()
    {
        return resultGameController.GetComponent<ResultGameController>().dataFormisDone;
    }
}
