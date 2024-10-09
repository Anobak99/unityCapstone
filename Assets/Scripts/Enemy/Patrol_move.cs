using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_move : Enemy
{
    [SerializeField] private float moveTime;
    [SerializeField] private float speed;
    private bool canMove;
    [SerializeField] private bool facingRight;
    [SerializeField] private GameObject damageBox;

    public enum Move_Direction
    {
        left, right, up, down
    }
    public Move_Direction current_move;

    private void Update()
    {
       if(canMove)
        {
            switch (current_move)
            {
                case Move_Direction.left:
                    rb.velocity = Vector2.left * speed;
                    break;
                case Move_Direction.right:
                    rb.velocity = Vector2.right * speed;
                    break;
                case Move_Direction.up:
                    rb.velocity = Vector2.up * speed;
                    break;
                case Move_Direction.down:
                    rb.velocity = Vector2.down * speed;
                    break;
                default:
                    break;
            }
        }
       else
            rb.velocity = Vector2.zero;
    }

    public override IEnumerator Think()
    {
        canMove = false;
        yield return new WaitForSeconds(0.5f);
        Flip();
        switch (current_move)
        {
            case Move_Direction.left:
                current_move = Move_Direction.right;
                break;
            case Move_Direction.right:
                current_move = Move_Direction.left;
                break;
            case Move_Direction.up:
                current_move = Move_Direction.down;
                break;
            case Move_Direction.down:
                current_move = Move_Direction.up;
                break;
            default:
                break;
        }
        canMove = true;
        //0.1초마다 다시 호출
        yield return new WaitForSeconds(moveTime);
        act1 = StartCoroutine(Think());
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        canDamage = false;
        spriteRenderer.material = flashMaterial;

        hp -= dmg;

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;
        yield return new WaitForSeconds(0.4f);

        if (hp <= 0)
        {
            StartCoroutine(Death());
            yield break;
        }

        canDamage = true;
        act1 = StartCoroutine(Think());
    }

    public override IEnumerator Death()
    {
        animator.SetBool("Hit", false);
        animator.SetTrigger("Death");
        canDamage = false;
        isDead = true;
        damageBox.SetActive(false);
        ItemDrop();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
