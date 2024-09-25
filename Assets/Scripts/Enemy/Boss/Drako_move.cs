using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drako_move : Boss
{
    private float moveDirection = 1;
    private bool facingRight = true;
    [SerializeField] private float speed;
    private float moveTime;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;
    private bool isMove;

    private float distanceFromPlayer;
    private float horizental;

    private int attackCount; //공격횟수
    private int attackCount2;
    private int countMax;
    [SerializeField] private float attackRange; //공격1 범위
    [SerializeField] private float attackRange2; //공격2 범위
    [SerializeField] private float jumpHeight;

    public GameObject objectPrefab;
    private List<GameObject> bullets = new List<GameObject>();
    [SerializeField] private BossBattle battle;

    public ParticleSystem dust;

    public override IEnumerator Think()
    {
        Check();

        if (GameManager.Instance.gameState != GameManager.GameState.Boss || isDead || Time.timeScale == 0) yield return null;

        if (canAct && player != null && !GameManager.Instance.isDead)
        {
            distanceFromPlayer = Vector2.Distance(player.position, transform.position);
            horizental = player.position.x - transform.position.x;
            FlipToPlayer(horizental);

            if (attackCount2 < 3)
            {
                if (attackCount != countMax)
                {
                    animator.SetInteger("AnimState", 0);
                    if (distanceFromPlayer < attackRange)
                    {
                        isMove = false;
                        StartCoroutine(Attack1());
                        yield break;
                    }
                    else if (distanceFromPlayer > attackRange2)
                    {
                        StartCoroutine(Attack2());
                        yield break;
                    }
                    else
                    {
                        if (isGround && !isWall)
                        {
                            isMove = true;
                            animator.SetInteger("AnimState", 2);
                            moveTime -= Time.deltaTime;
                            if (moveTime < 0)
                            {
                                StartCoroutine(Attack2());
                                yield break;
                            }
                        }
                        else
                        {
                            isMove = false;
                            rb.velocity = new Vector2(0, rb.velocity.y);
                        }
                    }
                }
                else
                {
                    StartCoroutine(Attack2());
                    attackCount = 0;
                    yield break;
                }
            }
            else
            {
                StartCoroutine(Attack3());
                yield break;
            }
        }

        if(isMove)
        {
            if(animator.GetInteger("AnimState") == 2 && isGround && !isWall)
            {
                FlipToPlayer(horizental);
                rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
            }
            else if(animator.GetInteger("AnimState") == 1)
            {
                dust.Play();

                if (isGround && !isWall)
                {
                    rb.velocity = new Vector2(moveDirection * speed * 3, rb.velocity.y);
                }
                else if(isGround && isWall)
                {
                    isMove = false;
                    StartCoroutine(Attack2Hit());
                    yield break;
                }
            }
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Think());
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

    IEnumerator Wait(float time)
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(time);
        moveTime = 2f;
        canAct = true;
        StartCoroutine(Think());
    }

    IEnumerator Attack1() //근접공격
    {
        canAct = false;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(1f);
        attackCount++;
        StartCoroutine(Wait(1.5f));
    }

    IEnumerator Attack2() //적에게 돌진
    {
        canAct = false;
        speed = 5f;
        FlipToPlayer(horizental);
        animator.SetTrigger("Ready");
        yield return new WaitForSeconds(0.5f);
        isMove = true;
        animator.SetInteger("AnimState", 1);
        StartCoroutine(Think());
    }

    IEnumerator Attack2Hit() //벽에 부딪힐 시 밀려남
    {
        speed = 3f;
        animator.SetBool("Hit", true);
        animator.SetInteger("AnimState", 0);
        rb.velocity = new Vector2(-1 * moveDirection * speed, jumpHeight);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Hit", false);
        countMax = Random.Range(2, 5);
        attackCount2++;
        StartCoroutine(Wait(1f));
    }

    IEnumerator Attack3() //제자리에서 포격 준비
    {
        isMove = false;
        canAct = false;

        animator.SetBool("Attack2_1", true);
        animator.Play("Drako_Attack2");
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Attack3Hit());
    }

    IEnumerator Attack3Hit() //플레이어 머리 위로 5회 불꽃 발사
    {
        GameObject fire;

        horizental = player.position.x - transform.position.x;
        FlipToPlayer(horizental);

        for (int i = 0; i < 5; i++)
        {
            animator.SetTrigger("Attack2_2");
            yield return new WaitForSeconds(0.3f);
            horizental = player.position.x - transform.position.x;
            fire = Shoot();
            fire.transform.position = new Vector2(player.position.x, 8f);
            yield return new WaitForSeconds(0.3f);
        }

        animator.SetBool("Attack2_1", false);
        animator.Play("Drako_Attack2_3");
        attackCount2 = 0;
        StartCoroutine(Wait(2f));
    }

    private GameObject Shoot() //총알 생성, 생성된 오브젝트 재활용 및 없을 시 생성
    {
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

        return select;
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        canDamage = false;
        spriteRenderer.material = flashMaterial;
        hp -= dmg;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;
        if (hp <= 0)
        {
            StartCoroutine(Death());
            yield break;
        }
        canDamage = true;
    }

    private IEnumerator Death()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", true);
        canDamage = false;
        isDead = true;
        yield return new WaitForSeconds(1f);
        battle.BossDead();
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, attackRange2);
    }
}
