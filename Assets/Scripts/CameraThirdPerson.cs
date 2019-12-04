using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThirdPerson : MonoBehaviour
{
    public GameObject target;
    public GameObject aimtarget;
    public Vector3 offset;

    private Vector3 _cameraOffset;
    private float mouseSensivity;

    public float minY = -10f;
    public float maxY = 30f;

    void Start()
    {

        Cursor.visible = false;

        _cameraOffset = transform.position - target.transform.position;
        mouseSensivity = target.GetComponent<PlayerController>().mouseSensivity;
    }

    private void LateUpdate()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseX = Input.GetAxis("Mouse X");

        Quaternion camTurnAngle = Quaternion.AngleAxis(mouseY * mouseSensivity, new Vector3(1, 0, 0));
        Quaternion camTurnAngleX = Quaternion.AngleAxis(mouseX * mouseSensivity, new Vector3(0, 1, 0));

        
        _cameraOffset = camTurnAngle * camTurnAngleX * _cameraOffset;

        Vector3 newPos = target.transform.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, 50); // interpolate the values for smoothness

        Vector3 t = target.transform.position;
        transform.LookAt(aimtarget.transform);
    }
}
