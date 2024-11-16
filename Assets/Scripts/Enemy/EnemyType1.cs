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
                        if (isGround && !isWall && isplatform && !animator.GetBool("Hit")) //개체 앞의 지형이 이동 가능한 경우
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
        isplatform = Physics2D.OverlapCircle(new Vector2(WallCheck.position.x, WallCheck.position.y - 1f), 0.1f, groundLayor);
    }

    private void FlipToPlayer(float playerPosition) //플레이어를 향해 방향 전환
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
