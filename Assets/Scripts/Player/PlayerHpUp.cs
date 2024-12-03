using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpUp : MonoBehaviour
{
    [SerializeField] private int statusId;
    [SerializeField] private int playerMaxHpUp;
    [SerializeField] private GameObject HpUPtext;
    [SerializeField] private PlayerController player;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        if (DataManager.instance.currentData.hpUpItem[statusId])
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.PlaySound(SoundType.SFX, 1f, 9);
            DataManager.instance.currentData.maxHp += playerMaxHpUp;
            GameManager.Instance.maxHp += playerMaxHpUp;
            GameManager.Instance.PlayerHeal(1);
            DataManager.instance.currentData.hpUpItem[statusId] = true;

            spriteRenderer.enabled = false;
            StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        HpUPtext.SetActive(true);

        yield return new WaitForSeconds(2f);

        HpUPtext.SetActive(false);
        gameObject.SetActive(false);
    }
}
