using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureFactory : MonoBehaviour
{
    [SerializeField] private GameObject bear;
    [SerializeField] private GameObject dino;
    [SerializeField] private GameObject dragon;

    public GameObject CreateBear(Vector3 position)
    {
        return Instantiate(bear, position, Quaternion.identity);
    }

    public GameObject CreateDino(Vector3 position)
    {
        return Instantiate(dino, position, Quaternion.identity);
    }

    public GameObject CreateDragon(Vector3 position)
    {
        return Instantiate(dragon, position, Quaternion.identity);
    }
}
