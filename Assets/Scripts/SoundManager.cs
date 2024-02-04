using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    List<AudioSource> sfxPlayers;
    private AudioSource select;
    private GameObject sfxSound;


    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        //������� ��� ������Ʈ �ʱ�ȭ
        GameObject bgmSound = new GameObject("BgmPlayer");
        bgmSound.transform.parent = transform;
        bgmPlayer = bgmSound.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        //ȿ���� ��� ������Ʈ �ʱ�ȭ
        sfxSound = new GameObject("SfxPlayer");
        sfxSound.transform.parent = transform;
        sfxPlayers = new List<AudioSource>();
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay) { bgmPlayer.Play(); }
        else { bgmPlayer.Stop(); }
    }

    public void PlaySfx(int sfx) //ȿ���� ���
    {
        bool canPlay = false;

        //��� ������ ������ҽ� Ž��
        foreach (AudioSource currentPlayer in sfxPlayers)
        {
            if(!currentPlayer.isPlaying)
            {
                select = currentPlayer;
                canPlay = true;
                break;
            }
        }


        //���� �� ���ο� ����� �ҽ� �߰�
        if(!canPlay)
        {
            select = sfxSound.AddComponent<AudioSource>();
            select.playOnAwake = false;
            select.volume = sfxVolume;
            sfxPlayers.Add(select);
        }

        select.clip = sfxClip[sfx];
        select.Play();
    }
}