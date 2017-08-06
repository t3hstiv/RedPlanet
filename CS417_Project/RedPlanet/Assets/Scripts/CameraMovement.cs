using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    //How fast the camera moves
    public float speed = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        MoveCamera();
	}

    void MoveCamera()
    {
        //Zoom in with mouse wheel, on the y axis
        if (Input.GetAxis("Mouse ScrollWheel") > 0) //&& transform.position.y > 40)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 2*speed, transform.position.z);
        }

        //Zoom out with mouse wheel, on the y axis
        if (Input.GetAxis("Mouse ScrollWheel") < 0) //&& transform.position.y < 130)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 2*speed, transform.position.z);
        }

        //Camera moves right on x axis when mouse is near right edge of window
        if (Input.mousePosition.x >= (Screen.width * 0.99) && transform.position.x < 100)
        {
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }

        //Camera moves left on x axis when mouse is near left edge of window
        if (Input.mousePosition.x <= (Screen.width * 0.01) && transform.position.x > 50)
        {
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        }

        //Camera moves up on z axis when mouse is near upper edge of window
        if (Input.mousePosition.y >= (Screen.height * 0.99) && transform.position.z < 35)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed);
        }

        //Camera moves down on z axis when mouse is near lower edge of window
        if (Input.mousePosition.y <= (Screen.height * 0.01) && transform.position.z > -35)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed);
        }
    }
}
