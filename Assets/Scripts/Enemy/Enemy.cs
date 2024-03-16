using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int dmg;

    [HideInInspector] public Transform player;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public Material flashMaterial;
    public Material defalutMaterial;
    [HideInInspector] public bool canAct;
    [HideInInspector] public bool canDamage;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAttack;
    public GameObject DamageBox;
    public List<DropItemInfo> dropTable = new List<DropItemInfo>();

    public virtual void OnEnable()
    {
        player = GameManager.Instance.player.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        DamageBox.SetActive(true);
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
        rb.velocity = Vector2.zero;
        if(!isAttack)
        {
            animator.SetTrigger("Hurt");
            animator.SetBool("Hit", true);
        }
        canDamage = false;
        animator.SetInteger("AnimState", 0);

        hp -= dmg;
        if (hp <= 0)
        {
            canAct = false;
            StopAction();
            StartCoroutine(Death());
            yield break;
        }

        if (attackPos.x > transform.position.x)
        {
            rb.velocity = Vector2.left * 1.5f;
        }
        else
        {
            rb.velocity = Vector2.right * 1.5f;
        }
        
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;

        if (animator.GetBool("Hit"))
        {
            animator.SetBool("Hit", false);
        }
        canDamage = true;
    }

    public virtual void StopAction()
    {
        StopCoroutine(Attack());
    }

    public virtual IEnumerator Death()
    {
        DamageBox.SetActive(false);
        animator.SetBool("Hit", false);
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
