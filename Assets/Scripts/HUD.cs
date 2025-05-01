using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    [SerializeField] private GameObject extraInfo;

    [SerializeField] private TextMeshProUGUI shotsLeft;
    [SerializeField] private TextMeshProUGUI targetsLeft;
    [SerializeField] private TextMeshProUGUI elevation;
    [SerializeField] private TextMeshProUGUI horizontal;
    [SerializeField] private TextMeshProUGUI power;
    [SerializeField] private Slider powerSlider;

    [SerializeField] private Canvas pauseMenu;
    [SerializeField] private SceneAsset mainMenu;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pauseMenu.enabled = false;
    }

    public void togglePauseMenu()
    {
        pauseMenu.enabled = !pauseMenu.enabled;
        Time.timeScale = pauseMenu.enabled ? 0.0f : 1.0f;
    }

    public void onMainMenuPressed()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(mainMenu.name);
    }

    public void setShotsLeft(int value)
    {
        shotsLeft.SetText("Shots Left: " + value);
    }

    public void setTargetsLeft(int value)
    {
        targetsLeft.SetText("Targets Left: " + value);
    }

    public void setElevation(int value)
    {
        elevation.SetText("Elevation: " + value + "°");
    }

    public void setHorizontal(int value)
    {
        horizontal.SetText("Horizontal: " + value + "°");
    }

    public void setPower(int value)
    {
        power.SetText("Power: " + value);
        powerSlider.value = value;
    }

    public void infoButtonClicked()
    {
        extraInfo.SetActive(!extraInfo.activeInHierarchy);
    }
}
