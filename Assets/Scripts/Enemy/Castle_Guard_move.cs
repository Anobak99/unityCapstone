using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Guard_move : Enemy
{
    public float moveDirection;
    public float speed;

    private float horizental;
    private float playerDistance;
    public float viewRange;
    public bool facingRight;
    public float attackRange;
    [SerializeField] private GameObject damageBox;

    private bool isMove;
    private int moveCnt;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;
    private bool isplatform;

    public override void OnEnable()
    {
        base.OnEnable();

        canAct = true;
        invinCnt = 1;
        animator.Play("Castle_guard_idle");
    }

    public void FixedUpdate()
    {
        if (isMove)
        {
            rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
            if (!isGround || isWall || !isplatform || moveCnt == 10)
            {
                rb.velocity = Vector2.zero;
                isMove = false;
                act2 = StartCoroutine(Attack2());
            }
        }
    }

    public override IEnumerator Think()
    {
        Check();

        if (player != null && !GameManager.Instance.isDead && canAct)
        {
            if ((facingRight && player.position.x > transform.position.x) || (!facingRight && player.position.x < transform.position.x))
            {
                if (playerDistance < attackRange && player.position.y < transform.position.y + 2f && player.position.y > transform.position.y - 0.2f) //����� �Ÿ��� ���ݹ��� ���� ���
                {
                    act2 = StartCoroutine(Attack1()); //���� �ڷ�ƾ ����
                    yield break; //���� �ڷ�ƾ ����
                }
            }
        }

        if (isMove)
        {
            moveCnt++;
        }

        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }

    IEnumerator Attack1() //������ ����
    {
        canAct = false;
        animator.SetTrigger("ready");
        yield return new WaitForSeconds(1f);
        isMove = true;
        animator.SetTrigger("attack");
        act1 = StartCoroutine(Think());
    }

    IEnumerator Attack2()
    {
        moveCnt = 0;
        animator.SetTrigger("return");
        Flip();
        yield return new WaitForSeconds(2f);
        invinCnt--;
        canAct = true;
        act1 = StartCoroutine(Think());
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        rb.velocity = Vector2.zero;
        SoundManager.PlaySound(SoundType.HURT, 0.2f, 2);
        canDamage = false;
        spriteRenderer.material = flashMaterial;

        hp -= dmg;

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;
        yield return new WaitForSeconds(0.4f);

        if (hp <= 0)
        {
            damageBox.SetActive(false);
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
        animator.SetTrigger("death");
        canDamage = false;
        isDead = true;
        ItemDrop();
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void Flip()
    {
        horizental = player.position.x - transform.position.x;
        moveDirection *= -1;
        if (horizental < 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if (horizental > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }
}
