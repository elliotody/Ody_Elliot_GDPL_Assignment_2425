using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class WinLose : MonoBehaviour
{
    public static WinLose instance;

    [SerializeField] private TextMeshProUGUI winScreen;
    [SerializeField] private TextMeshProUGUI loseScreen;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void win()
    {
        HUD.instance.gameObject.SetActive(false);
        gameObject.SetActive(true);
        loseScreen.gameObject.SetActive(false);
        returnToMainMenu(3000);
    }

    public void lose()
    {
        HUD.instance.gameObject.SetActive(false);
        gameObject.SetActive(true);
        winScreen.gameObject.SetActive(false);
        returnToMainMenu(3000);
    }

    async void returnToMainMenu(int time)
    {
        await Task.Delay(time);
        SceneManager.LoadScene(HUD.instance.mainMenu.name);
    }
}
