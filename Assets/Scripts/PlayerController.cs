using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float velocity = 5;
    public float sprintVelocity = 10;
    public float turnSpeed = 5;
    public GameObject cam;
    public float mouseSensivity = 3f;
    public float viewRange = 60f;
    
    Animator animator;
    PlayerControlls controls;
    Vector2 move;
    bool sprint;
    float angle;
    Vector3 lastMouseInput;

    private float timeSinceNoInputForMovement = 0.0f;
    private float timeForNoInputAnimation = 15.0f;
    private float timeForNoInputAnimationOffset = 10.0f;

    private float MouseX;
    private float MouseY;

    Quaternion targetRotation;

    private void Awake()
    {
        controls = new PlayerControlls();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        controls.Gameplay.Sprint.performed += ctx => sprint = true;
        controls.Gameplay.Sprint.canceled += ctx => sprint = false;
    }

    private void Start()
    {
        //Cursor ausblenden
        //Cursor.visible = false;

        animator = GetComponent<Animator>();
        lastMouseInput = Vector3.zero;
    }


    private void Update()
    {

       // rotatePlayer();

        rotatePlayerToMousePosition();

        // Animation wenn kein Input nach bestimmter Zeit festgestellt wurde
        if (move.Equals(Vector2.zero))
        {
            if ((timeSinceNoInputForMovement += Time.deltaTime) > timeForNoInputAnimation)
            {
                timeForNoInputAnimation += timeForNoInputAnimationOffset;
                animator.SetTrigger("noInputAnimation1");
            }

            animator.SetBool("walking", false);
            animator.SetBool("sprinting", false);
            animator.SetBool("backwards", false); 
            return;
        }

        timeSinceNoInputForMovement = 0.0f;

        CalculateDirection();
        Rotate();
        Move();

        lastMouseInput = Input.mousePosition;
    }


    void rotatePlayer()
    {
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(0,MouseX * mouseSensivity, 0);
    }


    void CalculateDirection()
    {
        angle = Mathf.Atan2(move.x, move.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.transform.eulerAngles.y;
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void Move()
    {
        //Debug.Log(move); 

        float moveVelocity;

        moveVelocity = velocity;

        if (move.y > 0)
        {
            if (sprint)
            {
                moveVelocity = sprintVelocity;
                animator.SetBool("sprinting", true);
            }
            else
            {
                animator.SetBool("sprinting", false);
                moveVelocity = velocity;
            }

            animator.SetBool("walking", true);
            transform.position += transform.forward * moveVelocity * Time.deltaTime;
        }


        // LINKS
        if (move.x < 0)
        {
            moveVelocity *= 0.5f;
            transform.position += -transform.right * moveVelocity * Time.deltaTime;
        }

        // RECHTS
        if (move.x > 0)
        {
            moveVelocity *= 0.5f;
            transform.position += transform.right * moveVelocity * Time.deltaTime;
        }

        // ZURÜCK
        if (move.y < 0)
        {
            animator.SetBool("backwards", true);
            moveVelocity *= 0.5f;
            transform.position += -transform.forward * moveVelocity * Time.deltaTime;
        }

    }

    void rotatePlayerToMousePosition()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            //Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void AnimationHandler()
    {
        animator.SetBool("walking", true);
    }

}