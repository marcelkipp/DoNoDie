using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFollow : MonoBehaviour
{

    public Transform PlayerTransform;
    public float SmoothFactor;
    public bool RotateAroundPlayer = true;
    public float RotationSpeed;

    private Vector3 _cameraOffset;

    PlayerControlls controls;
    Vector2 move;

    private void Awake()
    {
        controls = new PlayerControlls();
        controls.Gameplay.RotateCamera.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.RotateCamera.canceled += ctx => move = Vector2.zero;
    }

    void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;
    }

    // LateUpdate is called after update Method to make sure that the transform of the objects is completed
    void LateUpdate()
    {

        if (RotateAroundPlayer)
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(move.x * RotationSpeed, new Vector3(0,1,0));
            _cameraOffset = camTurnAngle * _cameraOffset; 
        }


        Vector3 newPos = PlayerTransform.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor); // interpolate the values for smoothness
        transform.LookAt(PlayerTransform); 
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
