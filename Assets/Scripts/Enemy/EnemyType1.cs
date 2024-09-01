using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyType1 : Enemy {
    public float moveDirection;
    public float speed;

    private float distanceFromPlayer;
    private float horizental;
    public float viewRange;
    public float attackRange;
    public bool facingRight;
    private bool playerfound;

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
        Check(); //���� üũ
        if (player != null && !GameManager.Instance.isDead) //�÷��̾ ������� ������ �۵�
        {
            horizental = player.position.x - transform.position.x; //�÷��̾� ���� üũ
            distanceFromPlayer = Vector2.Distance(player.position, transform.position); //�÷��̾���� �Ÿ� üũ
            if (distanceFromPlayer < viewRange) //����� �ν� ���� ������ ���
            {
                animator.SetInteger("AnimState", 1);
                FlipToPlayer(horizental);
                if (distanceFromPlayer > attackRange) //����� �Ÿ��� ���ݹ��� ���� ���
                {
                    if (isGround && !isWall && isplatform) //��ü ���� ������ �̵� ������ ���
                    {
                        animator.SetInteger("AnimState", 2);
                        if (isGround && !isWall && !animator.GetBool("Hit") && isplatform) //���� �̵� ������ ��
                            rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else //����� ���ݰŸ� ���� ���
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    act2 = StartCoroutine(Attack()); //���� �ڷ�ƾ ����
                    yield break; //���� �ڷ�ƾ ����
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
        yield return new WaitForSeconds(1f);
        Hit();
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(2f);
        act1 = StartCoroutine(Think());
    }


    public override void Hit() //���� ó��
    {
        Collider2D[] attackBox = Physics2D.OverlapCircleAll(attackPos.position, hitRange, whatIsEnemies);
        for (int i = 0; i < attackBox.Length; i++)
        {
            if (attackBox[i].gameObject.tag == "Player") // �÷��̾ ���� ������ ���� �� ������ ó��
            {
                //GetComponent<TimeStop>().StopTime(0.05f, 10, 0.1f); // �÷��̾� �ǰݽ� �ð� ����       
                GameManager.Instance.PlayerHit(dmg);
            }
        }
    }

}
