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
    public GameObject bulletPrefab; // 투사체 프리팹
    private List<GameObject> pool = new List<GameObject>(); // 프리팹 오브젝트 풀
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
        Check(); //지형 체크
        if (player != null && !GameManager.Instance.isDead) //플레이어가 살아있을 때에만 작동
        {
            horizental = player.position.x - transform.position.x; //플레이어까지의 x거리
            playerDistance = Mathf.Abs(horizental);
            if (playerDistance < viewRange && player.position.y >= transform.position.y - 2.5f && player.position.y < transform.position.y + 2.5f) //대상이 인식 범위 안쪽일 경우
            {
                FlipToPlayer(horizental);
                if (playerFound) //플레이어를 인식한 상황일 때
                {
                    if (playerDistance > attackRange) //대상의 거리가 공격범위 밖일 경우
                    {
                        if (isGround && !isWall && !animator.GetBool("Hit")) //개체 앞의 지형이 이동 가능한 경우
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
                    else //대상이 공격거리 안일 경우
                    {
                        animator.SetInteger("AnimState", 0);
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        act2 = StartCoroutine(Attack()); //공격 코루틴 시작
                        yield break; //현재 코루틴 정지
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
            else //대상을 찾지 못했을 때
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetInteger("AnimState", 0);
            }
        }

        //0.1초마다 다시 호출
        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }

    private void Check()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayor);
        isWall = Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayor);
    }

    private void FlipToPlayer(float playerPosition) //플레이어를 향해 방향 전환
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
        Vector2 bulletDirection; // 투사체 방향

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
        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg; // 회전 각도 구하기 (라디안 값을 각도로 변환)
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle); // 총알을 회전시키기
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
