using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mage_move : Enemy
{
private float horizental;
    private float playerDistance;
    public float viewRange;
    public float attackRange;
    public bool facingRight;
    private bool playerFound;

    [SerializeField] private Enemy_Pool objectPool;
    public GameObject objectPrefab;
    private List<GameObject> bullets = new List<GameObject>();

    public override IEnumerator Think()
    {
        if (player != null && !GameManager.Instance.isDead) //플레이어가 살아있을 때에만 작동
        {
            horizental = Vector2.Distance(player.transform.position, transform.position); //플레이어까지의 x거리
            playerDistance = Mathf.Abs(horizental);
            if (playerDistance < viewRange) //대상이 인식 범위 안쪽일 경우
            {
                FlipToPlayer(horizental);
                if (playerFound) //플레이어를 인식한 상황일 때
                {
                    if (playerDistance > attackRange) //대상의 거리가 공격범위 밖일 경우
                    {
                        act2 = StartCoroutine(Attack2()); //원거리 공격 코루틴 시작
                        yield break; //현재 코루틴 정지
                    }
                    else if(player.position.y >= transform.position.y - 0.5f && player.position.y < transform.position.y + 2.5f)//대상이 공격거리 안일 경우
                    {
                        act2 = StartCoroutine(Attack()); //근접 공격 코루틴 시작
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
            }
        }

        //0.1초마다 다시 호출
        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }


    private void FlipToPlayer(float playerPosition) //플레이어를 향해 방향 전환
    {
        if (playerPosition < 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if (playerPosition > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    public override IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.5f);
        act1 = StartCoroutine(Think());
    }

    public IEnumerator Attack2()
    {
        GameObject fire;
        animator.SetTrigger("cast");
        yield return new WaitForSeconds(0.8f);
        animator.SetTrigger("magic");
        yield return new WaitForSeconds(0.5f);
        fire = objectPool.GetObject(new Vector2(transform.position.x, transform.position.y +0.7f), "Mage");
        Rigidbody2D rigid = fire.GetComponent<Rigidbody2D>();
        Vector2 dirVec = player.transform.position - transform.position;
        rigid.AddForce(dirVec.normalized*5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1.5f);
        act1 = StartCoroutine(Think());
    }


    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", true);
        canDamage = false;
        animator.SetInteger("AnimState", 0);
        spriteRenderer.material = flashMaterial;

        hp -= dmg;

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;
        yield return new WaitForSeconds(0.4f);
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", false);

        if (hp <= 0)
        {
            StartCoroutine(Death());
            yield break;
        }

        canDamage = true;
        act1 = StartCoroutine(Think());
    }
}
