using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnSelectedItem : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public AudioClip selectSound;  // 선택 효과음
    public GameObject itemSelectIcon;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.6f;
        audioSource.playOnAwake = false;  // 자동 재생 방지
    }

    public void OnSelect(BaseEventData eventData)
    {
        int num = transform.GetSiblingIndex();
        InventoryManager.instance.ItemDescTextType(num);
        itemSelectIcon.SetActive(true);
        PlaySelectSound();        

    }

    public void OnDeselect(BaseEventData eventData)
    {
        InventoryManager.instance.ItemDescTextClear();
        itemSelectIcon.SetActive(false);
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
