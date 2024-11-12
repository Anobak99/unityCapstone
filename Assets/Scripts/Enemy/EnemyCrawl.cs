using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyCrawl : Enemy
{
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform wallCheck;
    [SerializeField] private bool isGround;
    [SerializeField] private bool isWall;
    [SerializeField] private float groundDistance;
    [SerializeField] private float wallDistance;
    [SerializeField] private float speed;
    [SerializeField] private float rotateDistance;

    private bool turned;
    private enum Direction
    {
        left, right, up, down
    }
    [SerializeField] private Direction dir;
    private int ZaxisAdd;

    public override IEnumerator Think()
    {
        Check();

        if (!turned)
        {
            rb.velocity = transform.right * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        

        yield return new WaitForSeconds(0.1f);
        act1 = StartCoroutine(Think());
    }

    void Check()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayor);
        isWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayor);

        if (!isGround)
        {
            if(!turned)
            {
                ZaxisAdd -= 90;
                transform.eulerAngles = new Vector3(0, 0, ZaxisAdd);
                if(dir == Direction.right)
                {
                    transform.position = new Vector2(Mathf.FloorToInt(transform.position.x), transform.position.y - rotateDistance);
                    dir = Direction.down;
                }
                else if (dir == Direction.down) 
                {
                    transform.position = new Vector2(transform.position.x - rotateDistance, Mathf.CeilToInt(transform.position.y));
                    turned = true;
                    dir = Direction.left;
                }
                else if (dir == Direction.left)
                {
                    transform.position = new Vector2(Mathf.CeilToInt(transform.position.x), transform.position.y + rotateDistance);;
                    turned = true;
                    dir = Direction.up;
                }
                else if (dir == Direction.up)
                {
                    transform.position = new Vector2(transform.position.x + rotateDistance, Mathf.FloorToInt(transform.position.y));
                    turned = true;
                    dir = Direction.right;
                }
            }
        }
        else
        {
            turned = false;
        }

        if(isWall)
        {
            if (!turned)
            {
                ZaxisAdd += 90;
                transform.eulerAngles = new Vector3(0, 0, ZaxisAdd);
                if (dir == Direction.right)
                {
                    transform.position = new Vector2(Mathf.CeilToInt(transform.position.x), transform.position.y + rotateDistance);
                    turned = true;
                    dir = Direction.up;
                }
                else if (dir == Direction.down)
                {
                    transform.position = new Vector2(transform.position.x + rotateDistance, Mathf.FloorToInt(transform.position.y));
                    turned = true;
                    dir = Direction.right;
                }
                else if (dir == Direction.left)
                {
                    transform.position = new Vector2(Mathf.FloorToInt(transform.position.x), transform.position.y - rotateDistance);
                    turned = true;
                    dir = Direction.down;
                }
                else if (dir == Direction.up)
                {
                    transform.position = new Vector2(transform.position.x - rotateDistance, Mathf.CeilToInt(transform.position.y));
                    turned = true;
                    dir = Direction.left;
                }
            }
        }
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        rb.velocity = Vector2.zero;
        canDamage = false;

        spriteRenderer.material = flashMaterial;
        hp -= dmg;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;
        yield return new WaitForSeconds(0.2f);
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
        rb.velocity = Vector2.zero;
        //animator.SetTrigger("Death");
        canDamage = false;
        isDead = true;
        ItemDrop();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
