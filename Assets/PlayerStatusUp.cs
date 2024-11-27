using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusUp : MonoBehaviour
{
    [SerializeField] private GameObject HpUPtext;
    [SerializeField] private GameObject AttackUPtext;

    [SerializeField] private int statusId;
    [SerializeField] private int playerMaxHpUp;
    [SerializeField] private int playerDamageUp;
    [SerializeField] private PlayerController player;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isUse = false;

    private void OnEnable()
    {
        if(DataManager.instance.currentData.attackUpItem[statusId])
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            if (playerMaxHpUp > 0)
            {
                GameManager.Instance.hp += playerMaxHpUp;
                GameManager.Instance.maxHp += playerMaxHpUp;
                DataManager.instance.currentData.maxHp += playerMaxHpUp;
            }

            if (playerDamageUp > 0)
            {
                player.damage += playerDamageUp;
                DataManager.instance.currentData.attackUpItem[statusId] = true;
            }

            spriteRenderer.enabled = false;
            StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        if(playerMaxHpUp > 0)
        {
            HpUPtext.SetActive(true);
        }
        
        if (playerDamageUp > 0)
        {
            AttackUPtext.SetActive(true);
        }

        yield return new WaitForSeconds(2f);
        HpUPtext.SetActive(false);
        AttackUPtext.SetActive(false);
        gameObject.SetActive(false);
        isUse = true;
    }
}
