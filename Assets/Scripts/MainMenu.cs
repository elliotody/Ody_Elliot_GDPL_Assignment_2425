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

    [Header("Funky Mode")]
    [SerializeField] private Sprite notFunky;
    [SerializeField] private Sprite funky;

    public void playButtonPressed()
    {
        SceneManager.LoadScene(gameScene.name);
    }

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

    public void quitButtonPressed()
    {
        Application.Quit();
    }
}
