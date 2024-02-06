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
    [HideInInspector] public bool canAct;
    [HideInInspector] public bool canDamage;
    [HideInInspector] public bool isDead;

    // Start is called before the first frame update
    public virtual void OnEnable()
    {
        player = GameManager.Instance.player.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canAct = true;
        canDamage = true;
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
        if (canDamage)
        {
            StartCoroutine(TakeDamage(dmg, attackPos));
        }
    }

    public virtual IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        canAct = false;
        canDamage = false;
        animator.SetInteger("AnimState", 0);
        if (attackPos.x > transform.position.x)
        {
            Debug.Log("Attack by right");
            rb.velocity = Vector2.left * 1.5f;
        }
        else
        {
            Debug.Log("Attack by left");
            rb.velocity = Vector2.right * 1.5f;
        }
        animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        hp -= dmg;
        canDamage = true;
        if (hp <= 0)
        {            
            StartCoroutine(Death());
            yield break;
        }
        yield return new WaitForSeconds(0.5f);
        canAct = true;
    }

    public virtual IEnumerator Death()
    {
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Death");
        canDamage = false;
        isDead = true;
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
