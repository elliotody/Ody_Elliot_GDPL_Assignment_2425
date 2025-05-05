using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image logo;
    [SerializeField] private SceneAsset gameScene;

    // Changes the logo if Funky Mode is toggled
    [Header("Funky Mode")]
    [SerializeField] private Sprite notFunky;
    [SerializeField] private Sprite funky;

    // Loads the game scene
    public void playButtonPressed()
    {
        SceneManager.LoadScene(gameScene.name);
    }

    // Toggles funky mode on and off
    public void funkyButtonPressed()
    {
        if (PersistentData.funkyMode)
        {
            PersistentData.funkyMode = false;
            logo.sprite = notFunky;
        }
        else
        {
            PersistentData.funkyMode = true;
            logo.sprite = funky;
        }
    }

    // Quits the game
    public void quitButtonPressed()
    {
        Application.Quit();
    }
}
