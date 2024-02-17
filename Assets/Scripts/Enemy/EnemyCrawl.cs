using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    private void Update()
    {
        Check();
    }

    void FixedUpdate()
    {
        if(canAct)
        {
            Move();
        }
    }

    void Move()
    {
        rb.velocity = transform.right * speed;
    }

    void Check()
    {
        isGround = Physics2D.Raycast(groundCheck.position, -1 * transform.up, groundDistance, groundLayor);
        isWall = Physics2D.Raycast(wallCheck.position, transform.right, wallDistance, groundLayor);

        if(!isGround)
        {
            if(!turned)
            {
                ZaxisAdd -= 90;
                transform.eulerAngles = new Vector3(0, 0, ZaxisAdd);
                if(dir == Direction.right)
                {
                    transform.position = new Vector2(transform.position.x + rotateDistance, transform.position.y - rotateDistance);
                    turned = true;
                    dir = Direction.down;
                }
                else if (dir == Direction.down) 
                {
                    transform.position = new Vector2(transform.position.x - rotateDistance, transform.position.y - rotateDistance);
                    turned = true;
                    dir = Direction.left;
                }
                else if (dir == Direction.left)
                {
                    transform.position = new Vector2(transform.position.x - rotateDistance, transform.position.y + rotateDistance);
                    turned = true;
                    dir = Direction.up;
                }
                else if (dir == Direction.up)
                {
                    transform.position = new Vector2(transform.position.x + rotateDistance, transform.position.y + rotateDistance);
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
                    transform.position = new Vector2(transform.position.x + rotateDistance, transform.position.y + rotateDistance);
                    turned = true;
                    dir = Direction.up;
                }
                else if (dir == Direction.down)
                {
                    transform.position = new Vector2(transform.position.x + rotateDistance, transform.position.y - rotateDistance);
                    turned = true;
                    dir = Direction.right;
                }
                else if (dir == Direction.left)
                {
                    transform.position = new Vector2(transform.position.x - rotateDistance, transform.position.y - rotateDistance);
                    turned = true;
                    dir = Direction.down;
                }
                else if (dir == Direction.up)
                {
                    transform.position = new Vector2(transform.position.x - rotateDistance, transform.position.y + rotateDistance);
                    turned = true;
                    dir = Direction.left;
                }
            }
        }
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        canAct = false;
        canDamage = false;

        //animator.SetTrigger("Hurt");
        rb.velocity = Vector2.zero;
        hp -= dmg;
        if (hp <= 0)
        {
            StartCoroutine(Death());
            yield break;
        }
        yield return new WaitForSeconds(1f);
        canDamage = true;
        canAct = true;
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
