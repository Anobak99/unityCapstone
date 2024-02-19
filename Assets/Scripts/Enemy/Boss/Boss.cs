using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int hp;
    public int dmg;

    [HideInInspector] public Transform player;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public bool canAct;
    [HideInInspector] public bool canDamage;
    [HideInInspector] public bool isDead;
    public List<DropItemInfo> dropTable = new List<DropItemInfo>();

    public virtual void OnEnable()
    {
        player = GameManager.Instance.player.transform;
        rb = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        canAct = false;
        canDamage = true;
        isDead = false;
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
        yield return null;
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
}