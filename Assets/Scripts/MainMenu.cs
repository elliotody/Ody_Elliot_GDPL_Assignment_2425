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
    private bool isFunky = false;

    public void playButtonPressed()
    {
        SceneManager.LoadScene(gameScene.name);
    }

    public void funkyButtonPressed()
    {
        if (isFunky)
        {
            isFunky = false;
            logo.sprite = notFunky;
        }
        else
        {
            isFunky = true;
            logo.sprite = funky;
        }
    }

    public void quitButtonPressed()
    {
        Application.Quit();
    }
}
