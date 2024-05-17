using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class EnemyPatrol : Enemy
{
    [SerializeField] private Transform[] patrolPoints;
    private int currentPoint;
    private float speed;


    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Check(); //앞 지형체크

        //if (!canAct || isDead) return;

        //if (player != null && !GameManager.Instance.isDead)
        //{
        //    horizental = player.position.x - transform.position.x;
        //    distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        //    if (distanceFromPlayer < viewRange && canAct) //대상이 인식 범위 안쪽일 경우
        //    {
        //        animator.SetInteger("AnimState", 1);
        //        FlipToPlayer(horizental);
        //        if (distanceFromPlayer > attackRange) //대상의 거리가 공격범위 밖일 경우
        //        {
        //            if (isGround && !isWall) //개체 앞의 지형이 이동 가능한 경우
        //            {
        //                animator.SetInteger("AnimState", 2);
        //                rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
        //            }
        //            else
        //                rb.velocity = new Vector2(0, rb.velocity.y);
        //        }
        //        else //대상이 공격거리 안일 경우
        //        {
        //            rb.velocity = new Vector2(0, rb.velocity.y);
        //            StartCoroutine(Attack());
        //        }
        //    }
        //    else
        //    {
        //        rb.velocity = new Vector2(0, rb.velocity.y);
        //        animator.SetInteger("AnimState", 0);
        //    }
        //}
    }

    private void Check()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayor);
        isWall = Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayor);
    }
}
