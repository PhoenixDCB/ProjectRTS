using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    

public class map : MonoBehaviour
{

    public GameObject panelMap;

    private int heightScreen, widthScreen;

    void Start()
    {
        heightScreen = Screen.height;
        widthScreen = Screen.width;

        panelMap.SetActive(true);
        RectTransform rt = panelMap.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.01f, 0.01f);
        rt.anchorMax = new Vector2(0.2f, 0.3f);
    }

    void Update()
    {
    }
}