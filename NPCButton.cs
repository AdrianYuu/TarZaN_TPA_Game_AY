using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button buttonYes;
    [SerializeField] private Button buttonNo;
    [SerializeField] private GameObject NPCmenu;
    [SerializeField] private GameObject tc;

    // Start is called before the first frame update
    public void Start()
    {
        buttonYes.onClick.AddListener(StartTowerDefense);
        buttonNo.onClick.AddListener(HideMenu);
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public void HideMenu()
    {
        NPCmenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartTowerDefense()
    {
        tc.GetComponent<TDController>().StartGame();
    }
}
