using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject tower;
    [SerializeField] private GameObject tc;
    [SerializeField] private Slider towerHPSlider;
    [SerializeField] private Slider enemyCountSlider;
    [SerializeField] private TextMeshProUGUI enemyCounter;

    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        towerHPSlider.value = tower.GetComponent<Tower>().towerHP;
        enemyCountSlider.value = tc.GetComponent<TDController>().totalEnemy;
        enemyCounter.text = tc.GetComponent<TDController>().totalEnemy + " / " + tc.GetComponent<TDController>().wave + " remaining";
    }   

}
