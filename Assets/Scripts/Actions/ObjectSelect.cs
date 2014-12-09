using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectSelect : MonoBehaviour
{
    public GameObject panel;

    private GameObject lastUnit;    

    private int heightScreen, widthScreen;
    private Vector3 initMousePos;

	// Use this for initialization
	void Start () 
    {
        heightScreen = Screen.height;
        widthScreen = Screen.width;
	}
	
	// Update is called once per frame
	void Update () 
    {
        // SINGLE SELECTION
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 250.0f) && hit.transform.CompareTag("Unit"))
        {
            GameObject unit = hit.transform.gameObject;
            if (lastUnit != null && unit != lastUnit) lastUnit.renderer.material.color = Color.green;
            lastUnit = unit;
            lastUnit.renderer.material.color = Color.yellow;
        }
        else if (lastUnit != null) lastUnit.renderer.material.color = Color.green;

        // MULTIPLE SELECTION
        if (Input.GetMouseButton(0))
        {
            panel.SetActive(true);
            RectTransform rt = panel.GetComponent<RectTransform>();
            //rt.anchorMax = new Vector2(Input.mousePosition.x / widthScreen, Input.mousePosition.y / heightScreen);

            Vector3 sizeRect = Input.mousePosition - initMousePos;
            // Case 1 (left, bottom -> right, top)
            if (sizeRect.x > 0 && sizeRect.y > 0)
            {
                rt.anchorMin = new Vector2(initMousePos.x / widthScreen, initMousePos.y / heightScreen);
                rt.anchorMax = new Vector2(Input.mousePosition.x / widthScreen, Input.mousePosition.y / heightScreen);
            }
            // Case 2 (right, bottom -> left, top)
            else if (sizeRect.x < 0 && sizeRect.y > 0)
            {
                rt.anchorMin = new Vector2(Input.mousePosition.x / widthScreen, initMousePos.y / heightScreen);
                rt.anchorMax = new Vector2(initMousePos.x / widthScreen, Input.mousePosition.y / heightScreen);
            }
            // Case 3 (left, top -> right, bottom)
            else if (sizeRect.x > 0 && sizeRect.y < 0)
            {
                rt.anchorMin = new Vector2(initMousePos.x / widthScreen, Input.mousePosition.y / heightScreen);
                rt.anchorMax = new Vector2(Input.mousePosition.x / widthScreen, initMousePos.y / heightScreen);
            }
            // Case 4 (right, top -> left, bottom)
            else if (sizeRect.x < 0 && sizeRect.y < 0)
            {
                rt.anchorMin = new Vector2(Input.mousePosition.x / widthScreen, Input.mousePosition.y / heightScreen);
                rt.anchorMax = new Vector2(initMousePos.x / widthScreen, initMousePos.y / heightScreen);
            }
        }
        else
        {
            panel.SetActive(false);
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.zero;
            //rt.anchorMin = new Vector2(Input.mousePosition.x / widthScreen, Input.mousePosition.y / heightScreen);
            initMousePos = Input.mousePosition;
        }
	}
}
