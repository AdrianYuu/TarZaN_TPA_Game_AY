using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform gunTip;
    [SerializeField] private LayerMask grappleable;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private SoundController sc;

    [Header("Grappling")]
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float grappleDelayTime;
    [SerializeField] private float grapplingCd;
    private float grapplingCdTimer;
    [SerializeField] private float overshootYAxis;

    [Header("Keybinds")]
    [SerializeField] private KeyCode grappleKey = KeyCode.Mouse1;

    private PlayerMovement pm;
    private Vector3 grapplePoint;
    private RaycastHit hit;
    private bool isGrappling;
    
    public void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        if(Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
            sc.GrapplingSound();
        }

        if(grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }
    }

    public void LateUpdate()
    {
        if(isGrappling)
        {
            lr.SetPosition(0, gunTip.position);
        }
    }

    public void StartGrapple()
    {
        if(grapplingCdTimer > 0)
        {
            return;
        } 

        isGrappling = true;

        pm.isFreeze = true;

        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, grappleable))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    public void ExecuteGrapple()
    {
        pm.isFreeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if(grapplePointRelativeYPos < 0)
        {
            highestPointOnArc = overshootYAxis;
        }

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pm.isFreeze = false;
        isGrappling = false;
        grapplingCdTimer = grapplingCd;
        lr.enabled = false;
    }

}
