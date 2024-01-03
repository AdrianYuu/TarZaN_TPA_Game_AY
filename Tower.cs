using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider HPSlider;
    [SerializeField] public float towerbaseHP;
    
    private EndGameMenu egm;

    public float towerHP; 

    public void Start()
    {
        towerHP = towerbaseHP;
        egm = GameObject.Find("EndGameController").GetComponent<EndGameMenu>();
    }

    public void Update()
    {
        HPSlider.value = towerHP;
    }

    public void TakeDamage(float damage)
    {
        towerHP -= damage;

        if(towerHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        egm.EndGame("Lose");
    }
}
