using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnSelectedButton : MonoBehaviour, ISelectHandler
{
    public AudioClip selectSound;  // 선택 효과음
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.6f;
        audioSource.playOnAwake = false;  // 자동 재생 방지
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlaySelectSound();
    }

    private void PlaySelectSound()
    {
        if (audioSource != null && selectSound != null)
        {
            audioSource.clip = selectSound;
            audioSource.Play();
        }
    }
}
