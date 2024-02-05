using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;  // 공격 쿨타임 (0이 되면 공격가능)
    public float startTimeBtwAttack; // 공격 쿨타임 설정

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange; // 공격 범위
    public int damage;        // 데미지 수치

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
                    if (enemiesToDamage[i].gameObject.tag == "Enemy") // 적과 충돌 시 데미지 처리
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
