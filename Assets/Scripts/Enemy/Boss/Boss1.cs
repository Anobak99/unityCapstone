using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boss1 : Boss
{
    private float moveDirection = -1;
    private bool facingRight = false;
    [SerializeField] private float speed;
    private float moveTime;

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
    private bool isAttack3;

    public GameObject objectPrefab;
    private List<GameObject> bullets = new List<GameObject>();

    [SerializeField] private Transform attackPos;
    [SerializeField] private Transform attack3Pos1;
    [SerializeField] private Transform attack3Pos2;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private float hitRange;


    private void Start()
    {
        StartCoroutine(Think(3f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Check();

        if (GameManager.Instance.gameState != GameManager.GameState.Boss ||isDead || Time.timeScale == 0) return;

        if (canAct && player != null && !GameManager.Instance.isDead)
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
                        moveTime -= Time.deltaTime;
                        if(moveTime < 0) StartCoroutine(Attack2());
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
                if(isAttack3)
                {
                    rb.gravityScale = 1;
                    StartCoroutine(Attack3Hit());
                }
                else
                {
                    rb.gravityScale = 1;
                    Attack2Hit();
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
        moveTime = 3f;
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

        for (int i = 0; i < 4; i++)
        {
            Shoot(isAttack3);
        }

        attackCount++;
        StartCoroutine(Think(2f));
    }

    IEnumerator Attack3()
    {
        float downPos;

        canAct = false;
        isAttack3 = true;

        if (horizental < 0) //반대 벽으로 점프
        {
            downPos = attack3Pos2.position.x;
        }
        else
        {
            downPos = attack3Pos1.position.x;
        }

        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector2(downPos, jumpHeight);
        yield return new WaitForSeconds(0.5f);
        isJump = true;
        rb.velocity = new Vector2(downPos, 0f);
        animator.SetTrigger("JumpAttack");
    }

    IEnumerator Attack3Hit()
    {
        isJump = false;
        animator.SetTrigger("JumpEnd");
        yield return new WaitForSeconds(0.5f);
        horizental = player.position.x - transform.position.x;
        FlipToPlayer(horizental);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attack3", true);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 10; i++)
        {
            horizental = player.position.x - transform.position.x;
            Shoot(isAttack3);
            yield return new WaitForSeconds(0.5f);
        }

        isAttack3 = false;
        animator.SetBool("Attack3", false);
        attackCount = 0;
        StartCoroutine(Think(2f));
    }

    private void Shoot(bool isAttack3)
    {
        Boss1_bullet b_script;
        GameObject select = null;

        foreach (GameObject item in bullets)
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
            bullets.Add(select);
        }

        b_script = select.GetComponent<Boss1_bullet>();

        if (isAttack3)
        {
            b_script.isAttack3 = true;
            b_script.downPoint = horizental;
        }

        StartCoroutine(b_script.Jump());
    }

    IEnumerator Stuuned()
    {
        yield return new WaitForSeconds(3f);
    }

    private void StopAction()
    {
        StopCoroutine(Attack1());
        StopCoroutine(Attack2());
        StopCoroutine(Attack3());
        StopCoroutine(Attack3Hit());
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        canDamage = false;
        spriteRenderer.color = Color.red;
        hp -= dmg;
        if (hp <= 0)
        {
            StopAction();
            spriteRenderer.color = Color.white;
            StartCoroutine(Death());
            yield break;
        }
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
        canDamage = true;
    }

    private IEnumerator Death()
    {
        rb.velocity = Vector2.zero;
        //animator.SetTrigger("Death");
        canDamage = false;
        isDead = true;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, attackRange2);
    }
}
