using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour 
{
    private Quaternion rotationUp;
    private Quaternion rotationDown;


	// Use this for initialization
	void Start () 
    {
        rotationUp = Quaternion.Euler(new Vector3(45, 45, 0));
        rotationDown = Quaternion.Euler(new Vector3(30, 45, 0));
	}
	
	// Update is called once per frame
	void Update () 
    {
        // Move
        if (Input.GetKey(KeyCode.UpArrow)) transform.position += (Vector3.forward + Vector3.right) *  Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow)) transform.position -= (Vector3.forward + Vector3.right) * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) transform.position += (-Vector3.forward + Vector3.right) * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow)) transform.position -= (-Vector3.forward + Vector3.right) * Time.deltaTime;

        // Zoom
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.position += Vector3.up * 0.1f;
            transform.Rotate(1, 0, 0);

            if (transform.position.y > 2) 
            {
                transform.position = new Vector3(transform.position.x, 2, transform.position.z);
                transform.rotation = rotationUp;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.position -= Vector3.up * 0.1f;
            transform.Rotate(-1, 0, 0);

            if (transform.position.y < 0.5f)
            {
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                transform.rotation = rotationDown;
            }
        }
	}
}
