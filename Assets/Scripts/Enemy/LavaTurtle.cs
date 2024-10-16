using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LavaTurtle : Enemy
{
    public float moveDirection;
    public float speed;

    private float horizental;
    private float playerDistance;
    public float viewRange;
    public float attackRange;
    public bool facingRight;
    private int moveCount;

    public Transform attackPos;
    public Transform lavaPos;
    public LayerMask whatIsEnemies;
    public float hitRange;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;
    private bool isplatform;

    public GameObject lavaPrefab; // ��� ������
    private List<GameObject> pool = new List<GameObject>(); // ������ ������Ʈ Ǯ

    public Color color;
    public GameObject blood;

    public float cooldownTime = 5f; // ��Ÿ�� ���� 
    private bool isLavaFlowCooldown = false; // ��Ÿ�� ����

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hitRange);
    }

    public override IEnumerator Think()
    {
        Check();

        if (player != null && !GameManager.Instance.isDead)
        {
            horizental = player.position.x - transform.position.x;
            if (horizental < viewRange && player.position.y +1.5f >= transform.position.y && player.position.y < transform.position.y) //����� �ν� ���� ������ ���
            {
                FlipToPlayer(horizental);
                playerDistance = Mathf.Abs(horizental);
                if (playerDistance > attackRange) //����� �Ÿ��� ���ݹ��� ���� ���
                {
                    Debug.Log("�÷��̾� ���� ���� ��");
                    if (isGround && !isWall && !isplatform && !animator.GetBool("Hit")) //��ü ���� ������ �̵� ������ ���
                    {
                        Debug.Log("�÷��̾�� �̵�");
                        animator.SetInteger("AnimState", 1);
                        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                    }
                    else
                    {
                        Debug.Log("�ǵ������ : ���� ������ �̵��Ұ�����");
                        animator.SetInteger("AnimState", 0);
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else if (playerDistance <= attackRange) //����� ���ݰŸ� ���� ���
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    animator.SetInteger("AnimState", 0);
                    act2 = StartCoroutine(Attack());
                    yield break;
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetInteger("AnimState", 0);
            }
        }
        else
        {
            if (moveCount > 9)
            {
                moveCount = 0;
                moveDirection = Random.Range(-1, 2);
            }
            else
                moveCount++;

            if (moveDirection == 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetInteger("AnimState", 0);
            }
            else
            {
                if (isGround && !isWall && !animator.GetBool("Hit")) //��ü ���� ������ �̵� ������ ���
                {
                    animator.SetInteger("AnimState", 1);
                    rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                }
                else
                {
                    animator.SetInteger("AnimState", 0);
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }

                FlipToPlayer(0f);
            }
        }


        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
       
        Instantiate(blood, new Vector2(transform.position.x, transform.position.y + 1f), Quaternion.identity);
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", true);
        canDamage = false;
        animator.SetInteger("AnimState", 0);

        hp -= dmg;

        if (player.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(-2f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(2f, rb.velocity.y);
        }

        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", false);

        if (hp <= 0)
        {
            canAct = false;
            StartCoroutine(Death());
            yield break;
        }

        if (!isLavaFlowCooldown)
        {
            StartCoroutine(LavaOverFlow());
        }
        else
        {
            act1 = StartCoroutine(Think());
        }

        canDamage = true;
    }

    private void Check()
    {
        isGround = Physics2D.OverlapBox(groundCheck.position, new Vector2(1f, 0.1f), 0f);
        isWall = Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayor);
        isplatform = Physics2D.OverlapCircle(new Vector2(WallCheck.position.x, WallCheck.position.y - 0.5f), 0.1f, groundLayor);
    }

    private void FlipToPlayer(float playerPosition)
    {
        if (playerPosition < 0)
        {
            moveDirection = -1;
        }
        else if (playerPosition > 0)
        {
            moveDirection = 1;
        }

        if (moveDirection > 0 && !facingRight)
        {
            facingRight = true;
            transform.Rotate(0, 180, 0);
        }
        else if (moveDirection < 0 && facingRight)
        {
            facingRight = false;
            transform.Rotate(0, 180, 0);
        }
    }

    private IEnumerator LavaOverFlow()
    {
        Debug.Log("��� �����÷���");      
        animator.SetTrigger("LavaOverFlow");

        isLavaFlowCooldown = true;
        GameObject select = null;

        foreach (GameObject item in pool)
        {
            if (!item.activeSelf)
            {
                Debug.Log("������Ʈ Ǯ�� �������� ����");
                select = item;
                select.SetActive(true);
                select.transform.position = this.transform.position;
                break;
            }
        }

        if (!select)
        {
            Debug.Log("������Ʈ Ǯ�� �������� ����");
            select = Instantiate(lavaPrefab, lavaPos.transform.position, Quaternion.identity);
            pool.Add(select);
        }

        yield return new WaitForSeconds(cooldownTime);
        isLavaFlowCooldown = false;

        canAct = true;
        act1 = StartCoroutine(Think());

        yield return null;
    }

    public override IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.5f);
        act1 = StartCoroutine(Think());
    }
}
