using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Warrior_move : Enemy
{
    private float horizental;
    private float playerDistance;
    public float attackRange;
    public bool facingRight;
    public GameObject DamageBox;

    public override void OnEnable()
    {
        base.OnEnable();

        DamageBox.SetActive(true);
    }

    public override IEnumerator Think()
    {
        if (player != null && !GameManager.Instance.isDead) //플레이어가 살아있을 때에만 작동
        {
            horizental = player.position.x - transform.position.x; //플레이어까지의 x거리
            playerDistance = Mathf.Abs(horizental);
            //바라보는 방향에 플레이어가 있을 경우
            if ((facingRight && player.position.x > transform.position.x) || (!facingRight && player.position.x < transform.position.x))
            {
                if (playerDistance < attackRange) //대상의 거리가 공격범위 안일 경우
                {
                    act2 = StartCoroutine(Attack()); //공격 코루틴 시작
                    yield break; //현재 코루틴 정지
                }
            }
        }

        //0.1초마다 다시 호출
        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }

    public override IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(2f);
        act1 = StartCoroutine(Think());
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        canDamage = false;
        spriteRenderer.material = flashMaterial;
        hp -= dmg;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;
        yield return new WaitForSeconds(0.4f);

        if (hp <= 0)
        {
            DamageBox.SetActive(false);
            StartCoroutine(Death());
            yield break;
        }

        canDamage = true;
        act1 = StartCoroutine(Think());
    }
}
