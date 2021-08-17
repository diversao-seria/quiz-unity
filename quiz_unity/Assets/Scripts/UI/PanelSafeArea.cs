using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSafeArea : MonoBehaviour
{
    public Canvas canvas;
    public bool refresh = false;

    RectTransform panelSafeArea;

    //Rect currentSafeArea = new Rect();
    //ScreenOrientation currentOrientation = ScreenOrientation.AutoRotation;

    // Start is called before the first frame update
    void Awake()
    {
        panelSafeArea = GetComponent<RectTransform>();

        //currentOrientation = Screen.orientation;
        //currentSafeArea = Screen.safeArea;

        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        if (panelSafeArea == null)
            return;

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;


        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        panelSafeArea.anchorMin = anchorMin;
        panelSafeArea.anchorMax = anchorMax;

        //currentOrientation = Screen.orientation;
        //currentSafeArea = Screen.safeArea;

    }

    // Update is called once per frame
    void Update()
    {

        //BUG - When using ApplySafeArea() inside this if anchor values get bugged, when using directly on Update it works
        //if (currentOrientation != Screen.orientation )//|| currentSafeArea != Screen.safeArea)
        if(refresh)
        {
            ApplySafeArea();
        }
    }
}
