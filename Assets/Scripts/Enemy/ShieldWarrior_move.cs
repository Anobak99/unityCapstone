using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldWarrior_move : Enemy
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
    public float shieldRange;
    public GameObject shield;
    private bool isBlocking = false; // 방패 유무
    private float shieldDirection = 180f; // 방패 막는 방향 (0: 오른쪽, 180: 왼쪽)

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;
    private bool isplatform;

    public Color color;
    public GameObject blood;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hitRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, shieldRange);
    }

    public override IEnumerator Think()
    {
        Check();

        if (player != null && !GameManager.Instance.isDead && ready)
        {
            horizental = player.position.x - transform.position.x;
            if (horizental < viewRange && player.position.y >= transform.position.y && player.position.y < transform.position.y + 1.5f) //대상이 인식 범위 안쪽일 경우
            {
                FlipToPlayer(horizental);
                playerDistance = Mathf.Abs(horizental);
                if (playerDistance < shieldRange && playerDistance > attackRange) // 플레이어가 쉴드 범위 안에 들어올 경우
                {
                    shield.SetActive(true);
                    animator.SetBool("Shield", true);
                    isBlocking = true;
                }
                else if (playerDistance > attackRange) //대상의 거리가 공격범위 밖일 경우
                {
                    shield.SetActive(false);
                    animator.SetBool("Shield", false);
                    isBlocking = false;
                    Debug.Log("실드워리어 : 플레이어 공격 범위 밖");
                    if (isGround && !isWall && !isplatform && !animator.GetBool("Hit")) //개체 앞의 지형이 이동 가능한 경우
                    {
                        Debug.Log("실드워리어 : 플레이어에게 이동");
                        animator.SetInteger("AnimState", 1);
                        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                    }
                    else
                    {
                        Debug.Log("실드워리어 : 앞의 지형이 이동불가지형");
                        animator.SetInteger("AnimState", 0);
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }               
                else if (playerDistance < attackRange) //대상이 공격거리 안일 경우
                {
                    shield.SetActive(false);
                    animator.SetBool("Shield", false);
                    isBlocking = false;

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
            shield.SetActive(false);
            animator.SetBool("Shield", false);
            isBlocking = false;

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
                if (isGround && !isWall && isplatform && !animator.GetBool("Hit")) //개체 앞의 지형이 이동 가능한 경우
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
        if (isBlocking)
        {
            // 방패가 공격을 막을 수 있는 각도인지 검사
            float attackDir = transform.position.x - attackPos.x; // 양수면 왼쪽에서 오는 공격, 음수면 오른쪽에서 오는 공격                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              

            if (shieldDirection == 0 && attackDir < 0)
            {
                Debug.Log("공격이 방패에 막혔습니다!");
                // 공격이 방패에 막힌 경우 데미지를 받지 않음
                act1 = StartCoroutine(Think());
                yield break;
            }
            else if (shieldDirection == 180 && attackDir > 0)
            {
                Debug.Log("공격이 방패에 막혔습니다!");
                // 공격이 방패에 막힌 경우 데미지를 받지 않음
                act1 = StartCoroutine(Think());
                yield break;
            }
        }

        Flash(color);
        Instantiate(blood, new Vector2(transform.position.x, transform.position.y+1f), Quaternion.identity);
        rb.velocity = Vector2.zero;
        ready = true;
        animator.SetBool("Hit", true);
        canDamage = false;
        animator.SetInteger("AnimState", 0);

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
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", false);

        if (hp <= 0)
        {
            canAct = false;
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
            shieldDirection = 0;
            transform.Rotate(0, 180, 0);
        }
        else if (moveDirection < 0 && facingRight)
        {
            facingRight = false;
            shieldDirection = 180;
            transform.Rotate(0, 180, 0);
        }
    }

    public override IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        Hit();
        yield return new WaitForSeconds(2f);
        act1 = StartCoroutine(Think());
    }


    public override void Hit()
    {
        Collider2D[] attackBox = Physics2D.OverlapCircleAll(attackPos.position, hitRange, whatIsEnemies);
        for (int i = 0; i < attackBox.Length; i++)
        {
            if (attackBox[i].gameObject.tag == "Player") // 플레이어 충돌 시 데미지 처리
            {
                //GetComponent<TimeStop>().StopTime(0.05f, 10, 0.1f); // 플레이어 피격시 시간 정지       
                GameManager.Instance.PlayerHit(dmg);
            }
        }
    }
 


}
