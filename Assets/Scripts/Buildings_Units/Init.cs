using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
        InformationUnits.units.Add(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
