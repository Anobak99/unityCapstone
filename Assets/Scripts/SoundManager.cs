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
    RoomCheck roomCheck;
    [SerializeField] private SoundList[] soundList;
    public List<GameObject> enemysoundPool; 
    public static SoundManager instance;
    private AudioSource audioSource;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            Debug.Log(currentSceneName);
        }
    }

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

    public static void EnemyPlaySound(SoundType sound, float volume = 1, int num = 0)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioSource audioSource = null;
        GameObject enemySound = null;       

        foreach (GameObject item in instance.enemysoundPool)
        {
            if (!item.activeSelf)
            {                
                enemySound = item;
                enemySound.SetActive(true);
                audioSource = enemySound.GetComponent<AudioSource>();
                break;
            }
        }

        Debug.Log("적 사운드 : " + clips[num] + ",  볼륨 : " + volume);
        if (!enemySound)
        {
            audioSource.PlayOneShot(clips[num], volume);
        }
    }
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    public string name;
    [SerializeField] private AudioClip[] sounds;
}
