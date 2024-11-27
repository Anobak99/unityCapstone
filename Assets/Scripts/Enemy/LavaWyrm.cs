using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LavaWyrm : Enemy
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

    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform LavaCheck;
    private bool isLava;

    #region 투사체
    [SerializeField] private Enemy_Pool objectPool;
    public GameObject objectPrefab;
    private List<GameObject> bullets = new List<GameObject>();
    private Vector2 bulletDirection; // 투사체 방향
    [SerializeField] private float bulletSPeeed = 10f;
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

        if (player != null && !GameManager.Instance.isDead)
        {
            horizental = player.position.x - transform.position.x;
            playerDistance = Mathf.Abs(horizental);
            // 플레이어가 인식 범위 안으로 들어왔을 때
            if (playerDistance < viewRange)
            {
                FlipToPlayer(horizental);
                // 플레이어가 위에 있을 때
                if (player.position.y > this.transform.position.y)
                {
                    if (isLava && !animator.GetBool("Hit")) // 현재 위치가 용암안인 경우
                    {
                        rb.velocity = new Vector2(rb.velocity.x, 1f * speed);
                    }
                    else 
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else if (player.position.y + 3f < this.transform.position.y && !IsShotCoolingDown)
                {
                    rb.velocity = new Vector2(0, 0);
                    animator.SetTrigger("Attack");
                    act2 = StartCoroutine(Shot());
                }
                else if (IsShotCoolingDown)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -1f * speed);
                    yield return new WaitForSecondsRealtime(shotcooldownTime);
                }
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -1f * speed);
            }
        }
       
        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Hit");
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

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;
        yield return new WaitForSeconds(0.4f);
        rb.velocity = Vector2.zero;

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
        isLava = Physics2D.OverlapCircle(LavaCheck.position, 0.1f, groundLayor);
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

    private IEnumerator Shot()
    {
        EnemyBullet bullet;
        GameObject select = null;

        //foreach (GameObject item in pool)
        //{
        //    if (!item.activeSelf)
        //    {
        //        select = item;
        //        select.transform.position = attackPos.position;
        //        select.SetActive(true);
        //        break;
        //    }
        //}

        //if (!select)
        //{
        //    select = Instantiate(bulletPrefab, attackPos);
        //    select.transform.SetParent(null);
        //    pool.Add(select);
        //}

        select = objectPool.GetObject(new Vector2(transform.position.x, transform.position.y + 0.7f), "FireFly");
        bullet = select.GetComponent<EnemyBullet>();
        bullet.target = player.gameObject;
        bulletDirection = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg; // 회전 각도 구하기 (라디안 값을 각도로 변환)
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle); // 총알을 회전시키기
        bullet.rb.velocity = bulletDirection * bulletSPeeed;

        StartShotCoolDown();
        canAct = true;
        act1 = StartCoroutine(Think());

        yield return null;
    }
}
