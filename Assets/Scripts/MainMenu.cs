using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject selectStartBtn;
    [SerializeField] GameObject selectSaveBtn;
    [SerializeField] GameObject selectOptionBtn;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject optionMenu;
    [SerializeField] TextMeshProUGUI startText1;
    [SerializeField] TextMeshProUGUI startText2;
    [SerializeField] TextMeshProUGUI startText3;

    public AudioClip clickSound;  // 선택 효과음
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;  // 자동 재생 방지

        StartCoroutine(ButtonControl());
    }

    private void Update()
    {
        Debug.Log(DataManager.instance);
    }

    IEnumerator ButtonControl()
    {
        while (true)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (Input.anyKeyDown)
                {
                    if (mainMenu.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(selectStartBtn);
                    }
                    else if(startMenu.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(selectSaveBtn);
                    }
                    else if(optionMenu.activeSelf)
                    {
                        EventSystem.current.SetSelectedGameObject(selectOptionBtn);
                    }
                }
            }
            yield return null;
        }
    }

    public void GameStart()
    {
        PlayClickSound();
        EventSystem.current.SetSelectedGameObject(null);
        mainMenu.SetActive(false);
        startMenu.SetActive(true);


        if (DataManager.instance.FileCheck("saveFile1.json"))
            startText1.text = DataManager.instance.currentData.areaName;
        if (DataManager.instance.FileCheck("saveFile2.json"))  
            startText2.text = DataManager.instance.currentData.areaName;
        if (DataManager.instance.FileCheck("saveFile3.json"))
            startText3.text = DataManager.instance.currentData.areaName;
    }
           
    public void Option()
    {
        PlayClickSound();
        EventSystem.current.SetSelectedGameObject(null);
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void StartGame1()
    {
        PlayClickSound();
        StopCoroutine(ButtonControl());
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
        PlayClickSound();
        StopCoroutine(ButtonControl());
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
        PlayClickSound();
        StopCoroutine(ButtonControl());
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
        PlayClickSound();
        EventSystem.current.SetSelectedGameObject(null);
        startMenu.SetActive(false);
        optionMenu.SetActive(false);
        mainMenu.SetActive(true); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.clip = clickSound;
            audioSource.Play();
        }
    }
}
