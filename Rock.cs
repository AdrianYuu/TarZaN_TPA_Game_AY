using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float rockDamage;

    private GameObject player;

    public void Start()
    {
        player = GameObject.Find("PlayerContainer");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Invoke(nameof(DestroyStone), 3);
        }

        // Kalau dia hit bear
        if(collision.gameObject.name.ContainsInsensitive("bear"))
        {
            if(!collision.gameObject.GetComponent<Bear>().isPet)
            {
                collision.gameObject.GetComponent<Bear>().TakeDamage(rockDamage);
                player.GetComponent<PlayerAttack>().currEnemyObj = collision.gameObject;
                if(!collision.gameObject.GetComponent<Bear>().isAlive)
                {
                    player.GetComponent<PlayerAttack>().currEnemyObj = null;
                }
            }
            Invoke(nameof(DestroyStone), 0);
        }
        else if(collision.gameObject.name.ContainsInsensitive("dino"))
        {
            if(!collision.gameObject.GetComponent<Dinosaur>().isPet)
            {
                collision.gameObject.GetComponent<Dinosaur>().TakeDamage(rockDamage);
                player.GetComponent<PlayerAttack>().currEnemyObj = collision.gameObject;
                if(!collision.gameObject.GetComponent<Dinosaur>().isAlive)
                {
                    player.GetComponent<PlayerAttack>().currEnemyObj = null;
                }
            }
            Invoke(nameof(DestroyStone), 0);
        }
        else if(collision.gameObject.name.ContainsInsensitive("monster"))
        {
            collision.gameObject.GetComponent<Monster>().TakeDamage(rockDamage);
            player.GetComponent<PlayerAttack>().currEnemyObj = collision.gameObject;
            if(!collision.gameObject.GetComponent<Monster>().isAlive)
            {
                player.GetComponent<PlayerAttack>().currEnemyObj = null;
            }
            Invoke(nameof(DestroyStone), 0);
        }
        else if(collision.gameObject.name.ContainsInsensitive("dragon"))
        {
            collision.gameObject.GetComponent<Dragon>().TakeDamage(rockDamage);
            player.GetComponent<PlayerAttack>().currEnemyObj = collision.gameObject;
            Invoke(nameof(DestroyStone), 0);
        }
    }

    public void DestroyStone()
    {
        Destroy(gameObject);
    }
}
