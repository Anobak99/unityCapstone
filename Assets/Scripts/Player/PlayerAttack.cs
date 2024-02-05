using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;  // ���� ��Ÿ�� (0�� �Ǹ� ���ݰ���)
    public float startTimeBtwAttack; // ���� ��Ÿ�� ����

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange; // ���� ����
    public int damage;        // ������ ��ġ

    private void Awake()
    {
        
    }

    private void Update()
    {

        if (timeBtwAttack <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                Debug.Log("Player Attack!");
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (enemiesToDamage[i].gameObject.tag == "Enemy") // ���� �浹 �� ������ ó��
                    {
                        Debug.Log("Enemy Hit!");
                        enemiesToDamage[i].GetComponent<Enemy>().Attacked(damage, attackPos.position);
                    }
                    else if (enemiesToDamage[i].gameObject.tag == "Boss")
                    {
                        //[i].GetComponent<Boss>().TakeDamage(damage, attackPos.position);
                    }
                }
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
