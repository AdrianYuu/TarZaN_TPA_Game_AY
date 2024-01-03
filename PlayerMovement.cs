using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask treeLayer;
    [SerializeField] private SoundController sc;

    [Header("Speed")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float squatSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float climbSpeed;

    [Header("Movement")]
    [SerializeField] private float groundDrag;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;

    [Header("Squating")]
    [SerializeField] private float squatYScale;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode squatKey = KeyCode.LeftControl;

    private Animator animator;
    private Vector3 moveDirection;
    private Rigidbody rigidBody;
    private float horizontalInput;
    private float moveSpeed;
    private float verticalInput;
    private float originalYScale;
    private bool canJump;
    private bool isGrounded;
    public bool isFreeze;
    public bool isClimbing;
    private bool isSquating;
    private bool activeGrapple;
    private Vector3 velocityToSet;
    private bool enableMovementOnNextTouch;

    public void Start()
    {
        animator = player.GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        canJump = true;
        rigidBody.freezeRotation = true;

        originalYScale = transform.localScale.y;
    }

    public void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.2f, groundLayer);

        GetInput();
        SpeedControl();

        if(isGrounded && !activeGrapple)
        {
            rigidBody.drag = groundDrag;
            animator.SetBool("isFalling", false);
        }
        else
        {
            rigidBody.drag = 0;
            animator.SetBool("isFalling", true);
        }

    }

    public void FixedUpdate()
    {
        MovePlayer();
    }

    public void SetSpeed()
    {
        if(isClimbing)
        {
            moveSpeed = climbSpeed;
        }
        else if(isFreeze)
        {
            moveSpeed = 0;
            rigidBody.velocity = Vector3.zero;
        }
        else if(Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = sprintSpeed;
        }
        else if(isGrounded && !isSquating)
        {
            moveSpeed = walkSpeed;
        }
        else if(isSquating)
        {
            moveSpeed = squatSpeed;
        }
    }

    public void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        SetSpeed();

        if(verticalInput == 1 && !isClimbing && isGrounded && !isSquating)
        {
            animator.SetBool("isForward", true);
        } 
        else
        {
            animator.SetBool("isForward", false);
        }

        if(verticalInput == -1 && !isClimbing && isGrounded && !isSquating)
        {
            animator.SetBool("isBackward", true);
        } 
        else
        {
            animator.SetBool("isBackward", false);
        }

        if(horizontalInput == -1 && !isClimbing && isGrounded && !isSquating)
        {
            animator.SetBool("isLeft", true);
        } 
        else
        {
            animator.SetBool("isLeft", false);
        }

        if(horizontalInput == 1 && !isClimbing && isGrounded && !isSquating)
        {
            animator.SetBool("isRight", true);
        } 
        else
        {
            animator.SetBool("isRight", false);
        }

        if(Input.GetKey(jumpKey) && canJump && isGrounded)
        {
            Jump();
            sc.GaspSound();
        }

        if(Input.GetKey(squatKey) && !isSquating)
        {
            Squat();
        }
        else if(!Input.GetKey(squatKey) && isSquating)
        {
            UnSquat();
        }
    }

    public void MovePlayer()
    {
        if(activeGrapple || isSquating) return;

        // Move to direction to what player look look
        moveDirection = transform.forward * verticalInput * Time.deltaTime + transform.right * horizontalInput * Time.deltaTime;
        moveDirection.Normalize();

        if(moveDirection.magnitude > 0.01f)
        {
            sc.WalkingSound();
        }

        if(isGrounded)
        {
            rigidBody.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        } 
        else if(!isGrounded)
        {
            rigidBody.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        
    }

    public void SpeedControl()
    {
        if(activeGrapple) return;

        Vector3 flatVelocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        if(flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rigidBody.velocity = new Vector3(limitedVelocity.x, rigidBody.velocity.y, limitedVelocity.z);
        }
    }

    public void Squat()
    {
        transform.localScale = new Vector3(transform.localScale.x, squatYScale, transform.localScale.z);
        rigidBody.AddForce(Vector3.down * 7f, ForceMode.Impulse);
        isSquating = true;
    }

    public void UnSquat()
    {
        transform.localScale = new Vector3(transform.localScale.x, originalYScale, transform.localScale.z);
        isSquating = false;
    }

    public void Jump()
    {
        DisableJumpAgain();
        animator.SetBool("isJumping", true);
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
        rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    public void DisableJumpAgain()
    {
        canJump = false;
    }

    public void ResetJump()
    {
        canJump = true;
        animator.SetBool("isJumping", false);
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;

        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
        Invoke(nameof(ResetRestrictions), 3f);
    }

    public void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rigidBody.velocity = velocityToSet;
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    public void OnCollisionEnter(Collision other) 
    {
        if(enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<PlayerGrappling>().StopGrapple();
        }
    }
    
}
