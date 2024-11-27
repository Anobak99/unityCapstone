using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<UIManager>();
                if (obj != null)
                {
                    instance = obj;
                }
            }
            return instance;
        }
    }

    public ScreenFader screenFader;
    public GameObject HPUI;
    public GameObject blackScreen;
    public GameObject bloodScreen;

    private GameObject player;
    [SerializeField] private GameObject ResumeSelectButton;
    [SerializeField] private GameObject deathMassage;
    [SerializeField] private Slider hpBar;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider soundVolumeBar;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private GameObject volumeBtn;

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject inventoryScreen;
    [SerializeField] private GameObject settingScreen;
    [SerializeField] private GameObject mapImage;

    [SerializeField] private GameObject systemScreen;
    [SerializeField] private Image[] abilityGetImage;
    [SerializeField] private GameObject abilityconfirmButton;


    private bool isPauseMenu = false;

    public AudioClip clickSound;  // 선택 효과음
    private AudioSource audioSource;

    private void Awake()
    {
        var objs = FindObjectsOfType<UIManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        screenFader = GetComponentInChildren<ScreenFader>();
        audioSource = GetComponent<AudioSource>();

        soundVolumeBar.onValueChanged.AddListener(OnSliderValueChanged);
        OnSliderValueChanged(soundVolumeBar.value);
    }

    public void ActiveUI()
    {
        HPUI.SetActive(true);
    }

    public void DeactiveUI()
    {
        HPUI.SetActive(false);
    }

    public void RespawnBtn()
    {
        GameManager.Instance.RespawnPlayer();
    }

    public IEnumerator ActivateDeathMassage()
    {
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(screenFader.Fade(ScreenFader.FadeDirection.In, 0f));
        yield return new WaitForSeconds(0.8f);
        deathMassage.SetActive(true);
    }

    public IEnumerator DeactivateDeathMassage()
    {
        yield return new WaitForSeconds(0.2f);
        deathMassage.SetActive(false);
    }

    public IEnumerator ShowBloodScreen()
    {
        bloodScreen.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        bloodScreen.SetActive(false);
    }

    public void UpdateHealth(int hp, int maxHp)
    {
        hpBar.maxValue = maxHp;
        hpBar.value = hp;
    }

    public void PauseMenu()
    {
        PlayClickSound();

        if (inventoryScreen.activeSelf)
        {
            inventoryScreen.SetActive(false);
            StopCoroutine(InventoryManager.instance.ButtonControl());
        }

        if (settingScreen.activeSelf)
        {
            settingScreen.SetActive(false);
            StopCoroutine(InventoryManager.instance.ButtonControl());
        }

        if (mapImage.activeSelf)
        {
            mapImage.SetActive(false);
            StopCoroutine(InventoryManager.instance.ButtonControl());
        }

        if (!pauseScreen.activeSelf)
        {
            Time.timeScale = 0f;

            pauseScreen.SetActive(true);
            isPauseMenu = true;
            EventSystem.current.SetSelectedGameObject(ResumeSelectButton);

        }
        else
        {
            Time.timeScale = 1f;

            pauseScreen.SetActive(false);
            isPauseMenu = false;

        }
    }   

    public void GoSetting()
    {
        PlayClickSound();
        pauseScreen.SetActive(false);
        settingScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(volumeBtn);
    }

    public void SetVolune(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void OnSliderValueChanged(float volume)
    {
        volume = ((volume + 80) / 80) * 100;
        volumeText.text = volume.ToString("0");
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void GoInventory()
    {
        PlayClickSound();
        pauseScreen.SetActive(false);
        inventoryScreen.SetActive(true);
    }

    public void GoTitle()
    {
        PlayClickSound();
        Time.timeScale = 1f;

        pauseScreen.SetActive(false);
        DeactiveUI();
        GameManager.Instance.GoTitle();
    }

    public void MapMenu()
    {
        PlayClickSound();

        pauseScreen.SetActive(false);
        if (!mapImage.activeSelf)
        {
            mapImage.SetActive(true);
        }
        else
        {
            mapImage.SetActive(false);
        }
    }

    public bool MapOpened()
    {
        if (mapImage.activeSelf)
        {
            return true;
        }
        else { return false; }
    }

    public void SystemScreenON(int abilityNum)
    {
        systemScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(abilityconfirmButton);
        AbilityImageOn(abilityNum);
    }

    private void AbilityImageOn(int abilityNum)
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerController>().canAct = false;
        abilityGetImage[abilityNum].gameObject.SetActive(true);
    }

    public void SystemScreenOff()
    {
        for (int i = 0; i < abilityGetImage.Length; i++)
        {
            abilityGetImage[i].gameObject.SetActive(false);
        }
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerController>().canAct = true;
        systemScreen.SetActive(false);
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
