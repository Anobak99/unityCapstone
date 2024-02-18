using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boss1 : Boss
{
    private float moveDirection = -1;
    private bool facingRight = false;
    [SerializeField] private float speed;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;

    private float distanceFromPlayer;
    private float horizental;

    public int attackCount; //공격횟수
    [SerializeField] private float attackRange; //공격1 범위
    [SerializeField] private float attackRange2; //공격2 범위
    [SerializeField] private float jumpHeight;
    private bool isJump;

    [SerializeField] private Transform attackPos;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private float hitRange;
    [SerializeField] private GameObject[] Rocks;


    private void Start()
    {
        StartCoroutine(Think(3f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Check();

        if(canAct && !isDead)
        {
            distanceFromPlayer = Vector2.Distance(player.position, transform.position);
            horizental = player.position.x - transform.position.x;
            FlipToPlayer(horizental);
            if (attackCount != 5)
            {
                animator.SetInteger("AnimState", 0);
                if (distanceFromPlayer < attackRange)
                {
                    StartCoroutine(Attack1());
                }
                else if(distanceFromPlayer > attackRange2)
                {
                    StartCoroutine(Attack2());
                }
                else
                {
                    if (isGround && !isWall)
                    {
                        animator.SetInteger("AnimState", 2);
                        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                    }
                    else
                        rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            else
            {
                StartCoroutine(Attack3());
            }
        }

        if(isJump)
        {
             rb.gravityScale = 10;

            if (isGround)
            {
                rb.gravityScale = 1;
                Attack2Hit();
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

    IEnumerator Think(float time)
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(time);
        canAct = true;
    }

    IEnumerator Attack1() //근접공격
    {
        canAct = false;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(1);
        Hit();
        attackCount++;
        StartCoroutine(Think(1.5f));
    }

    public void Hit()
    {
        Collider2D[] attackBox = Physics2D.OverlapCircleAll(attackPos.position, hitRange, whatIsEnemies);
        for (int i = 0; i < attackBox.Length; i++)
        {
            if (attackBox[i].gameObject.tag == "Player")
            {     
                GameManager.Instance.PlayerHit(dmg);
            }
        }
    }

    IEnumerator Attack2() //적에게 점프
    {
        canAct = false;
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector2(horizental, jumpHeight);
        yield return new WaitForSeconds(0.5f);
        isJump = true;
        rb.velocity = new Vector2(horizental, 0f);
        animator.SetTrigger("JumpAttack");
    }

    void Attack2Hit()
    {
        isJump = false;
        animator.SetTrigger("JumpEnd");

        //for (int i = 0; i < Rocks.Length; i++)
        //{
        //    Rocks[i].SetActive(true);
        //    Boss1_bullet bullet = Rocks[i].GetComponent<Boss1_bullet>();
        //    bullet.distanceFromPlayer = transform.position.x + Random.Range(-2f, 2f);
        //    bullet.Jump();
        //}

        attackCount++;
        StartCoroutine(Think(2f));
    }

    IEnumerator Attack3()
    {
        canAct = false;
        attackCount = 0;
        yield return null;
        StartCoroutine(Think(6f));
    }

    IEnumerator Stuuned()
    {
        yield return new WaitForSeconds(3f);
    }

    public virtual void StopAction()
    {
        StopCoroutine(Attack1());
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        canDamage = false;
        spriteRenderer.color = Color.red;
        hp -= dmg;
        if (hp <= 0)
        {
            spriteRenderer.color = Color.white;
            StartCoroutine(Death());
            yield break;
        }
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
        canDamage = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, attackRange2);
    }
}
