using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyType1 : MonoBehaviour {
    private int hp = 3;
    private float moveDirection = -1;
    public float speed;
    public int dmg;

    public Transform player;
    private float distanceFromPlayer;
    private float horizental;
    public float viewRange;
    private bool canAttack;
    private bool facingRight;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    public bool isGround;
    public bool isWall;

    
    void Start()
    {
        facingRight = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canAttack = true;
    }

    void Update()
    {
        Check(); //�� ����üũ

        horizental = player.position.x - transform.position.x;
        distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if(distanceFromPlayer < viewRange) //����� �ν� ���� ������ ���
        {
            animator.SetInteger("AnimState", 1);
            FlipToPlayer(horizental);
            if(distanceFromPlayer > 1.5f) //����� �Ÿ��� ���ݹ��� ���� ���
            {
                if (isGround && !isWall) //��ü ���� ������ �̵� ������ ���
                {
                    animator.SetInteger("AnimState", 2);
                    rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
                }
                else
                    rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else //����� ���ݰŸ� ���� ���
            {
                if (canAttack)
                {
                   StartCoroutine(Attack());
                }
            }
        }
        else
        {
            animator.SetInteger("AnimState", 0);
        }
    }

    private void Check()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayor);
        isWall = Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayor);
    }

    private void FlipToPlayer(float playerPosition)
    {
        if(playerPosition < 0 && facingRight)
        {
            moveDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if(playerPosition > 0 && !facingRight)
        {
            moveDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        animator.SetTrigger("Attack");
        Debug.Log("Enemy's Attack!");
        yield return new WaitForSeconds(3f);
        canAttack = true;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if(hp <= 0 )
        {
            IsDead();
            gameObject.SetActive(false);
        }
    }

    private void IsDead()
    {
        Debug.Log("Enemy is Dead.");
    }
}
