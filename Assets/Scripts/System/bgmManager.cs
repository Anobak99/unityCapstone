using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageName
{
    FOREST, VOLCANO, SKYTOWER
}

[RequireComponent(typeof(AudioSource))]
public class bgmManager : MonoBehaviour
{
    public static bgmManager instance;

    [SerializeField] AudioClip[] bgm;
    private AudioSource audioSource;
    [SerializeField] private int currentStage = (int)StageName.VOLCANO;

    private void Awake()
    {       
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlayBGM(currentStage);
    }

    public static void PlayBGM(int stageNum, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.bgm[stageNum], volume);
    }
}
