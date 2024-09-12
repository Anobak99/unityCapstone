using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHp;
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
    public Coroutine act1 = null;
    public Coroutine act2 = null;
    public List<DropItemInfo> dropTable = new List<DropItemInfo>();

    public virtual void OnEnable()
    {
        player = GameManager.Instance.player.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        DamageBox.SetActive(true);
        canAct = false;
        canDamage = true;
        isDead = false;
        act1 = StartCoroutine(Think());
        hp = maxHp;
    }

    public virtual IEnumerator Think()
    {
        yield return null;
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
            if(act1 != null)
            {
                StopCoroutine(act1);
            }
            if(act2 != null)
            {
                StopCoroutine(act2);
            }
            
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
        
        if (player.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(-2f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(2f, rb.velocity.y);
        }
        
        yield return new WaitForSeconds(0.5f);  
        rb.velocity = Vector2.zero;

        if (animator.GetBool("Hit"))
        {
            animator.SetBool("Hit", false);
        }
        if (hp <= 0)
        {
            canAct = false;
            StartCoroutine(Death());
            yield break;
        }
        canDamage = true;
        act1 = StartCoroutine(Think());
    }

    public virtual void StopAction(IEnumerator action)
    {
        StopCoroutine(action);
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
