using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyType1 : Enemy {
    public float moveDirection;
    public float speed;

    private float horizental;
    private float playerDistance;
    public float viewRange;
    public float attackRange;
    public bool facingRight;
    private bool playerFound;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float hitRange;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;
    private bool isplatform;

    public override void OnEnable()
    {
        base.OnEnable();
        playerFound = false;
        animator.Play("Drago_idle");
    }

    public override IEnumerator Think()
    {
        Check(); //���� üũ
        if (player != null && !GameManager.Instance.isDead) //�÷��̾ ������� ������ �۵�
        {
            horizental = player.position.x - transform.position.x; //�÷��̾������ x�Ÿ�
            playerDistance = Mathf.Abs(horizental);
            if (playerDistance < viewRange && player.position.y >= transform.position.y - 2.5f && player.position.y < transform.position.y + 2.5f) //����� �ν� ���� ������ ���
            {
                FlipToPlayer(horizental);
                if (playerFound) //�÷��̾ �ν��� ��Ȳ�� ��
                {
                    if (playerDistance > attackRange) //����� �Ÿ��� ���ݹ��� ���� ���
                    {
                        if (isGround && !isWall && isplatform && !animator.GetBool("Hit")) //��ü ���� ������ �̵� ������ ���
                        {
                            animator.SetInteger("AnimState", 2);
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
                        animator.SetInteger("AnimState", 0);
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        act2 = StartCoroutine(Attack()); //���� �ڷ�ƾ ����
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
                animator.SetInteger("AnimState", 0);
            }
        }

        //0.1�ʸ��� �ٽ� ȣ��
        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }

    private void Check()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayor);
        isWall = Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayor);
        isplatform = Physics2D.OverlapCircle(new Vector2(WallCheck.position.x, WallCheck.position.y - 1f), 0.1f, groundLayor);
    }

    private void FlipToPlayer(float playerPosition) //�÷��̾ ���� ���� ��ȯ
    {
        if(playerPosition < 0 && facingRight)
        {
            moveDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if(playerPosition > 0 && !facingRight)
        {
            moveDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    public override IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(1f);
        act1 = StartCoroutine(Think());
    }

}
