using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBox : MonoBehaviour
{
    [SerializeField] private Transform attackPos;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private float hitRange;
    [SerializeField] private int dmg;

    public void Hit()
    {
        Collider2D[] attackBox = Physics2D.OverlapCircleAll(attackPos.position, hitRange, whatIsEnemies);
        for (int i = 0; i < attackBox.Length; i++)
        {
            if (attackBox[i].gameObject.tag == "Player")
            {
                GameManager.Instance.PlayerHit(dmg);
            }
        }
    }
}
