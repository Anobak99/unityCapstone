using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                else
                {
                    var newObj = new GameObject().AddComponent<UIManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    public ScreenFader screenFader;
    [SerializeField] private GameObject deathMassage;
    [SerializeField] private Slider hpBar;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject mapImage;

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
        StartCoroutine(screenFader.Fade(ScreenFader.FadeDirection.Out, 0f));
    }

    public void UpdateHealth(int hp, int maxHp)
    {
        hpBar.maxValue = maxHp;
        hpBar.value = hp;
    }

    public void PauseMenu()
    {
        if (!pauseScreen.activeSelf)
        {
            Time.timeScale = 0f;

            pauseScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;

            pauseScreen.SetActive(false);
        }
    }

    public void GoTitle()
    {
        Time.timeScale = 1f;

        pauseScreen.SetActive(false);
    }

    public void MapMenu()
    {
        if(!mapImage.activeSelf)
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
}
