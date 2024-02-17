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
    public GameObject[] item;

    public virtual void OnEnable()
    {
        player = GameManager.Instance.player.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canAct = true;
        canDamage = true;
        isDead = false;
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
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Hurt");
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
        ItemDrop();
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    public virtual void ItemDrop()
    {
        if(item != null)
        {
            int num1 = Random.Range(0, 10);
            if (num1 == 1)
            {
                int num2 = Random.Range(0, item.Length);
                Instantiate(item[num2], new Vector3(transform.position.x, transform.position.y + 1), Quaternion.identity);
            }
        }
    }
}
