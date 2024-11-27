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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (itype == ItemType.Heal)
            {
                GameManager.Instance.PlayerHeal(healPoint);
                SoundManager.PlaySound(SoundType.SFX, 1f, 9);
                Destroy(gameObject);
            }
        }
    }
}
