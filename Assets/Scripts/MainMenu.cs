using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject optionMenu;

    public void GameStart()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void Option()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void StartGame()
    {
        GameManager.Instance.respawnScene = "Level1-0";
        SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
    }

    public void GoToMain()
    {
        startMenu.SetActive(false);
        optionMenu.SetActive(false);
        mainMenu.SetActive(true); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
