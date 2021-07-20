using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Required References")]
    public Shooter playerShooter;
    public Health Playerhealth;

    public List<GameObject> disableWhileDead;

    [Header("Settings")]
    public float movespeed = 2;
    public float lookspeed = 60;
    public float gravity = 9.8f;
    public float jumpPower = 12;

    [Header("Jump Timing")]
    public float JumotimeLeniency = 0.1f;
    private float TimeToStopLenient = 0;

    private bool doublejumpavailable = false;
    
    private Vector3 moveDirection;

    private CharacterController controller;
    private InputManager inputManager;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.instance;

        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Playerhealth.currentHealth <= 0)
        {
            foreach (GameObject ingameobject in disableWhileDead)
            {
                ingameobject.SetActive(false);
            }
            return;
        }
        else
        {
            foreach (GameObject ingameobject in disableWhileDead)
            {
                ingameobject.SetActive(true);
            }
        }

        ProcessMovement();
        ProcessRotation();
    }

    void ProcessMovement()
    {
        float LeftRightInput = inputManager.horizontalMoveAxis;
        float BackwardForwardInput = inputManager.verticalMoveAxis;
        bool jumpPressed = inputManager.jumpPressed;


        if (controller.isGrounded)
        {
            TimeToStopLenient = Time.time + JumotimeLeniency;
            moveDirection = new Vector3(LeftRightInput, 0, BackwardForwardInput);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * movespeed;

            doublejumpavailable = true;

            if (jumpPressed)
            {
                moveDirection.y = jumpPower;
            }
        }
        else
        {
            moveDirection = new Vector3(LeftRightInput * movespeed, moveDirection.y, BackwardForwardInput * movespeed);
            moveDirection = transform.TransformDirection(moveDirection);

            if(jumpPressed && Time.time < TimeToStopLenient)
            {
                moveDirection.y = jumpPower;
            }
            else if(jumpPressed && doublejumpavailable)
            {
                moveDirection.y = jumpPower;
                doublejumpavailable = false;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        if(controller.isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = -0.3f;
        }

        controller.Move(moveDirection * Time.deltaTime);
    }

    void ProcessRotation()
    {
        float horizontallookinput = inputManager.horizontalLookAxis;
        Vector3 playerRotation = transform.rotation.eulerAngles;
        Vector3 p = new Vector3(playerRotation.x, playerRotation.y + horizontallookinput * lookspeed * Time.deltaTime, playerRotation.z);
        //transform.rotation = Quaternion.Euler(new Vector3(playerRotation.x, playerRotation.y + horizontallookinput * lookspeed * Time.deltaTime, playerRotation.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(p), lookspeed );
    }
}
