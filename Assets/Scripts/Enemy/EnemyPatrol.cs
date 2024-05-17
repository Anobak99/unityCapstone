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
        Check(); //�� ����üũ

        //if (!canAct || isDead) return;

        //if (player != null && !GameManager.Instance.isDead)
        //{
        //    horizental = player.position.x - transform.position.x;
        //    distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        //    if (distanceFromPlayer < viewRange && canAct) //����� �ν� ���� ������ ���
        //    {
        //        animator.SetInteger("AnimState", 1);
        //        FlipToPlayer(horizental);
        //        if (distanceFromPlayer > attackRange) //����� �Ÿ��� ���ݹ��� ���� ���
        //        {
        //            if (isGround && !isWall) //��ü ���� ������ �̵� ������ ���
        //            {
        //                animator.SetInteger("AnimState", 2);
        //                rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
        //            }
        //            else
        //                rb.velocity = new Vector2(0, rb.velocity.y);
        //        }
        //        else //����� ���ݰŸ� ���� ���
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
