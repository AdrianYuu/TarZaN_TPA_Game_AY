using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject TDMenu;
    [SerializeField] private Slider enemyCountSlider;
    [SerializeField] private SoundController sc;

    private MonsterSpawner ms;

    public int wave = 1;
    public int totalEnemy;

    public static bool isRunning = false;

    public void Awake()
    {
        ms = GameObject.Find("MonsterSpawner").GetComponent<MonsterSpawner>();
    }

    public void Start()
    {
        
    }

    public void Update()
    {
        CheckGame();
    }
    
    public void CheckGame()
    {
        if(isRunning)
        {
            if(totalEnemy == 0)
            {
                EndGame();
            }
        }
    }

    public void StartGame()
    {
        isRunning = true;

        totalEnemy = wave;
        Debug.Log(totalEnemy);
        enemyCountSlider.maxValue = totalEnemy;

        TDMenu.SetActive(true);
        ms.SpawnMonster(totalEnemy);
    }

    public void EndGame()
    {
        sc.SuccessSound();
        TDMenu.SetActive(false);
        isRunning = false;
        wave++;
    }

}
