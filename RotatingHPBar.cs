using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingHPBar : MonoBehaviour
{
    private Transform player;

    public void Start()
    {
        player = GameObject.Find("PlayerContainer").GetComponent<Transform>();
    }

    public void Update()
    {
        transform.LookAt(new Vector3(player.position.x, player.position.y + 2f, player.position.z));
    }
}
