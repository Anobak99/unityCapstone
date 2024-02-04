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

    public virtual void Attacked(int dmg)
    {
        StartCoroutine(TakeDamage(dmg));
    }

    public virtual IEnumerator TakeDamage(int dmg)
    {
        if (canAct)
        {
            canAct = false;
            animator.SetTrigger("Hurt");
            yield return new WaitForSeconds(0.5f);
            canAct = true;
        }
        hp -= dmg;
        if (hp <= 0)
        {
            IsDead();
        }
    }

    public virtual void IsDead()
    {
        animator.SetTrigger("Death");
        gameObject.SetActive(false);
    }
}
