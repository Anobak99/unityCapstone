using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage_move : Enemy
{
    public float moveDirection;
    public float speed;

    private float horizental;
    private float playerDistance;
    public float viewRange;
    public float attackRange;
    public float attackRange2;
    public bool facingRight;
    private bool playerFound;

    public GameObject objectPrefab;
    private List<GameObject> bullets = new List<GameObject>();

    public override IEnumerator Think()
    {
        if (player != null && !GameManager.Instance.isDead) //�÷��̾ ������� ������ �۵�
        {
            horizental = player.position.x - transform.position.x; //�÷��̾������ x�Ÿ�
            playerDistance = Mathf.Abs(horizental);
            if (playerDistance < viewRange ) //����� �ν� ���� ������ ���
            {
                FlipToPlayer(horizental);
                if (playerFound) //�÷��̾ �ν��� ��Ȳ�� ��
                {
                    if (playerDistance > attackRange) //����� �Ÿ��� ���ݹ��� ���� ���
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        act2 = StartCoroutine(Attack2()); //���Ÿ� ���� �ڷ�ƾ ����
                        yield break; //���� �ڷ�ƾ ����
                    }
                    else if(player.position.y >= transform.position.y - 0.5f && player.position.y < transform.position.y + 2f)//����� ���ݰŸ� ���� ���
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
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
        }

        //0.1�ʸ��� �ٽ� ȣ��
        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }


    private void FlipToPlayer(float playerPosition) //�÷��̾ ���� ���� ��ȯ
    {
        if (playerPosition < 0 && facingRight)
        {
            moveDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if (playerPosition > 0 && !facingRight)
        {
            moveDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    public override IEnumerator Attack()
    {
        animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(2f);
        act1 = StartCoroutine(Think());
    }

    public IEnumerator Attack2()
    {
        GameObject fire;
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(0.6f);
        fire = Shoot();
        fire.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
        Rigidbody2D rigid = fire.GetComponent<Rigidbody2D>();
        Vector2 dirVec = player.transform.position - transform.position;
        rigid.AddForce(dirVec.normalized*8, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1.5f);
        act1 = StartCoroutine(Think());
    }

    private GameObject Shoot() //�Ѿ� ����, ������ ������Ʈ ��Ȱ�� �� ���� �� ����
    {
        GameObject select = null;

        foreach (GameObject item in bullets)
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(objectPrefab, transform);
            bullets.Add(select);
        }

        return select;
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", true);
        canDamage = false;
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

    public override void OnNotify()
    {
        GameObject select = null;

        foreach (GameObject item in bullets)
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(false);
                break;
            }
        }

        gameObject.SetActive(false);
    }
}
