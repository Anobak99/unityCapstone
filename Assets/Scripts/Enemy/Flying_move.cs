using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Flying_move : Enemy
{
    //movement
    private Vector2 moveDirection;
    private float moveX;
    private float moveY;
    private float horizental;
    [SerializeField] private float speed;

    //playerCheck
    private float playerDistance;
    [SerializeField] private float viewRange;
    [SerializeField] private bool facingRight;
    private bool playerFound;

    [SerializeField] private GameObject damageBox;

    public override void OnEnable()
    {
        base.OnEnable();

        animator.Play("Cloud_eye_inactive");
        playerFound = false;
    }

    public override IEnumerator Think()
    {
        if (player != null && !GameManager.Instance.isDead) //�÷��̾ ������� ������ �۵�
        {
            horizental = Vector2.Distance(player.transform.position, transform.position); //�÷��̾������ �Ÿ�
            moveDirection = (player.transform.position - transform.position);
            if(moveDirection.x >= 0) { moveX = 1; } else { moveX = -1; }
            if(moveDirection.y >= 0) { moveY = 1; } else { moveY = -1; }
            playerDistance = Mathf.Abs(horizental);
            if (playerFound) //�÷��̾ �ν��� ��Ȳ�� ��
            {
                FlipToPlayer(horizental);
                rb.velocity = new Vector2(moveX * speed, (moveY+0.5f) * speed);
            }
            else
            {
                if (playerDistance < viewRange) //����� �ν� ���� ������ ���
                {
                    animator.SetTrigger("Awake");
                    yield return new WaitForSeconds(0.5f);
                    playerFound = true;
                }
            }
        }

        //0.1�ʸ��� �ٽ� ȣ��
        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }

    private void FlipToPlayer(float playerPosition)
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

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        rb.velocity = Vector2.zero;
        canDamage = false;
        spriteRenderer.material = flashMaterial;

        hp -= dmg;

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;

        if (hp <= 0)
        {
            damageBox.SetActive(false);
            StartCoroutine(Death());
            yield break;
        }

        canDamage = true;
        act1 = StartCoroutine(Think());
    }

    public override IEnumerator Death()
    {
        animator.SetTrigger("Death");
        canDamage = false;
        isDead = true;
        ItemDrop();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            StartCoroutine(Death());
        }
    }
}
