using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mage_move : Enemy
{
private float horizental;
    private float playerDistance;
    public float viewRange;
    public float attackRange;
    public bool facingRight;
    private bool playerFound;

    [SerializeField] private Enemy_Pool objectPool;
    public GameObject objectPrefab;
    private List<GameObject> bullets = new List<GameObject>();

    public override IEnumerator Think()
    {
        if (player != null && !GameManager.Instance.isDead) //�÷��̾ ������� ������ �۵�
        {
            horizental = Vector2.Distance(player.transform.position, transform.position); //�÷��̾������ x�Ÿ�
            playerDistance = Mathf.Abs(horizental);
            if (playerDistance < viewRange) //����� �ν� ���� ������ ���
            {
                FlipToPlayer(horizental);
                if (playerFound) //�÷��̾ �ν��� ��Ȳ�� ��
                {
                    if (playerDistance > attackRange) //����� �Ÿ��� ���ݹ��� ���� ���
                    {
                        act2 = StartCoroutine(Attack2()); //���Ÿ� ���� �ڷ�ƾ ����
                        yield break; //���� �ڷ�ƾ ����
                    }
                    else if(player.position.y >= transform.position.y - 0.5f && player.position.y < transform.position.y + 2.5f)//����� ���ݰŸ� ���� ���
                    {
                        act2 = StartCoroutine(Attack()); //���� ���� �ڷ�ƾ ����
                        yield break; //���� �ڷ�ƾ ����
                    }
                }
                else
                {
                    if (facingRight)
                    {
                        if (horizental > 0)
                            playerFound = true;
                    }
                    else
                    {
                        if (horizental < 0)
                            playerFound = true;
                    }
                }
            }
            else //����� ã�� ������ ��
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        //0.1�ʸ��� �ٽ� ȣ��
        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }


    private void FlipToPlayer(float playerPosition) //�÷��̾ ���� ���� ��ȯ
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

    public override IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.5f);
        act1 = StartCoroutine(Think());
    }

    public IEnumerator Attack2()
    {
        GameObject fire;
        animator.SetTrigger("cast");
        yield return new WaitForSeconds(0.8f);
        animator.SetTrigger("magic");
        yield return new WaitForSeconds(0.5f);
        fire = objectPool.GetObject(new Vector2(transform.position.x, transform.position.y +0.7f), "Mage");
        Rigidbody2D rigid = fire.GetComponent<Rigidbody2D>();
        Vector2 dirVec = player.transform.position - transform.position;
        rigid.AddForce(dirVec.normalized*5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1.5f);
        act1 = StartCoroutine(Think());
    }


    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", true);
        canDamage = false;
        animator.SetInteger("AnimState", 0);
        spriteRenderer.material = flashMaterial;

        hp -= dmg;

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;
        yield return new WaitForSeconds(0.4f);
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", false);

        if (hp <= 0)
        {
            StartCoroutine(Death());
            yield break;
        }

        canDamage = true;
        act1 = StartCoroutine(Think());
    }
}
