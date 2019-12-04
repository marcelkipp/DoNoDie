using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCursor : MonoBehaviour
{

    public bool debug = false;
    public GameObject player;

    void Start()
    {

    }

    void Update()
    {
        rotateFirepointToCursor();
    }

    void rotateFirepointToCursor()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, player.transform.position.y, 0));
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            if(debug) Debug.DrawLine(transform.position, pointToLook, Color.red);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}
