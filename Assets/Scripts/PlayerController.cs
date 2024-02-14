using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    //public float dashLength = 10f;
    public float slideMultiplier = 1.5f;
    public float normalHeight, crouchHeight;
 
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
 
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
 
    public bool canMove = true;
    public bool crouching = false;
    //public bool canDash = true;
    public bool isSliding = false;
    
    private Vector3 playerSlide;
    public float slideDuration = 1.0f;
    private float slideTimer;

    public float speedBoostDuration = 5.0f;
    public float speedBoostTimer;
    public bool isSpeedBoost = false;

    private Vector3 offset; 

    public Player player;
    public bool isMoving;
    
    CharacterController characterController;
    public CapsuleCollider playerCollider;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        offset = new Vector3(0, 0.5f, 0);
        player = GetComponent<Player>();
    }
 
    void Update()
    {
        isMoving = characterController.velocity != Vector3.zero;
        // Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && !crouching && (player.currentHunger != 0);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
  
        // Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
  
        // Rotation
        characterController.Move(moveDirection * Time.deltaTime);
 
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Crouching

        if (Input.GetKeyDown(KeyCode.C))
        {
            crouching = true;
            characterController.height = crouchHeight;
            playerCollider.height = crouchHeight;
            walkSpeed /= 2;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            crouching = false;
            characterController.height = normalHeight;
            playerCollider.height = normalHeight;
            transform.position += offset;
            walkSpeed *= 2;
        }

        //sliding
        if(isRunning) {
            if(crouching) {
                isSliding = true;
                playerSlide = moveDirection.normalized;
                startSlide();
            }
        }
        if(isSliding) {
            updateSlide(playerSlide);
        }

        // speed boost update
        if (isSpeedBoost)
        {
            updateSpeedBoost();
        }

        // Hunger updates
        if (isMoving) {
            if(isRunning){
                player.SprintHunger();
            }
            else {
                player.WalkHunger();
            }
        }
        else {
            player.StillHunger();
        }
    }

    void startSlide() {
        slideTimer = slideDuration;
    }

    void updateSlide(Vector3 slideDir) {
        if (slideTimer > 0) {
            characterController.Move(slideDir * Time.deltaTime * slideMultiplier * runSpeed);
            //Debug.Log(slideTimer);
            slideTimer -= Time.deltaTime;
            isSliding = crouching;
        } else {
            isSliding = false;
        }
    }

    public void startSpeedBoost() {
        if (!isSpeedBoost)
        {
            speedBoostTimer = speedBoostDuration;
            runSpeed*=2;
        }   
        isSpeedBoost = true;
    }

    void updateSpeedBoost() {
        if (speedBoostTimer > 0) {
            speedBoostTimer -= Time.deltaTime;
        } else {
            runSpeed/=2;
            isSpeedBoost = false;
        }
    }
}