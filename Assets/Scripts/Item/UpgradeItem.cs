using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeItem : MonoBehaviour
{
    [SerializeField] private int upgradeNum;

    private void OnEnable()
    {
        if (DataManager.Instance.currentData.abilities[upgradeNum])
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DataManager.Instance.currentData.abilities[upgradeNum] = true;
            DataManager.Instance.currentData.maxHp++;
            GameManager.Instance.maxHp = DataManager.Instance.currentData.maxHp;
            GameManager.Instance.PlayerHeal(1);
            gameObject.SetActive(false);
        }
    }
}
