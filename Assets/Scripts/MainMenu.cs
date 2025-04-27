using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneAsset gameScene; 

    public void playButtonPressed()
    {
        SceneManager.LoadScene(gameScene.name);
    }

    public void quitButtonPressed()
    {
        Application.Quit();
    }
}
