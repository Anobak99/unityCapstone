using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public enum ItemType
    {
        Heal
    }
    public ItemType itype;
    [SerializeField] private int healPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(itype == ItemType.Heal)
            {
                GameManager.Instance.PlayerHeal(healPoint);
                Destroy(gameObject);
            }
        }
    }
}
