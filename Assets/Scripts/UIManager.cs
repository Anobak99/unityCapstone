using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        screenFader = GetComponentInChildren<ScreenFader>();
    }

    public IEnumerator ActiveDeathMassage()
    {
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(screenFader.Fade(ScreenFader.FadeDirection.In));
        yield return new WaitForSeconds(0.8f);
        deathMassage.SetActive(true);
    }
}
