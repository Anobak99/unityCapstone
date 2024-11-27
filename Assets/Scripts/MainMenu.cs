using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject mainCam;
    [SerializeField] GameObject mainmenuScreen;
    [SerializeField] GameObject selectStartBtn;
    [SerializeField] GameObject selectSaveBtn;
    [SerializeField] GameObject selectOptionBtn;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject optionMenu;
    [SerializeField] TextMeshProUGUI startText1;
    [SerializeField] TextMeshProUGUI startText2;
    [SerializeField] TextMeshProUGUI startText3;


    [SerializeField] TextMeshProUGUI startGame1_text;
    [SerializeField] TextMeshProUGUI startGame2_text;
    [SerializeField] TextMeshProUGUI startGame3_text;
    [SerializeField] GameObject startGame1;
    [SerializeField] GameObject startGame2;
    [SerializeField] GameObject startGame3;
    [SerializeField] GameObject deleteGame1;
    [SerializeField] GameObject deleteGame2;
    [SerializeField] GameObject deleteGame3;

    public AudioClip clickSound;  // 선택 효과음
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;  // 자동 재생 방지
        SoundManager.PlayBGMSound("Title", 0.2f, 0);
        StartCoroutine(ButtonControl());
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


        if (DataManager.Instance.FileCheck("saveFile1.json"))
        {
            startText1.text = DataManager.instance.currentData.areaName;
            startGame1_text.text = "이어하기";
            deleteGame1.SetActive(true);
        }
        else
        {
            startGame1_text.text = "새로하기";
        }

        if (DataManager.Instance.FileCheck("saveFile2.json"))
        {
            startText2.text = DataManager.instance.currentData.areaName;
            startGame2_text.text = "이어하기";
            deleteGame2.SetActive(true);
        }
        else
        {
            startGame2_text.text = "새로하기";
        }

        if (DataManager.Instance.FileCheck("saveFile3.json"))
        {
            startText3.text = DataManager.instance.currentData.areaName;
            startGame3_text.text = "이어하기";
            deleteGame3.SetActive(true);
        }
        else
        {
            startGame3_text.text = "새로하기";
        }
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
        mainmenuScreen.SetActive(false);

        PlayClickSound();
        StopCoroutine(ButtonControl());
        DisableMenu();
        SoundManager.StopBGMSound();
        if (DataManager.Instance.FileCheck("saveFile1.json"))
        {
            DataManager.Instance.LoadData();
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
        else
        {
            GameManager.Instance.respawnScene = "Level1-0";
            GameManager.Instance.respawnPoint = new Vector2(-7.04f, 0);
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
    }

    public void DeleteGame1()
    {
        PlayClickSound();

        DataManager.Instance.FileDelete("saveFile1.json");
        startText1.text = "Empty";
        startGame1_text.text = "새로하기";
        DataManager.Instance.FileCheck("saveFile1.json");

        deleteGame1.SetActive(false);
        EventSystem.current.SetSelectedGameObject(startGame1);
    }

    public void StartGame2()
    {
        mainmenuScreen.SetActive(false);

        PlayClickSound();
        StopCoroutine(ButtonControl());
        DisableMenu();
        SoundManager.StopBGMSound();
        if (DataManager.Instance.FileCheck("saveFile2.json"))
        {
            DataManager.Instance.LoadData();
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
        else
        {
            GameManager.Instance.respawnScene = "Level1-0";
            GameManager.Instance.respawnPoint = new Vector2(-7.04f, 0);
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
    }

    public void DeleteGame2()
    {
        PlayClickSound();

        DataManager.Instance.FileDelete("saveFile2.json");
        startText2.text = "Empty";
        startGame2_text.text = "새로하기";
        DataManager.Instance.FileCheck("saveFile2.json");

        deleteGame2.SetActive(false);
        EventSystem.current.SetSelectedGameObject(startGame2);
    }

    public void StartGame3()
    {
        mainmenuScreen.SetActive(false);

        PlayClickSound();
        StopCoroutine(ButtonControl());
        DisableMenu();
        SoundManager.StopBGMSound();
        if (DataManager.Instance.FileCheck("saveFile3.json"))
        {
            DataManager.Instance.LoadData();
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
        else
        {
            GameManager.Instance.respawnScene = "Level1-0";
            GameManager.Instance.respawnPoint = new Vector2(-7.04f, 0);
            SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        }
    }

    public void DeleteGame3()
    {
        PlayClickSound();

        DataManager.Instance.FileDelete("saveFile3.json");
        startText3.text = "Empty";
        startGame3_text.text = "새로하기";
        DataManager.Instance.FileCheck("saveFile3.json");

        deleteGame3.SetActive(false);
        EventSystem.current.SetSelectedGameObject(startGame3);
    }

    public void DeleteFiles()
    {
        return;
    }

    public void GoToMain()
    {
        PlayClickSound();
        EventSystem.current.SetSelectedGameObject(null);
        startMenu.SetActive(false);
        optionMenu.SetActive(false);
        mainMenu.SetActive(true); 
    }

    public void DisableMenu()
    {
        mainCam.SetActive(false);
        mainmenuScreen.SetActive(false);
    }

    public void EnableMainMenu()
    {
        mainCam.SetActive(true);
        mainmenuScreen.SetActive(true);
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
