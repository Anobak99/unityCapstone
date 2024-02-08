using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int dmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {              
            GameManager.Instance.PlayerHit(1);
            Debug.Log("hit!");
        }
    }
}
