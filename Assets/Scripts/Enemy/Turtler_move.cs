using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtler_move : Enemy
{
    public float moveDirection;
    public float speed;

    private float horizental;
    private float playerDistance;
    public float viewRange;
    public float attackRange;
    public bool facingRight;
    private bool ready;
    private int moveCount;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float hitRange;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;
    private bool isplatform;

    public override IEnumerator Think()
    {
        Check();

        if (player != null && !GameManager.Instance.isDead && ready)
        {
            horizental = player.position.x - transform.position.x;
            if (horizental < viewRange && player.position.y >= transform.position.y && player.position.y < transform.position.y + 1.5f) //����� �ν� ���� ������ ���
            {
                FlipToPlayer(horizental);
                playerDistance = Mathf.Abs(horizental);
                if (playerDistance > attackRange) //����� �Ÿ��� ���ݹ��� ���� ���
                {
                    if (isGround && !isWall && isplatform && !animator.GetBool("Hit")) //��ü ���� ������ �̵� ������ ���
                    {
                        animator.SetInteger("AnimState", 1);
                        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                    }
                    else
                    {
                        animator.SetInteger("AnimState", 0);
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else //����� ���ݰŸ� ���� ���
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
                if (isGround && !isWall && isplatform && !animator.GetBool("Hit")) //��ü ���� ������ �̵� ������ ���
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
        rb.velocity = Vector2.zero;
        ready = true;
        animator.SetBool("Hit", true);
        canDamage = false;
        animator.SetInteger("AnimState", 0);
        spriteRenderer.material = flashMaterial;

        hp -= dmg;

        if (player.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(-2f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(2f, rb.velocity.y);
        }

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

    private void Check()
    {
        isGround = Physics2D.OverlapBox(groundCheck.position, new Vector2(1f, 0.1f), 0f);
        isWall = Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayor);
        isplatform = Physics2D.OverlapCircle(new Vector2(WallCheck.position.x, WallCheck.position.y - 0.5f), 0.1f, groundLayor);
    }

    private void FlipToPlayer(float playerPosition)
    {
        if (playerPosition < 0 && ready)
        {
            moveDirection = -1;
        }
        else if (playerPosition > 0 && ready)
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

    public override IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.5f);
        act1 = StartCoroutine(Think());
    }

}
