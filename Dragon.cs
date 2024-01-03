using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Slider HPSlider;
    [SerializeField] private GameObject HPSliderBackground;

    [Header("Attributes")]
    [SerializeField] private float dragonBaseHP;
    [SerializeField] private float dragonAttack;

    [Header("Idling")]
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;

    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;

    [Header("Range")]
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    private bool playerInSightRange;
    private bool playerInAttackRange;
    private bool enemyInAttackRange;

    private float dragonHP;
    private NavMeshAgent agent;
    private GameObject player;
    private Animator animator;
    private RaycastHit hit;
    private GameObject currEnemyObj;

    private EndGameMenu egm;
    
    public void Awake()
    {
        player = GameObject.Find("PlayerContainer");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        spawnPoint = transform.position;
        walkPointSet = false;

        dragonHP = dragonBaseHP;
        egm = GameObject.Find("EndGameController").GetComponent<EndGameMenu>();
    }

    public void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if(!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        
        if(playerInSightRange && !playerInAttackRange)
        {
            ChasingPlayer();
        }

        if(playerInSightRange && playerInAttackRange)
        {
            Debug.Log("test");
            AttackPlayer();
        }
        
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        HPSlider.value = dragonHP;
    }

    public void FollowPlayer()
    {
        agent.SetDestination(player.transform.position);
        animator.SetBool("isWalking", true);
    }

    public void Patroling()
    {
        if(!walkPointSet)
        {
            SearchWalkPoint();
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
            animator.SetBool("isWalking", true);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(spawnPoint.x + randomX, spawnPoint.y, spawnPoint.z + randomZ);
        walkPointSet = true;
    }

    public void ChasingPlayer()
    {
        agent.SetDestination(player.transform.position);
        animator.SetBool("isWalking", true);
    }

    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player.transform);
        
        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            player.GetComponent<Player>().TakeDamage(dragonAttack);
            animator.SetBool("isAttacking", true);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        dragonHP -= damage;
        
        if(dragonHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        egm.EndGame("Win");
    }

}
