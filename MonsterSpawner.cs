using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject monster;

    private int curr;

    private Vector3[] spawnLocation = 
    {
        new Vector3(490f, 0.001f, 990f),
        new Vector3(490f, 0.001f, 10f),
        new Vector3(990f, 0.001f, 490f),
        new Vector3(10f, 0.001f, 490f)
    };

    public void Start()
    {
        curr = 0;
    }

    public void SpawnMonster(int totalEnemy)
    {
        for(int i = 0; i < totalEnemy; i++)
        {
            GameObject newMonster = Instantiate(monster, spawnLocation[curr], Quaternion.identity);
            newMonster.GetComponent<Monster>().spawnDir = curr;

            curr++;
            
            if(curr == 4)
            {
                curr = 0;
            }
        }
    }
}
