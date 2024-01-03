using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button mainMenuButton;

    public void Start()
    {
        mainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    public void Update()
    {
        
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
