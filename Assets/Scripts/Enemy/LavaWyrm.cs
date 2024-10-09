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
    private bool ready;
    private int moveCount;

    public Transform attackPos;
    public LayerMask whatIsEnemies;

    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform LavaCheck;
    private bool isLava;

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
                // 플레이어가 위에 있을 때
                if (player.position.y > this.transform.position.y)
                {
                    if (isLava && !animator.GetBool("Hit")) // 현재 위치가 용암안인 경우
                    {
                        Debug.Log("플레이어가 인식 범위 안이고 위에 있음");
                        rb.velocity = new Vector2(rb.velocity.x, 1f * speed);
                    }
                    else 
                    {
                        Debug.Log("플레이어가 인식 범위 안이고 위에 있으나 현재 위치가 용암이 아니거나 공격중");
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else if (player.position.y + 3f < this.transform.position.y && !IsShotCoolingDown)
                {
                    Debug.Log("플레이어가 인식 범위 안이고 아래에 있음 : 공격");
                    rb.velocity = new Vector2(0, 0);
                    animator.SetTrigger("Attack");
                    act2 = StartCoroutine(Shot());
                }
                else if (IsShotCoolingDown)
                {
                    Debug.Log("공격이 쿨타임중임");
                    rb.velocity = new Vector2(rb.velocity.x, -1f * speed);
                    yield return new WaitForSecondsRealtime(shotcooldownTime);
                }
            }
            else
            {
                Debug.Log("플레이어가 인식 범위 밖에 있음");
                rb.velocity = new Vector2(rb.velocity.x, -1f * speed);
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
        isLava = Physics2D.OverlapCircle(LavaCheck.position, 0.1f, groundLayor);
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
