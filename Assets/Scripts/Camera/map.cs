using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    

public class Map : MonoBehaviour
{
    private static Material lineMaterial;

    public GameObject panelMap;
    public LayerMask layer;
    public GameObject plane;

    private int heightScreen, widthScreen;
    private float divHeight, divWidth;
    private float hMinMap, wMinMap, hMap, wMap;
    private float hRel, wRel;
    private Vector2 p1, p2, p3, p4;
    private Vector3 bounds;

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                "SubShader { Pass { " +
                "    Blend SrcAlpha OneMinusSrcAlpha " +
                "    ZWrite Off Cull Off Fog { Mode Off } " +
                "    BindChannels {" +
                "      Bind \"vertex\", vertex Bind \"color\", color }" +
                "} } }");
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    void Start()
    {
        bounds = plane.GetComponent<MeshFilter>().mesh.bounds.size;

        heightScreen = Screen.height;
        widthScreen = Screen.width;
        divHeight = heightScreen / 100.0f;
        divWidth = widthScreen / 100.0f;
        hMinMap = 29 * divHeight;
        wMinMap = 19 * divWidth;
        hMap = bounds.z * 2;
        wMap = bounds.x * 2;
        hRel = hMinMap / hMap;
        wRel = wMinMap / wMap;

        panelMap.SetActive(true);
        RectTransform rt = panelMap.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.01f, 0.01f);
        rt.anchorMax = new Vector2(0.2f, 0.3f);
    }

    void Update()
    {
        Ray ray1, ray2, ray3, ray4;
        ray1 = Camera.main.ScreenPointToRay(Vector3.zero);
        ray2 = Camera.main.ScreenPointToRay(new Vector3(widthScreen, 0, 0));
        ray3 = Camera.main.ScreenPointToRay(new Vector3(widthScreen, heightScreen, 0));
        ray4 = Camera.main.ScreenPointToRay(new Vector3(0, heightScreen, 0));
        RaycastHit hit1, hit2, hit3, hit4;

        if (Physics.Raycast(ray1, out hit1, 250.0f, layer) && hit1.transform.CompareTag("Terrain"))
        {
            Debug.DrawLine(ray1.origin, hit1.point, Color.red);
            p1 = new Vector2((hit1.point.x + bounds.x)*wRel + divWidth, (hit1.point.z + bounds.z)*hRel + divHeight);

        }
        if (Physics.Raycast(ray2, out hit2, 250.0f, layer) && hit2.transform.CompareTag("Terrain"))
        {
            Debug.DrawLine(ray2.origin, hit2.point, Color.red);
            p2 = new Vector2((hit2.point.x + bounds.x) * wRel + divWidth, (hit2.point.z + bounds.z) * hRel + divHeight);
        }
        if (Physics.Raycast(ray3, out hit3, 250.0f, layer) && hit3.transform.CompareTag("Terrain"))
        {
            Debug.DrawLine(ray3.origin, hit3.point, Color.red);
            p3 = new Vector2((hit3.point.x + bounds.x) * wRel + divWidth, (hit3.point.z + bounds.z) * hRel + divHeight);
        }
        if (Physics.Raycast(ray4, out hit4, 250.0f, layer) && hit4.transform.CompareTag("Terrain"))
        {
            Debug.DrawLine(ray4.origin, hit4.point, Color.red);
            p4 = new Vector2((hit4.point.x + bounds.x) * wRel + divWidth, (hit4.point.z + bounds.z) * hRel + divHeight);
        }
    }

    void OnPostRender()
    {
        CreateLineMaterial();
        // set the current material
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.LoadPixelMatrix();
        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex3(p1.x, p1.y, 0);
        GL.Vertex3(p2.x, p2.y, 0);

        GL.Vertex3(p2.x, p2.y, 0);
        GL.Vertex3(p3.x, p3.y, 0);

        GL.Vertex3(p3.x, p3.y, 0);
        GL.Vertex3(p4.x, p4.y, 0);

        GL.Vertex3(p4.x, p4.y, 0);
        GL.Vertex3(p1.x, p1.y, 0);

        //GL.Color(Color.blue);
        //GL.Vertex3(1, 1, 0);
        //GL.Vertex3(1, 2, 0);
        //GL.Vertex3(2, 1, 0);
        //GL.Vertex3(2, 2, 0);
        GL.End();
        GL.PopMatrix();
    }
}