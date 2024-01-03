using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearSpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float bearCount;
    [SerializeField] private float spawnRange;
    [SerializeField] private CreatureFactory cf;

    public void Start()
    {
        for(int i = 0; i < bearCount; i++)
        {
            float randomX = Random.Range(-spawnRange, spawnRange);
            float randomZ = Random.Range(-spawnRange, spawnRange);
            Vector3 bearPosition = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            cf.CreateBear(bearPosition);
        }
    }
    
}
