using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowChat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject NPCMenu;
    [SerializeField] private TextMeshProUGUI dialog;

    private bool isTrigger = false;

    public void Start()
    {
        
    }

    public void Update()
    {
        if(isTrigger)
        {
            if(Input.GetKey(KeyCode.F))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                dialog.text = "Do you want to help the village to gain some experience point? [Yes / No]";
            }
        }
    }   

    public void OnTriggerEnter(Collider other) 
    {
        NPCMenu.SetActive(true);
        isTrigger = true;
    }

    public void OnTriggerExit(Collider other) 
    {
        NPCMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isTrigger = false;
        dialog.text = "Press F to interact";
    }
}
