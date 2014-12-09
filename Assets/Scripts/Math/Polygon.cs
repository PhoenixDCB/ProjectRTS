using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Polygon 
{
    public static bool Inside(Vector3 point, List<Vector3> listOfPoints)
    {
        int i, j, nvert = listOfPoints.Count;
        bool c = false;
      
        for(i = 0, j = nvert - 1; i < nvert; j = i++) 
        {
            if 
            (
                ((listOfPoints[i].z >= point.z) != (listOfPoints[j].z >= point.z)) &&
                (point.x <= (listOfPoints[j].x - listOfPoints[i].x) * (point.z - listOfPoints[i].z) / (listOfPoints[j].z - listOfPoints[i].z) + listOfPoints[i].x)
            )
            c = !c;
        }

        return c;
    }
}
