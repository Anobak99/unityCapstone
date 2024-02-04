using System.Collections;
using System.Collections.Generic;
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

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float hitRange;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;


    //public override void Start()
    //{
    //    base.Start();
    //}

    void Update()
    {
        Check(); //앞 지형체크

        if(player != null)
        {
            horizental = player.position.x - transform.position.x;
            distanceFromPlayer = Vector2.Distance(player.position, transform.position);
            if (distanceFromPlayer < viewRange && canAct) //대상이 인식 범위 안쪽일 경우
            {
                animator.SetInteger("AnimState", 1);
                FlipToPlayer(horizental);
                if (distanceFromPlayer > attackRange) //대상의 거리가 공격범위 밖일 경우
                {
                    if (isGround && !isWall) //개체 앞의 지형이 이동 가능한 경우
                    {
                        animator.SetInteger("AnimState", 2);
                        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                    }
                    else
                        rb.velocity = new Vector2(0, rb.velocity.y);
                }
                else //대상이 공격거리 안일 경우
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    StartCoroutine(Attack());
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetInteger("AnimState", 0);
            }
        }


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
        canAct = false;
        animator.SetTrigger("Attack");
        Debug.Log("Enemy's Attack!");
        yield return new WaitForSeconds(3f);
        canAct = true;
    }

    public override void Hit()
    {
        Collider2D[] attackBox = Physics2D.OverlapCircleAll(attackPos.position, hitRange, whatIsEnemies);
        for (int i = 0; i < attackBox.Length; i++)
        {
            if ( attackBox[i].gameObject.tag == "Player") // 플레이어 충돌 시 데미지 처리
            {
                GameManager.Instance.PlayerHit(dmg);
            }
        }
    }
}
