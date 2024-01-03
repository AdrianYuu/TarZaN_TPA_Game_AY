using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject throwableRock;

    [Header("Throwing")]
    [SerializeField] private float throwForce;
    [SerializeField] private float throwUpwardForce;
    [SerializeField] private float throwCooldown;

    [Header("Keybinds")]
    [SerializeField] private KeyCode throwKey = KeyCode.X;

    private bool canThrow;

    public void Start()
    {
        canThrow = true;
    }

    public void Update()
    {
        if(Input.GetKeyDown(throwKey) && canThrow)
        {
            Throw();
        }
    }

    public void Throw()
    {
        canThrow = false;
        Vector3 initialPosition = attackPoint.position + cam.transform.forward * 0.5f;
        GameObject rock = Instantiate(throwableRock, initialPosition, cam.rotation);
        Rigidbody rockRb = rock.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        Vector3 force = forceDirection * throwForce + transform.up * throwUpwardForce;
        rockRb.AddForce(force, ForceMode.Impulse);

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    public void ResetThrow()
    {
        canThrow = true;
    }
}
