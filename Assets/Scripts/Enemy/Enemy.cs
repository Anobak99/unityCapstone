using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int dmg;

    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;
    public bool canAct;

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameManager.Instance.player.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canAct = true;
    }

    public virtual IEnumerator Attack()
    {
        yield return null;
    }

    public virtual void Hit()
    {
    }

    public virtual void Attacked(int dmg, Vector2 attackPos)
    {
        StartCoroutine(TakeDamage(dmg, attackPos));
    }

    public virtual IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        canAct = false;
        if(attackPos.x > transform.position.x)
        {
            Debug.Log("Attack by right");
            rb.velocity = Vector2.left * 1f;
        }
        else
        {
            Debug.Log("Attack by left");
            rb.velocity = Vector2.right * 1f;
        }
        animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(0.5f);
        hp -= dmg;
        if (hp <= 0)
        {
            IsDead();
        }
        canAct = true;
    }

    public virtual void IsDead()
    {
        animator.SetTrigger("Death");
        gameObject.SetActive(false);
    }
}
