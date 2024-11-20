using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum SoundType
{
    SWORD,FOOTSTEP,JUMP,DASH,LAND,HURT,MAGIC, // PLAYER
    FOREST,VOLCANO,                                  // ENEMY
    SFX                                       // Sound Effect
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    public static SoundManager instance;
    private AudioSource audioSource;

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

        // soundList �迭�� �ʱ�ȭ�Ǿ����� üũ
        if (soundList == null || soundList.Length == 0)
        {
            Debug.LogError("Sound list is not assigned or is empty.");
        }
    }

    public static void PlaySound(SoundType sound, float volume = 1, int num = 0)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        //Debug.Log("���� : "+ clips[num] + ",  ���� : " + volume);
        instance.audioSource.PlayOneShot(clips[num], volume);
    }

}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    public string name;
    [SerializeField] private AudioClip[] sounds;
}
