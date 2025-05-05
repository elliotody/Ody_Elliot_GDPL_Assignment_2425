using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

// Handles win/lose conditions and transitions back to the main menu
public class WinLose : MonoBehaviour
{
    public static WinLose instance; // Singleton instance for easy access

    [SerializeField] private TextMeshProUGUI winScreen; // UI element for the win screen
    [SerializeField] private TextMeshProUGUI loseScreen; // UI element for the lose screen

    private void Awake()
    {
        instance = this; // Assigns the instance for global access
    }

    private void Start()
    {
        gameObject.SetActive(false); // Hides the win/lose UI at the start
    }

    // Triggers the win scenario
    public void win()
    {
        HUD.instance.gameObject.SetActive(false); // Hides the HUD
        gameObject.SetActive(true); // Displays win/lose UI
        loseScreen.gameObject.SetActive(false); // Ensures lose screen is hidden
        returnToMainMenu(3000); // Initiates return to main menu after 3 seconds
    }

    // Triggers the lose scenario
    public void lose()
    {
        HUD.instance.gameObject.SetActive(false); // Hides the HUD
        gameObject.SetActive(true); // Displays win/lose UI
        winScreen.gameObject.SetActive(false); // Ensures win screen is hidden
        returnToMainMenu(3000); // Initiates return to main menu after 3 seconds
    }

    // Returns to the main menu after a set delay
    async void returnToMainMenu(int time)
    {
        await Task.Delay(time); // Waits for the specified time before proceeding
        SceneManager.LoadScene(HUD.instance.mainMenu.name); // Loads the main menu scene
    }
}
