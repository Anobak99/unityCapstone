using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject optionMenu;
    [SerializeField] TextMeshPro     startText1;
    [SerializeField] TextMeshPro startText2;
    [SerializeField] TextMeshPro startText3;

    public void GameStart()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(true);
        //if (DataManager.instance.FileCheck("saveFile1.json"))
        //    startText1.text = DataManager.instance.currentData.areaName;
        //if (DataManager.instance.FileCheck("saveFile2.json"))
        //    startText2.text = DataManager.instance.currentData.areaName;
        //if (DataManager.instance.FileCheck("saveFile3.json"))
        //    startText3.text = DataManager.instance.currentData.areaName;
    }

    public void Option()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void StartGame1()
    {
        if (DataManager.Instance.FileCheck("saveFile1.json"))
        {
            DataManager.Instance.LoadData();
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
        else
        {
            GameManager.Instance.respawnScene = "Level1-0";
            GameManager.Instance.respawnPoint = new Vector2(-7.04f, -0.98f);
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
    }

    public void StartGame2()
    {
        if (DataManager.Instance.FileCheck("saveFile2.json"))
        {
            DataManager.Instance.LoadData();
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
        else
        {
            GameManager.Instance.respawnScene = "Level1-0";
            GameManager.Instance.respawnPoint = new Vector2(-7.04f, -0.98f);
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
    }

    public void StartGame3()
    {
        if (DataManager.Instance.FileCheck("saveFile3.json"))
        {
            DataManager.Instance.LoadData();
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
        else
        {
            GameManager.Instance.respawnScene = "Level1-0";
            GameManager.Instance.respawnPoint = new Vector2(-7.04f, -0.98f);
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
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
