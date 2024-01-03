using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Dinosaur : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Slider HPSlider;
    [SerializeField] private GameObject HPSliderBackground;

    [Header("Attributes")]
    [SerializeField] private float dinoBaseHP;
    [SerializeField] private float dinoAttack;

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

    private float dinoHP;
    private NavMeshAgent agent;
    private GameObject player;
    private Animator animator;
    private RaycastHit hit;
    private GameObject currEnemyObj;
    public bool isPet;
    public bool isAlive;
    
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

        dinoHP = dinoBaseHP;
        
        isAlive = true;
        isPet = false;
    }

    public void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if(!isPet)
        {
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
                AttackPlayer();
            }
        }
        else
        {
            attackRange = 2f;
            currEnemyObj = player.GetComponent<PlayerAttack>().currEnemyObj;
            enemyInAttackRange = Physics.Raycast(transform.position, transform.forward, out hit, attackRange);
            
            if(!currEnemyObj)
            {
                FollowPlayer();
            }

            if(currEnemyObj && !enemyInAttackRange)
            {
                ChasingEnemy();
            }
            
            if(currEnemyObj && enemyInAttackRange)
            {
                AttackEnemy();
            }
        }

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        HPSlider.value = dinoHP;
    }

    public void FollowPlayer()
    {
        agent.SetDestination(player.transform.position);
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
        animator.SetBool("isAttacking", false);
    }

    public void ChasingEnemy()
    {
        agent.SetDestination(currEnemyObj.transform.position);
        animator.SetBool("isAttacking", false);
    }

    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player.transform);
        
        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            player.GetComponent<Player>().TakeDamage(dinoAttack);
            animator.SetBool("isAttacking", true);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void AttackEnemy()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(currEnemyObj.transform);

        if(!alreadyAttacked)
        {
            alreadyAttacked = true;

             // Kalau dia bear
            if(currEnemyObj.name.ContainsInsensitive("bear") && !currEnemyObj.GetComponent<Bear>().isPet)
            {
                currEnemyObj.GetComponent<Bear>().TakeDamage(dinoAttack);
                if(!currEnemyObj.GetComponent<Bear>().isAlive)
                {
                    player.GetComponent<PlayerAttack>().currEnemyObj = null;
                }
            }
            else if(currEnemyObj.name.ContainsInsensitive("dino") && !currEnemyObj.GetComponent<Dinosaur>().isPet)
            {
                currEnemyObj.GetComponent<Dinosaur>().TakeDamage(dinoAttack);
                if(!currEnemyObj.GetComponent<Dinosaur>().isAlive)
                {
                    player.GetComponent<PlayerAttack>().currEnemyObj = null;
                }
            }
            else if(currEnemyObj.name.ContainsInsensitive("monster"))
            {
                attackRange = 6f;
                currEnemyObj.GetComponent<Monster>().TakeDamage(dinoAttack);
                if(!currEnemyObj.GetComponent<Monster>().isAlive)
                {
                    player.GetComponent<PlayerAttack>().currEnemyObj = null;
                }
            }
            else if(currEnemyObj.name.ContainsInsensitive("dragon"))
            {
                currEnemyObj.GetComponent<Dragon>().TakeDamage(dinoAttack);
            }

            animator.SetBool("isAttacking", true);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;
        animator.SetBool("isAttacking", false);
    }

    public void TakeDamage(float damage)
    {
        dinoHP -= damage;
        
        if(dinoHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isAlive = false;
        BecomePet();
    }

    public void BecomePet()
    {
        isPet = true;
        dinoHP = dinoBaseHP;

        // HPSliderBackground.GetComponent<Image>().color = Color.blue;

        player.GetComponent<Player>().AddPet();
        player.GetComponent<Player>().AddExp(dinoBaseHP);

        agent.SetDestination(player.transform.position);
        animator.SetBool("isAttacking", false);
    }
}
