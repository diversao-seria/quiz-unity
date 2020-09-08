using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchControl : MonoBehaviour, IPointerDownHandler
{
    //
    // ENQUANTO Enquanto botão está apertado :
    // -- aumenta o valor do slider
    // -- if(slider >= 1.0) próxima pergunta (cena)
    // FIM-ENQUANTO
    // slider = 0.0;

    private Button oldButtonPressed;
    private Button actualButtonPressed;


    // Update is called once per frame
    void Update()
    {
     
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name + " Was Clicked.");
    }
}
