using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("References")]
    public GameObject antialiasing;
    public GameObject quality;

    public void Start()
    {
        antialiasing.GetComponent<Dropdown>().value = QualitySettings.antiAliasing / 2;
        quality.GetComponent<Dropdown>().value = QualitySettings.GetQualityLevel();
    }

    public void SetAntialiasing()
    {
        QualitySettings.antiAliasing = antialiasing.GetComponent<Dropdown>().value * 2;
    }

    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(quality.GetComponent<Dropdown>().value);
    }
}
