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
    public List<DropItemInfo> dropTable = new List<DropItemInfo>();

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
            StopAction();
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
            rb.velocity = Vector2.left * 1.5f;
        }
        else
        {
            rb.velocity = Vector2.right * 1.5f;
        }
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Hurt");
        hp -= dmg;
        if (hp <= 0)
        {            
            StartCoroutine(Death());
            yield break;
        }
        yield return new WaitForSeconds(0.5f);
        canDamage = true;
        canAct = true;
    }

    public virtual void StopAction()
    {
        StopCoroutine(Attack());
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
        foreach (DropItemInfo item in dropTable)
        {
            if(Random.Range(0f, 100f) <= item.dropChance)
            {
                if(item.itemPrefab != null)
                Instantiate(item.itemPrefab, new Vector3(transform.position.x, transform.position.y + 1), Quaternion.identity);
                break;
            }
        }
    }
}
