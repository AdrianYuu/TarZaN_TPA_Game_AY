using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float dragonCount;
    [SerializeField] private CreatureFactory cf;

    public void Start()
    {
        for(int i = 0; i < dragonCount; i++)
        {
            Vector3 dragonPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            cf.CreateDragon(dragonPosition);
        }
    }
}
