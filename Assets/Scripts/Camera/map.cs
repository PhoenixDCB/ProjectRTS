using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    

public class Map : MonoBehaviour
{
    private static Material lineMaterial;

    //public GameObject panelMap;
    public LayerMask layer;
    public GameObject plane;

    private int heightScreen, widthScreen;
    private float divHeightMap, divWidthMap, divHeightMinMap, divWidthMinMap;
    private float hMinMap, wMinMap, hMap, wMap;
    private float hRel, wRel;
    private Vector2 p1, p2, p3, p4;
    private Vector3 bounds;
    private Texture2D textBackgroundMinimap;
    private Rect[][] minimapMatrix;
    private Texture2D textUnit;
    private Rect[] units;

    //------------------------------------------

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
        // bounds of terrain
        bounds = plane.GetComponent<MeshFilter>().mesh.bounds.size;
        // attributes of map and minimap
        heightScreen = Screen.height;
        widthScreen = Screen.width;
        divHeightMap = heightScreen / 100.0f;
        divWidthMap = widthScreen / 100.0f;
        hMinMap = 30 * divHeightMap;
        wMinMap = 20 * divWidthMap;
        divHeightMinMap = hMinMap / 100.0f;
        divWidthMinMap = wMinMap / 100.0f;
        hMap = bounds.z * 2;
        wMap = bounds.x * 2;
        hRel = hMinMap / hMap;
        wRel = wMinMap / wMap;
        // background texture for minimap
        textBackgroundMinimap = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        textBackgroundMinimap.SetPixel(0, 0, Color.black);
        textBackgroundMinimap.SetPixel(1, 0, Color.black);
        textBackgroundMinimap.SetPixel(0, 1, Color.black);
        textBackgroundMinimap.SetPixel(1, 1, Color.black);
        textBackgroundMinimap.Apply();
        // minimap matrix
        minimapMatrix = new Rect[100][];
        for (int i = 0; i < 100; i++)
        {
            minimapMatrix[i] = new Rect[100];
            for (int j = 0; j < 100; j++)
            {
                Vector2 pos = PointInvertedIntoMinimap(new Vector2(i * divWidthMinMap, j * divHeightMinMap));
                //minimapMatrix[i][j] = new Rect(i * divWidthMinMap + 0.01f * widthScreen, (heightScreen - 0.01f * heightScreen - (100 - j) * divHeightMinMap), divWidthMinMap, divHeightMinMap);
                minimapMatrix[i][j] = new Rect(pos.x, pos.y, divWidthMinMap, divHeightMinMap);
            }
        }
        // texture units
        textUnit = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        textUnit.SetPixel(0, 0, Color.green);
        textUnit.SetPixel(1, 0, Color.green);
        textUnit.SetPixel(0, 1, Color.green);
        textUnit.SetPixel(1, 1, Color.green);
        textUnit.Apply();
        // units
        units = new Rect[InformationUnits.units.Count];

        //panelMap.SetActive(true);
        //RectTransform rt = panelMap.GetComponent<RectTransform>();
        //rt.anchorMin = new Vector2(0.01f, 0.01f);
        //rt.anchorMax = new Vector2(0.2f, 0.3f);
    }

    void Update()
    {
        // camera projection
        Ray ray1, ray2, ray3, ray4;
        ray1 = Camera.main.ScreenPointToRay(Vector3.zero);
        ray2 = Camera.main.ScreenPointToRay(new Vector3(widthScreen, 0, 0));
        ray3 = Camera.main.ScreenPointToRay(new Vector3(widthScreen, heightScreen, 0));
        ray4 = Camera.main.ScreenPointToRay(new Vector3(0, heightScreen, 0));
        RaycastHit hit1, hit2, hit3, hit4;

        if (Physics.Raycast(ray1, out hit1, 250.0f, layer) && hit1.transform.CompareTag("Terrain"))
        {
            Debug.DrawLine(ray1.origin, hit1.point, Color.red);
            p1 = Worldmap2Minimap(hit1.point);

        }
        if (Physics.Raycast(ray2, out hit2, 250.0f, layer) && hit2.transform.CompareTag("Terrain"))
        {
            Debug.DrawLine(ray2.origin, hit2.point, Color.red);
            p2 = Worldmap2Minimap(hit2.point);
        }
        if (Physics.Raycast(ray3, out hit3, 250.0f, layer) && hit3.transform.CompareTag("Terrain"))
        {
            Debug.DrawLine(ray3.origin, hit3.point, Color.red);
            p3 = Worldmap2Minimap(hit3.point);
        }
        if (Physics.Raycast(ray4, out hit4, 250.0f, layer) && hit4.transform.CompareTag("Terrain"))
        {
            Debug.DrawLine(ray4.origin, hit4.point, Color.red);
            p4 = Worldmap2Minimap(hit4.point);
        }
        // units position
        for (int i = 0; i < InformationUnits.units.Count; i++)
        {
            Vector2 pos = Worldmap2Minimap(InformationUnits.units[i].transform.position);
            pos = PointSymmetricIntoMinimap(pos);
            units[i] = new Rect(pos.x, pos.y, 2, 2);
        }
    }

    void OnGUI()
    {
        // minimap background
        for (int i = 0; i < 100; i++)
            for (int j = 0; j < 100; j++)
            {
                GUI.DrawTexture(minimapMatrix[i][j], textBackgroundMinimap);
            }
        // units
        for (int i = 0; i < InformationUnits.units.Count; i++)
        {
            GUI.DrawTexture(units[i], textUnit);
        }
        // camera projection
        GLPaint();
    }

    //----------------------------------

    // Camera projection
    private void GLPaint()
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
        GL.End();
        GL.PopMatrix();
    }

    // Transform a world point into a minimap point
    private Vector2 Worldmap2Minimap(Vector3 worldPoint)
    {
        return new Vector2((worldPoint.x + bounds.x) * wRel + divWidthMap, (worldPoint.z + bounds.z) * hRel + divHeightMap);
    }

    private Vector2 PointInvertedIntoMinimap(Vector2 point)
    {
        return new Vector2(point.x + 0.01f * widthScreen, heightScreen - 0.01f * heightScreen - hMinMap + point.y);
    }

    private Vector2 PointSymmetricIntoMinimap(Vector2 point)
    {
        return new Vector2(point.x, heightScreen - point.y);
    }
}