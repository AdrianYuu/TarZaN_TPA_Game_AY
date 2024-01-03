using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private SoundController sc;

    [Header("Attributes")]
    [SerializeField] private float punchCooldown = 1f;
    [SerializeField] private float attackRange = 2f;

    [Header("Keybinds")]
    [SerializeField] private KeyCode punchKey = KeyCode.Mouse0;

    private Animator animator;
    private RaycastHit hit;
    public GameObject currEnemyObj;
    private float playerAttack;
    private bool canPunch;
    private bool isRightHand;

    public void Start()
    {
        playerAttack = playerContainer.GetComponent<Player>().playerAttack;
        animator = player.GetComponent<Animator>();
        canPunch = true;
        isRightHand = true;
    }

    public void Update()
    {
        if(TDController.isRunning)
        {   
            currEnemyObj = GameObject.FindWithTag("Monster");
        }

        if(Input.GetKeyDown(punchKey) && canPunch)
        {
            Punch();
        }
    }

    public void DealDamage()
    {
        // Kalau dia bear
        if(currEnemyObj.name.ContainsInsensitive("bear") && !currEnemyObj.GetComponent<Bear>().isPet)
        {
            currEnemyObj.GetComponent<Bear>().TakeDamage(playerAttack);
            if(!currEnemyObj.GetComponent<Bear>().isAlive)
            {
                currEnemyObj = null;
            }
        }
        else if(currEnemyObj.name.ContainsInsensitive("monster"))
        {
            Debug.Log("Masuk");
            currEnemyObj.GetComponent<Monster>().TakeDamage(playerAttack);
        }
        else if(currEnemyObj.name.ContainsInsensitive("dino") && !currEnemyObj.GetComponent<Dinosaur>().isPet)
        {
            currEnemyObj.GetComponent<Dinosaur>().TakeDamage(playerAttack);
            if(!currEnemyObj.GetComponent<Dinosaur>().isAlive)
            {
                currEnemyObj = null;
            }
        }
        else if(currEnemyObj.name.ContainsInsensitive("dragon"))
        {
            currEnemyObj.GetComponent<Dragon>().TakeDamage(playerAttack);
        }
    }

    public void Punch()
    {
        sc.PunchSound();

        DisablePunchAgain();
        
        if(Physics.Raycast(transform.position, transform.forward, out hit, attackRange, enemyLayer))
        {
            currEnemyObj = hit.collider.gameObject;
            DealDamage();
        }

        if(isRightHand)
        {
            animator.SetBool("isPunchingRight", true);
        }
        else
        {
            animator.SetBool("isPunchingLeft", true);
        }

        Invoke(nameof(ResetPunch), punchCooldown);
    }

    public void DisablePunchAgain()
    {
        canPunch = false;
    }

    public void ResetPunch()
    {
        if(isRightHand)
        {
            animator.SetBool("isPunchingRight", false);
            isRightHand = false;
        }
        else
        {
            animator.SetBool("isPunchingLeft", false);
            isRightHand = true;
        }

        canPunch = true;
    }
}
