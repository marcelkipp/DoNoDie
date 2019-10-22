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
    
    
    Animator animator;
    PlayerControlls controls;
    Vector2 move;
    Vector2 input;
    bool sprint;
    float angle;

    private float timeSinceNoInputForMovement = 0.0f;
    private float timeForNoInputAnimation = 5.0f;
    private float timeForNoInputAnimationOffset = 10.0f;
    private float timeForNoInputAnimationDuration = 3.0f;

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
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Debug.Log(timeSinceNoInputForMovement);

        if (move.Equals(Vector2.zero))
        {

            //Debug.Log(timeSinceNoInputForMovement);

            if ((timeSinceNoInputForMovement += Time.deltaTime) > timeForNoInputAnimation)
            {
                timeForNoInputAnimation += timeForNoInputAnimationOffset;
                animator.SetTrigger("noInputAnimation1");
            }

            animator.SetBool("walking", false);
            animator.SetBool("sprinting", false);
            return;
        }

        timeSinceNoInputForMovement = 0.0f;

        AnimationHandler();
        CalculateDirection();
        Rotate();
        Move();
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
        float moveVelocity;

        if (sprint)
        {
            moveVelocity = sprintVelocity;
            animator.SetBool("sprinting", true);
        } else
        {
            animator.SetBool("sprinting", false);
            moveVelocity = velocity;
        }

        transform.position += transform.forward * moveVelocity * Time.deltaTime;
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