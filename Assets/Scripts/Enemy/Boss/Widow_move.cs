using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Widow_move : Boss
{
    private bool facingRight = true;
    private float horizental;

    private int attackCount; //공격횟수
    private int patternNum = 0;
    [SerializeField] private float jumpHeight;
    [SerializeField] private BossBattle battle;

    public override IEnumerator Think()
    {

        if (GameManager.Instance.gameState != GameManager.GameState.Boss || isDead || Time.timeScale == 0) yield return null;

        if (canAct && player != null && !GameManager.Instance.isDead)
        {
            horizental = player.position.x - transform.position.x;
            FlipToPlayer(horizental);

            switch (patternNum)
            {
                case 0:
                    act2 = StartCoroutine(Attack1());
                    yield break;
                case 1:
                    act2 = StartCoroutine(Attack2());
                    yield break;
                case 2:
                    act2 = StartCoroutine(Jump());
                    yield break;
                case 3:
                    act2 = StartCoroutine(Land());
                    yield break;
                case 4:
                    act2 = StartCoroutine(Attack3());
                    yield break;
                default:
                    break;
            }

            yield return new WaitForSeconds(0.1f);
            act1 = StartCoroutine(Think());
        }
    }


    private void FlipToPlayer(float playerPosition)
    {
        if (playerPosition < 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if (playerPosition > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        canAct = true;
        act1 = StartCoroutine(Think());
    }

    IEnumerator Attack1() //근접공격
    {
        canAct = false;
        animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(1f);
        patternNum++;
        act2 = StartCoroutine(Wait(1.5f));
    }

    IEnumerator Attack2() //원거리 공격
    {
        canAct = false;
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(1f);
        patternNum++;
        act2 = StartCoroutine(Wait(1.5f));
    }
    
    IEnumerator Jump()
    {
        canAct = false;
        canDamage = false;
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(1f);
        patternNum++;
        act2 = StartCoroutine(Wait(1.5f));
    }

    IEnumerator Land()
    {
        canAct = false;
        canDamage = true;
        transform.position = new Vector2(player.transform.position.x, 27f);
        animator.SetTrigger("Land");
        yield return new WaitForSeconds(1f);
        patternNum++;
        attackCount++;
        if(attackCount == 3)
        {
            attackCount = 0;
            act2 = StartCoroutine(Wait(5f));
        }
        else 
            act2 = StartCoroutine(Wait(1.5f));
    }

    IEnumerator Attack3() //주변 공격
    {
        canAct = false;
        animator.SetTrigger("Attack3");
        yield return new WaitForSeconds(1f);
        patternNum = 0;
        act2 = StartCoroutine(Wait(1.5f));
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        canDamage = false;
        spriteRenderer.material = flashMaterial;
        hp -= dmg;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;
        if (hp <= 0)
        {
            StartCoroutine(Death());
            yield break;
        }
        canDamage = true;
    }

    private IEnumerator Death()
    {
        StopCoroutine(act1);
        StopCoroutine(act2);
        rb.velocity = Vector2.zero;
        canDamage = false;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(4f);
        animator.SetTrigger("Explo");
        yield return new WaitForSeconds(1.5f);
        isDead = true;
        battle.BossDead();
        gameObject.SetActive(false);
    }
}
