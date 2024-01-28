using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyType2 : MonoBehaviour {
    private int hp = 3;
    private Rigidbody2D rb;
    private Animator animator;
    private bool canAct;

    //movement
    private Vector2 moveDirection;
    private float horizental;
    public float speed;

    //attack
    public int dmg;
    public GameObject objectPrefab;
    private List<GameObject> pool;

    //playerCheck
    public Transform player;
    private float distanceFromPlayer;
    public float viewRange;
    public float attackRange;
    public bool facingRight;

    //collisionCheck
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isGround;
    private bool isWall;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        pool = new List<GameObject>();
        canAct = true;
    }

    void Update()
    {
        Check(); //�� ����üũ

        horizental = player.position.x - transform.position.x;
        distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if(distanceFromPlayer < viewRange && canAct) //����� �ν� ���� ������ ���
        {
            FlipToPlayer(horizental);
            if(distanceFromPlayer > attackRange) //����� �Ÿ��� ���ݹ��� ���� ���
            {
                if (!isGround && !isWall) //��ü ���� ������ �̵� ������ ���
                {
                    moveDirection = (player.position - transform.position).normalized * speed;
                    rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
                }
                else
                    rb.velocity = Vector2.zero;
            }
            else //����� ���ݰŸ� ���� ���
            {
                rb.velocity = Vector2.zero;
                StartCoroutine(Attack());
            }
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
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if(playerPosition > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private IEnumerator Attack()
    {
        canAct = false;
        animator.SetTrigger("Attack");
        Debug.Log("Enemy's Attack!");
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(Shot());
    }

    private IEnumerator Shot()
    {
        EnemyBullet bullet;
        GameObject select = null;

        foreach (GameObject item in pool)
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(objectPrefab, transform);
            pool.Add(select);
        }

        bullet = select.GetComponent<EnemyBullet>();
        bullet.target = player.gameObject;
        moveDirection = (player.position - transform.position).normalized * 5f;
        bullet.rb.velocity = moveDirection;

        yield return new WaitForSeconds(3f);

        canAct = true;
    }

    public IEnumerator TakeDamage(int dmg)
    {
        if(canAct)
        {
            canAct = false;
            animator.SetTrigger("Hurt");
            yield return new WaitForSeconds(0.5f);
            canAct = true;
        }
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