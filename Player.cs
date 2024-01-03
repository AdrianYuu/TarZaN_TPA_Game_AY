using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject experienceBar;
    [SerializeField] private GameObject level;
    [SerializeField] private GameObject petCount;
    [SerializeField] private SoundController sc;
    
    private EndGameMenu egm;

    [Header("Attributes")]
    [SerializeField] private float playerBaseHP;

    private float playerHP;
    private float playerExp;
    public float playerAttack;
    private float playerDef;
    private float playerLvl;
    private float playerPet;

    private void Awake()
    {
        playerHP = playerBaseHP;
        playerDef = 1;
        playerAttack = 10;
        playerExp = 0;
        playerLvl = 1;  
        playerPet = 0;
    }

    private void Update()
    {
        healthBar.GetComponent<Image>().fillAmount = playerHP / 100f;
        experienceBar.GetComponent<Image>().fillAmount = playerExp / 1000f;
        level.GetComponent<Text>().text = playerLvl.ToString();
        petCount.GetComponent<TextMeshProUGUI>().text = "You have " + playerPet + " pets.";
        egm = GameObject.Find("EndGameController").GetComponent<EndGameMenu>();
    }

    public void TakeDamage(float damage)
    {
        playerHP -= (damage - (playerDef / 2));
        sc.HurtSound();

        if(playerHP <= 0)
        {
            sc.DeadSound();
            Die();
        }
    }

    public void Die()
    {
        egm.EndGame("Lose");
    }

    public void Heal()
    {
        playerHP = playerBaseHP;
    }

    public void AddPet()
    {
        playerPet++;
    }

    public void AddExp(float exp)
    {
        playerExp += exp;
        CheckExp();
    }

    public void CheckExp()
    {
        if(playerExp >= 1000)
        {
            playerExp = 0;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Heal();
        playerLvl++;
        playerBaseHP += 10;
        playerAttack += 2;
        playerDef += (float)0.5;
    }
}
