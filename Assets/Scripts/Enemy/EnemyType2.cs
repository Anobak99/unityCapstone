using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyType2 : Enemy {
    //movement
    private Vector2 moveDirection;
    private float horizental;
    public float speed;

    //attack
    public GameObject objectPrefab;
    private List<GameObject> pool;

    //playerCheck
    private float distanceFromPlayer;
    public float viewRange;
    public float attackRange;
    public bool facingRight;

    //collisionCheck
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;


    public override void Start()
    {
        base.Start();
        pool = new List<GameObject>();
    }

    void Update()
    {
        Check(); //앞 지형체크

        if (player != null) 
        {
            horizental = player.position.x - transform.position.x;
            distanceFromPlayer = Vector2.Distance(player.position, transform.position);
            if (distanceFromPlayer < viewRange && canAct) //대상이 인식 범위 안쪽일 경우
            {
                FlipToPlayer(horizental);
                if (distanceFromPlayer > attackRange) //대상의 거리가 공격범위 밖일 경우
                {
                    if (!isGround && !isWall) //개체 앞의 지형이 이동 가능한 경우
                    {
                        moveDirection = (player.position - transform.position).normalized * speed;
                        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
                    }
                    else
                        rb.velocity = Vector2.zero;
                }
                else //대상이 공격거리 안일 경우
                {
                    rb.velocity = Vector2.zero;
                    StartCoroutine(Attack());
                }
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
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if(playerPosition > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    public override IEnumerator Attack()
    {
        canAct = false;
        animator.SetTrigger("Attack");
        Debug.Log("Enemy's Attack!");
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(Shot());
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
            select = Instantiate(objectPrefab, transform);
            pool.Add(select);
        }

        bullet = select.GetComponent<EnemyBullet>();
        bullet.target = player.gameObject;
        moveDirection = (player.position - transform.position).normalized * 5f;
        bullet.rb.velocity = moveDirection;

        yield return new WaitForSeconds(3f);

        canAct = true;
    }

    public override IEnumerator TakeDamage(int dmg)
    {
        if(canAct)
        {
            canAct = false;
            animator.SetTrigger("Hurt");
            yield return new WaitForSeconds(0.5f);
            canAct = true;
        }
        hp -= dmg;
        if(hp <= 0 )
        {
            IsDead();
            gameObject.SetActive(false);
        }
    }

}
