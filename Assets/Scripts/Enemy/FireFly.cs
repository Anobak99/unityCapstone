using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireFly : Enemy
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
    public LayerMask whatIsEnemies;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isWall;
    private bool isplatform;

    #region ����ü
    public GameObject bulletPrefab; // ����ü ������
    private List<GameObject> pool = new List<GameObject>(); // ������ ������Ʈ Ǯ
    private Vector2 bulletDirection; // ����ü ����
    #endregion

    #region ��Ÿ��
    [SerializeField] private float shotcooldownTime;
    private float nextFireTime;

    public bool IsShotCoolingDown => Time.time < nextFireTime;
    public void StartShotCoolDown() => nextFireTime = Time.time + shotcooldownTime;
    #endregion


    public override IEnumerator Think()
    {
        Check();

        if (player != null && !GameManager.Instance.isDead )
        {
            horizental = player.position.x - transform.position.x;
            // �÷��̾ �ν� ���� ������ ������ ��
            if (horizental < viewRange) 
            {
                FlipToPlayer(horizental);
                playerDistance = Mathf.Abs(horizental);
                // �÷��̾ ���� ���� ���� ��
                if (playerDistance > attackRange) 
                {
                    if (!isWall && !isplatform && !animator.GetBool("Hit")) 
                    {
                        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else if (playerDistance <= attackRange && !IsShotCoolingDown)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    act2 = StartCoroutine(Shot());
                    yield break;
                }
                else if (playerDistance <= attackRange && IsShotCoolingDown)
                {
                    if (!isWall && !isplatform && !animator.GetBool("Hit"))
                    {
                        rb.velocity = new Vector2(moveDirection * speed * -1f, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
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
            }
            else
            {
                if (!isWall && isplatform && !animator.GetBool("Hit")) 
                {
                    rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                }
                else
                {                 
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }

                FlipToPlayer(0f);
            }
        }


        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
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
                select.transform.position = attackPos.position;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(bulletPrefab, attackPos);
            select.transform.SetParent(null);
            pool.Add(select);
        }

        bullet = select.GetComponent<EnemyBullet>();
        bullet.target = player.gameObject;
        bulletDirection = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg; // ȸ�� ���� ���ϱ� (���� ���� ������ ��ȯ)
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle); // �Ѿ��� ȸ����Ű��
        bullet.rb.velocity = bulletDirection * 5f;

        StartShotCoolDown();
        canAct = true;
        act1 = StartCoroutine(Think());

        yield return null;
    }
  
    private void Check()
    {
        isWall = Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayor);
        isplatform = Physics2D.OverlapCircle(new Vector2(WallCheck.position.x, WallCheck.position.y - 0.5f), 0.1f, groundLayor);
    }

    private void FlipToPlayer(float playerPosition)
    {
        if (playerPosition < 0 )
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

    public override IEnumerator Death()
    {
        rb.gravityScale = 1f;
        animator.SetBool("Hit", false);
        animator.SetTrigger("Death");
        canDamage = false;
        isDead = true;
        ItemDrop();
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

    }

}
