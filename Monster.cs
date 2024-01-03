using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider HPSlider;

    private Animator animator;

    private float monsterBaseHP = 200;
    private float monsterHP;

    private GeneratePath gp;
    private Tower tower;
    private GameObject player;
    private NavMeshAgent agent;
    private TDController tc;
    private List<Vector3> pathList;

    public int spawnDir;
    private int idx;
    private bool isDone;
    private int pathCount;
    public bool isAlive;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("PlayerContainer");
        tower = GameObject.Find("Tower").GetComponent<Tower>();
        gp = GameObject.Find("TerrainContainer").GetComponent<GeneratePath>();
        tc = GameObject.Find("TDController").GetComponent<TDController>();
    }

    public void Start()
    {
        idx = 0;
        GetPath();
        isAlive = true;
        monsterHP = monsterBaseHP;

        agent.speed += tc.wave * 1;
        monsterBaseHP += tc.wave * 10;
    }

    public void Update()
    {
        Move();
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        HPSlider.value = monsterHP;
    }

    public void GetPath()
    {
        if(spawnDir == 0)
        {
            pathList = gp.optimumNorthPaths;
        }
        else if(spawnDir == 1)
        {
            pathList = gp.optimumSouthPaths;
        }
        else if(spawnDir == 2)
        {
            pathList = gp.optimumEastPaths;
        }
        else if(spawnDir == 3)
        {
            pathList = gp.optimumWestPaths;
        }

        pathCount = pathList.Count;
    }

    public void Move()
    {
        // Kali 10 karena skala
        animator.SetBool("isRunning", true);
        Vector3 moveTo = new Vector3(pathList[idx].x * 10, transform.position.y, pathList[idx].z * 10); 
        agent.SetDestination(moveTo);

        // Kalau  lah sampai di pathnya ubah tujuan menjadi next
        isDone = transform.position.x == pathList[idx].x * 10 && transform.position.z == pathList[idx].z * 10; 

        if(isDone)
        {
            idx++;
        }

        if(idx == pathCount)
        {
            Destroy(gameObject);
            tc.GetComponent<TDController>().totalEnemy--;
            tower.TakeDamage(1);
        }

    }

    public void TakeDamage(float damage)
    {
        monsterHP -= damage;

        if(monsterHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isAlive = false;
        tc.GetComponent<TDController>().totalEnemy--;
        player.GetComponent<Player>().AddExp(monsterBaseHP);
        Destroy(gameObject);
    }

}
