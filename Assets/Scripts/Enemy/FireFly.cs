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
    private bool ready;
    private int moveCount;

    public Transform attackPos;
    public LayerMask whatIsEnemies;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isWall;
    private bool isplatform;

    #region 투사체
    public GameObject bulletPrefab; // 투사체 프리팹
    private List<GameObject> pool = new List<GameObject>(); // 프리팹 오브젝트 풀
    private Vector2 bulletDirection; // 투사체 방향
    #endregion

    #region 쿨타임
    [SerializeField] private float shotcooldownTime;
    private float nextFireTime;

    public bool IsShotCoolingDown => Time.time < nextFireTime;
    public void StartShotCoolDown() => nextFireTime = Time.time + shotcooldownTime;
    #endregion




    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

    }


    public override IEnumerator Think()
    {
        Check();

        if (player != null && !GameManager.Instance.isDead && ready)
        {
            horizental = player.position.x - transform.position.x;
            // 플레이어가 인식 범위 안으로 들어왔을 때
            if (horizental < viewRange) 
            {
                FlipToPlayer(horizental);
                playerDistance = Mathf.Abs(horizental);
                // 플레이어가 공격 범위 밖일 때
                if (playerDistance > attackRange) 
                {
                    if (!isWall && !isplatform && !animator.GetBool("Hit")) 
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
                else if (playerDistance <= attackRange && !IsShotCoolingDown)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    animator.SetInteger("AnimState", 0);
                    act2 = StartCoroutine(Shot());
                    yield break;
                }
                else if (playerDistance <= attackRange && IsShotCoolingDown)
                {
                    if (!isWall && !isplatform && !animator.GetBool("Hit"))
                    {
                        animator.SetInteger("AnimState", 1);
                        rb.velocity = new Vector2(moveDirection * speed * -1f, rb.velocity.y);
                    }
                    else
                    {
                        animator.SetInteger("AnimState", 0);
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
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
                if (!isWall && isplatform && !animator.GetBool("Hit")) 
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
            select = Instantiate(bulletPrefab, attackPos);
            pool.Add(select);
        }

        bullet = select.GetComponent<EnemyBullet>();
        bullet.target = player.gameObject;
        bulletDirection = (player.position - transform.position).normalized * 5f;
        bullet.rb.velocity = bulletDirection;

        StartShotCoolDown();
        canAct = true;
        act1 = StartCoroutine(Think());

        yield return null;
    }
}
