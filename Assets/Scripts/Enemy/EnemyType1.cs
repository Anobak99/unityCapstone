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
        Check(); //지형 체크
        if (player != null && !GameManager.Instance.isDead) //플레이어가 살아있을 때에만 작동
        {
            horizental = player.position.x - transform.position.x; //플레이어 방향 체크
            distanceFromPlayer = Vector2.Distance(player.position, transform.position); //플레이어와의 거리 체크
            if (distanceFromPlayer < viewRange) //대상이 인식 범위 안쪽일 경우
            {
                animator.SetInteger("AnimState", 1);
                FlipToPlayer(horizental);
                if (distanceFromPlayer > attackRange) //대상의 거리가 공격범위 밖일 경우
                {
                    if (isGround && !isWall && isplatform) //개체 앞의 지형이 이동 가능한 경우
                    {
                        animator.SetInteger("AnimState", 2);
                        if (isGround && !isWall && !animator.GetBool("Hit") && isplatform) //앞이 이동 가능할 때
                            rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else //대상이 공격거리 안일 경우
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    act2 = StartCoroutine(Attack()); //공격 코루틴 시작
                    yield break; //현재 코루틴 정지
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
        yield return new WaitForSeconds(1f);
        Hit();
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(2f);
        act1 = StartCoroutine(Think());
    }


    public override void Hit() //공격 처리
    {
        Collider2D[] attackBox = Physics2D.OverlapCircleAll(attackPos.position, hitRange, whatIsEnemies);
        for (int i = 0; i < attackBox.Length; i++)
        {
            if (attackBox[i].gameObject.tag == "Player") // 플레이어가 공격 범위에 존재 시 데미지 처리
            {
                //GetComponent<TimeStop>().StopTime(0.05f, 10, 0.1f); // 플레이어 피격시 시간 정지       
                GameManager.Instance.PlayerHit(dmg);
            }
        }
    }

}
