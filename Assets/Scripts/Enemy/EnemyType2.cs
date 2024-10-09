using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyType2 : Enemy {
    //movement
    private Vector2 moveDirection;
    private float horizental;
    public float speed;

    //attack
    public GameObject objectPrefab;
    private List<GameObject> pool = new List<GameObject>();

    //playerCheck
    private float playerDistance;
    public float viewRange;
    public float attackRange;
    public bool facingRight;
    private bool playerFound;

    //collisionCheck
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;


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
                        if (isGround && !isWall && !animator.GetBool("Hit")) //��ü ���� ������ �̵� ������ ���
                        {
                            animator.SetInteger("AnimState", 2);
                            rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
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
                    playerFound = true;
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
    }

    private void FlipToPlayer(float playerPosition)
    {
        if(playerPosition < 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if(playerPosition > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    public override IEnumerator Attack()
    {
        canAct = false;
        animator.SetTrigger("Attack");
        Debug.Log("Enemy's Attack!");
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(Shot());
    }

    private IEnumerator Shot()
    {
        EnemyBullet bullet;
        GameObject select = null;

        foreach (GameObject item in pool)
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
            pool.Add(select);
        }

        bullet = select.GetComponent<EnemyBullet>();
        bullet.target = player.gameObject;
        moveDirection = (player.position - transform.position).normalized * 5f;
        bullet.rb.velocity = moveDirection;

        yield return new WaitForSeconds(3f);

        canAct = true;
    }
}
