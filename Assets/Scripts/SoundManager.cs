using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum SoundType
{
    SWORD,FOOTSTEP,JUMP,DASH,LAND,HURT,MAGIC, 
    FOREST,VOLCANO,                              
    SFX, SKYTOWER, CASTLETOP                                    
}

public enum FiledType
{
    Forest, Castle, Tower, Volcano, CastleTop, Boss, Final, Title
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    [SerializeField] private BGMList[] bgmList;
    public static SoundManager instance;
    private AudioSource audioSource;
    public AudioSource bgmSource;

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

        // soundList 배열이 초기화되었는지 체크
        if (soundList == null || soundList.Length == 0)
        {
            Debug.LogError("Sound list is not assigned or is empty.");
        }
    }

    public static void PlaySound(SoundType sound, float volume = 1, int num = 0)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        //Debug.Log("사운드 : "+ clips[num] + ",  볼륨 : " + volume);
        instance.audioSource.PlayOneShot(clips[num], volume);
    }

    public static void PlayBGMSound(string stageName, float volume = 1, int num = 0)
    {
        foreach(BGMList stage in instance.bgmList)
        {
            if(stage.name == stageName)
            {
                AudioClip[] clips = stage.Sounds;
                instance.bgmSource.clip = clips[num];
                instance.bgmSource.Play();
                instance.bgmSource.volume = volume;
            }
        }
    }

    public static void StopBGMSound()
    {
        instance.bgmSource.Stop();
    }

}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    public string name;
    [SerializeField] private AudioClip[] sounds;
}

[Serializable]
public struct BGMList
{
    public AudioClip[] Sounds { get => sounds; }
    public string name;
    [SerializeField] private AudioClip[] sounds;
}
