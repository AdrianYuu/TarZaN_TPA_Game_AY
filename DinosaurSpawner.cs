using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DinosaurSpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float dinosaurCount;
    [SerializeField] private float spawnRange;
    [SerializeField] private CreatureFactory cf;

    public void Start()
    {
        for(int i = 0; i < dinosaurCount; i++)
        {
            float randomX = Random.Range(-spawnRange, spawnRange);
            float randomZ = Random.Range(-spawnRange, spawnRange);
            Vector3 dinosaurPosition = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            cf.CreateDino(dinosaurPosition);
        }
    }
}
