using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnSelectedButton : MonoBehaviour, ISelectHandler
{
    public AudioClip selectSound;  // ���� ȿ����
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.6f;
        audioSource.playOnAwake = false;  // �ڵ� ��� ����
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
