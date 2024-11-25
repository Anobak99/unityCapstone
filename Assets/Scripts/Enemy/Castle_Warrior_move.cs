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
        if (player != null && !GameManager.Instance.isDead) //�÷��̾ ������� ������ �۵�
        {
            horizental = player.position.x - transform.position.x; //�÷��̾������ x�Ÿ�
            playerDistance = Mathf.Abs(horizental);
            //�ٶ󺸴� ���⿡ �÷��̾ ���� ���
            if ((facingRight && player.position.x > transform.position.x) || (!facingRight && player.position.x < transform.position.x))
            {
                if (playerDistance < attackRange) //����� �Ÿ��� ���ݹ��� ���� ���
                {
                    act2 = StartCoroutine(Attack()); //���� �ڷ�ƾ ����
                    yield break; //���� �ڷ�ƾ ����
                }
            }
        }

        //0.1�ʸ��� �ٽ� ȣ��
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
