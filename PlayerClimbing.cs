using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask treeLayer;

    [Header("Climbing")]
    [SerializeField] private float climbSpeed;

    [Header("ClimbingJump")]
    [SerializeField] private float climbJumpUpForce;
    [SerializeField] private float climbJumpBackForce;

    [Header("Detection")]
    [SerializeField] private float detectionLength;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float maxLookAngle;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    private Rigidbody rigidBody;
    private RaycastHit treeFrontHit;
    private PlayerMovement pm;
    private bool isClimbing;
    private bool isTreeFront;
    private float lookAngle;

    public void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        TreeCheck();
        Climb();
    }

    public void Climb()
    {
        if(isTreeFront && Input.GetKey(KeyCode.W) && lookAngle < maxLookAngle)
        {
            if(!isClimbing)
            {
                StartClimbing();
            }
        }
        else
        {
            if(isClimbing)
            {
                StopClimbing();
            }
        }

        if(isClimbing)
        {
            ClimbingMovement();
        }

        if(isTreeFront && Input.GetKeyDown(jumpKey))
        {
            ClimbJump();
        }
    }

    public void TreeCheck()
    {
        isTreeFront = Physics.SphereCast(transform.position, sphereCastRadius, player.forward, out treeFrontHit, detectionLength, treeLayer);
        lookAngle = Vector3.Angle(player.forward, -treeFrontHit.normal);
    }

    public void StartClimbing()
    {
        isClimbing = true;
        pm.isClimbing = true;
    }

    public void ClimbingMovement()
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, climbSpeed, rigidBody.velocity.z);

        // Taro Suara
    }

    public void StopClimbing()
    {
        isClimbing = false;
        pm.isClimbing = false;
    }

    public void ClimbJump()
    {
        Vector3 applyForce = transform.up * climbJumpUpForce + treeFrontHit.normal * climbJumpBackForce;
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
        rigidBody.AddForce(applyForce, ForceMode.Impulse);
    }
}
