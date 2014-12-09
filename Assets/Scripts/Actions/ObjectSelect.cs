using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

            Ray ray1, ray2, ray3, ray4;
            ray1 = ray2 = ray3 = ray4 = ray;
            Vector3 sizeRect = Input.mousePosition - initMousePos;
            // Case 1 (left, bottom -> right, top)
            if (sizeRect.x > 0 && sizeRect.y > 0)
            {
                rt.anchorMin = new Vector2(initMousePos.x / widthScreen, initMousePos.y / heightScreen);
                rt.anchorMax = new Vector2(Input.mousePosition.x / widthScreen, Input.mousePosition.y / heightScreen);

                ray1 = Camera.main.ScreenPointToRay(new Vector2(initMousePos.x, initMousePos.y));
                ray2 = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, initMousePos.y));
                ray3 = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                ray4 = Camera.main.ScreenPointToRay(new Vector2(initMousePos.x, Input.mousePosition.y));
                
            }
            // Case 2 (right, bottom -> left, top)
            else if (sizeRect.x < 0 && sizeRect.y > 0)
            {
                rt.anchorMin = new Vector2(Input.mousePosition.x / widthScreen, initMousePos.y / heightScreen);
                rt.anchorMax = new Vector2(initMousePos.x / widthScreen, Input.mousePosition.y / heightScreen);

                ray1 = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, initMousePos.y));
                ray2 = Camera.main.ScreenPointToRay(new Vector2(initMousePos.x, initMousePos.y));
                ray3 = Camera.main.ScreenPointToRay(new Vector2(initMousePos.x, Input.mousePosition.y));
                ray4 = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            }
            // Case 3 (left, top -> right, bottom)
            else if (sizeRect.x > 0 && sizeRect.y < 0)
            {
                rt.anchorMin = new Vector2(initMousePos.x / widthScreen, Input.mousePosition.y / heightScreen);
                rt.anchorMax = new Vector2(Input.mousePosition.x / widthScreen, initMousePos.y / heightScreen);

                ray1 = Camera.main.ScreenPointToRay(new Vector2(initMousePos.x, Input.mousePosition.y));
                ray2 = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                ray3 = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, initMousePos.y));
                ray4 = Camera.main.ScreenPointToRay(new Vector2(initMousePos.x, initMousePos.y));                
            }
            // Case 4 (right, top -> left, bottom)
            else if (sizeRect.x < 0 && sizeRect.y < 0)
            {
                rt.anchorMin = new Vector2(Input.mousePosition.x / widthScreen, Input.mousePosition.y / heightScreen);
                rt.anchorMax = new Vector2(initMousePos.x / widthScreen, initMousePos.y / heightScreen);

                ray1 = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                ray2 = Camera.main.ScreenPointToRay(new Vector2(initMousePos.x, Input.mousePosition.y));
                ray3 = Camera.main.ScreenPointToRay(new Vector2(initMousePos.x, initMousePos.y));
                ray4 = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, initMousePos.y));
            }

            // 4 hits
            RaycastHit hit1, hit2, hit3, hit4;
            Physics.Raycast(ray1, out hit1, 250.0f);
            Physics.Raycast(ray2, out hit2, 250.0f);
            Physics.Raycast(ray3, out hit3, 250.0f);
            Physics.Raycast(ray4, out hit4, 250.0f);
            // DEBUG
            Debug.DrawLine(ray1.origin, hit1.point, Color.green);
            Debug.DrawLine(ray2.origin, hit2.point, Color.yellow);
            Debug.DrawLine(ray3.origin, hit3.point, Color.red);
            Debug.DrawLine(ray4.origin, hit4.point, Color.blue);

            for (int i = 0; i < InformationUnits.units.Count; i++)
            {
                GameObject unit = (GameObject)InformationUnits.units[i];
                List<Vector3> listOfPoints = new List<Vector3>();
                listOfPoints.Add(hit1.point);
                listOfPoints.Add(hit2.point);
                listOfPoints.Add(hit3.point);
                listOfPoints.Add(hit4.point);
                if (Polygon.Inside(unit.transform.position, listOfPoints)) unit.renderer.material.color = Color.yellow;
                else unit.renderer.material.color = Color.green;
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
