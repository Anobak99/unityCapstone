using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorm : Enemy
{
    #region Move
    public float moveDirection;
    public float speed;
    private float horizental;
    private float playerDistance;
    public bool facingRight;
    #endregion

    #region Attack
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public GameObject bulletPrefab; // ����ü ������
    private List<GameObject> pool = new List<GameObject>(); // ������ ������Ʈ Ǯ
    public float viewRange;
    public float attackRange;
    [SerializeField] private float bulletSPeeed = 10f;
    #endregion

    #region Checker
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool playerFound;
    private bool isGround;
    private bool isWall;
    #endregion


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
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(3f);
        act1 = StartCoroutine(Think());
    }

    public void ShotFireBall()
    {
        EnemyBullet bullet;
        GameObject select = null;
        Vector2 bulletDirection; // ����ü ����

        foreach (GameObject item in pool)
        {
            if (!item.activeSelf)
            {
                select = item;
                select.transform.position = attackPos.position;
                select.transform.localScale = new Vector3(gameObject.transform.localScale.x, 1, 1);
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(bulletPrefab, new Vector2(attackPos.position.x, attackPos.position.y), Quaternion.identity);
            select.transform.SetParent(null);
            pool.Add(select);
        }

        SoundManager.PlaySound(SoundType.VOLCANO, 0.5f, 9);
        select.transform.localScale = new Vector3(gameObject.transform.localScale.x, 1, 1);
        bullet = select.GetComponent<EnemyBullet>();
        bulletDirection = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg; // ȸ�� ���� ���ϱ� (���� ���� ������ ��ȯ)
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle); // �Ѿ��� ȸ����Ű��
        if (facingRight)
        {
            bullet.rb.velocity = new Vector2(bulletSPeeed, 0);
        }
        else if (!facingRight)
        {
            bullet.rb.velocity = new Vector2(bulletSPeeed * -1, 0);
        }
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", true);
        animator.SetTrigger("Damaged");
        canDamage = false;
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

        yield return new WaitForSeconds(0.5f);
        spriteRenderer.material = defalutMaterial;
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", false);

        if (hp <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Death());
            yield break;
        }

        canDamage = true;
        act1 = StartCoroutine(Think());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }

}
