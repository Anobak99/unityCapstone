using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusUp : MonoBehaviour
{
    [SerializeField] private int statusId;
    [SerializeField] private int playerMaxHpUp;
    [SerializeField] private int playerDamageUp;
    [SerializeField] private PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            GameManager.Instance.hp += playerMaxHpUp;
            GameManager.Instance.maxHp += playerMaxHpUp;
            player.damage += playerDamageUp;

            DataManager.instance.currentData.maxHp += 1;
            DataManager.instance.currentData.attackUp[statusId]=true;
            gameObject.SetActive(false);
        }
    }
}
