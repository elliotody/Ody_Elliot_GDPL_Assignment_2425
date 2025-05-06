using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class HUD : MonoBehaviour
{
    // Creates a reference to the singleton
    public static HUD instance;

    [Header("HUD Elements")]
    [SerializeField] private GameObject extraInfo;
    [SerializeField] private GameObject WASDKeys;
    [SerializeField] private TextMeshProUGUI shotsLeft;
    [SerializeField] private TextMeshProUGUI targetsLeft;
    [SerializeField] private TextMeshProUGUI elevation;
    [SerializeField] private TextMeshProUGUI horizontal;
    [SerializeField] private TextMeshProUGUI power;
    [SerializeField] private Slider powerSlider;

    [Header("Menus")]
    [SerializeField] private Canvas pauseMenu;
    [SerializeField] public SceneAsset mainMenu;

    // Sets up the singleton 
    private void Awake()
    {
        instance = this;
    }

    // Disables the pause menu
    private void Start()
    {
        pauseMenu.enabled = false;
    }

    // Toggles the pause menu on and off
    public void togglePauseMenu()
    {
        pauseMenu.enabled = !pauseMenu.enabled;
        // Changes the timescale depending on whether the pause menu is active
        Time.timeScale = pauseMenu.enabled ? 0.0f : 1.0f;
    }

    // Resets the timescale and changes the scene to the main menu
    public void onMainMenuPressed()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(mainMenu.name);
    }

    // Sets the text value for shots left
    public void setShotsLeft(int value)
    {
        shotsLeft.SetText("Shots Left: " + value);
    }

    // Sets the text value for targets left
    public void setTargetsLeft(int value)
    {
        targetsLeft.SetText("Targets Left: " + value);
    }

    // Sets the text value for elevation left
    public void setElevation(int value)
    {
        elevation.SetText("Elevation: " + value + "°");
    }

    // Sets the text value for horizontal left
    public void setHorizontal(int value)
    {
        horizontal.SetText("Horizontal: " + value + "°");
    }

    // Sets the text value for power
    public void setPower(int value)
    {
        power.SetText("Power: " + value);
        powerSlider.value = value;
    }

    // Sets the text value for shots left
    public void infoButtonClicked()
    {
        extraInfo.SetActive(!extraInfo.activeInHierarchy);
    }

    // Hides the WASD Keys
    public void hideWASD()
    {
        WASDKeys.SetActive(false);
    }
}
