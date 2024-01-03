using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private GameObject NPCMenu;
    [SerializeField] private GameObject TDMenu;

    public void EndGame(String condition)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        playerHUD.SetActive(false);
        NPCMenu.SetActive(false);
        TDMenu.SetActive(false);

        if(condition.Equals("Win"))
        {
            winMenu.SetActive(true);
        }
        else
        {
            loseMenu.SetActive(true);
        }
    }

}
