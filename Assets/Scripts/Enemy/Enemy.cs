using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IObserver
{
    [SerializeField] public int maxHp;
    public int hp;
    public int dmg;

    [HideInInspector] public Transform player;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [SerializeField] Subject playerSubject;
    public Material flashMaterial;
    public Material defalutMaterial;

    [HideInInspector] public bool canAct;
    [HideInInspector] public bool canDamage;
    public int invinCnt = 0;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAttack;
    public Vector2 originPos;
    public Coroutine act1 = null;
    public Coroutine act2 = null;
    public List<DropItemInfo> dropTable = new List<DropItemInfo>();

    public virtual void OnEnable()
    {
        player = GameManager.Instance.player.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        canAct = false;
        canDamage = true;
        isDead = false;
        transform.position = originPos;
        hp = maxHp;
        playerSubject.AddObsrver(this);
        act1 = StartCoroutine(Think());
    }

    public virtual IEnumerator Think()
    {
        yield return null;
    }

    public virtual IEnumerator Attack()
    {
        yield return null;
    }


    public virtual void Attacked(int dmg, Vector2 attackPos)
    {
        if (canDamage)
        {
            if (invinCnt == 0)
            {
                if (act1 != null)
                {
                    StopCoroutine(act1);
                }
                if (act2 != null)
                {
                    StopCoroutine(act2);
                }
            }

            ObjectPoolManager.instance.GetEffectObject(transform.position, transform.rotation);
            SoundManager.PlaySound(SoundType.HURT, 0.3f, 1);

            StartCoroutine(TakeDamage(dmg, attackPos));
        }
    }

    public virtual IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        rb.velocity = Vector2.zero;
        if (invinCnt==0)
        {
            animator.SetTrigger("Hurt");
            animator.SetBool("Hit", true);
        }
        canDamage = false;
        animator.SetInteger("AnimState", 0);
        spriteRenderer.material = flashMaterial;

        hp -= dmg;

        if (player.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(-2f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(2f, rb.velocity.y);
        }
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;
        yield return new WaitForSeconds(0.3f);
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
        if (invinCnt == 0)
        {
            act1 = StartCoroutine(Think());
            invinCnt = 3;
        }
    }

    public virtual void StopAction(IEnumerator action)
    {
        StopCoroutine(action);
    }

    public virtual IEnumerator Death()
    {
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
            if (Random.Range(0f, 100f) <= item.dropChance)
            {
                if (item.itemPrefab != null)
                    Instantiate(item.itemPrefab, new Vector3(transform.position.x, transform.position.y + 1), Quaternion.identity);
                break;
            }
        }
    }

    public void OnDisable()
    {
        playerSubject.RemoveObserver(this);
    }

    public void OnNotify()
    {
        gameObject.SetActive(false);
    }
}
