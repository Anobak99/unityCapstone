using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHead_move : Enemy
{
    public float moveDirection = 1;
    public float speed;

    private float horizental;
    private float playerDistance;
    public float viewRange;
    public bool facingRight = true;
    private bool playerFound;
    [SerializeField] private GameObject damageBox;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;
    private bool isplatform;

    public override void OnEnable()
    {
        base.OnEnable();

        animator.Play("");
        playerFound = false;
    }

    public override IEnumerator Think()
    {
        Check();

        if (player != null && !GameManager.Instance.isDead)
        {
            horizental = Vector2.Distance(player.transform.position, transform.position);
            if (playerFound) //플레이어를 인식한 상황일 때
            {
                if (isGround && !isWall && isplatform && !animator.GetBool("Hit"))
                {
                    rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    Flip();
                }
            }
            else
            {
                if (horizental < viewRange && player.position.y >= transform.position.y && player.position.y < transform.position.y + 1.5f)
                {
                    playerDistance = Mathf.Abs(horizental);
                    if (playerDistance < viewRange) //대상이 인식 범위 안쪽일 경우
                    {
                        animator.SetTrigger("Awake");
                        yield return new WaitForSeconds(0.5f);
                        playerFound = true;
                        damageBox.SetActive(true);
                    }
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }

            }
        }

        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        damageBox.SetActive(false);
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", true);
        SoundManager.PlaySound(SoundType.HURT, 0.2f, 2);
        canDamage = false;
        spriteRenderer.material = flashMaterial;

        hp -= dmg;

        if (player.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(-2f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(2f, rb.velocity.y);
        }

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
        damageBox.SetActive(true);
        act1 = StartCoroutine(Think());
    }

    private void Check()
    {
        isGround = Physics2D.OverlapBox(groundCheck.position, new Vector2(1f, 0.1f), 0f);
        isWall = Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayor);
        isplatform = Physics2D.OverlapCircle(new Vector2(WallCheck.position.x, WallCheck.position.y - 1f), 0.1f, groundLayor);
    }

    public override IEnumerator Death()
    {
        animator.SetBool("Hit", false);
        animator.SetTrigger("Death");
        canDamage = false;
        isDead = true;
        ItemDrop();
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void Flip()
    {
        moveDirection *= -1;
        transform.Rotate(0, 180, 0);
    }
}
