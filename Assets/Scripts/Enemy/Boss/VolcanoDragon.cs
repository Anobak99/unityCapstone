using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class VolcanoDragon : Enemy
{
    private float moveDirection = 1;
    private bool facingRight = true;
    [SerializeField] private float speed;
    private float moveTime;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;
    [SerializeField] Transform WallCheck;
    private bool isWall;
    private bool isMove;

    private float distanceFromPlayer;
    private float horizental;
    private float hideHorizental;

    [SerializeField] private GameObject[] firstLava; // ��� ������
    [SerializeField] private GameObject[] secondLava; // ��� ������
    [SerializeField] private Transform hidePos; // ���� ��ġ
    private GameObject this_sprite;

    private int attackCount_1; // �⺻ ���� ī��Ʈ
    private int attackCount_2; // Ư�� ���� ī��Ʈ
    private int lavafloodCount;
    private int breathCount;
    [SerializeField] private int countMax;

    [SerializeField] private float meleeattackRange;
    [SerializeField] private float breathRange;

    #region WaitForSec
    private readonly WaitForSeconds wait0Dot1 = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds wait1 = new WaitForSeconds(1f);
    #endregion

    public float cooldownTime = 5f; // ��Ÿ�� ���� 
    private bool isLavaCooldown = false; // ��Ÿ�� ����

    private void Awake()
    {
        this_sprite = transform.Find("VolcanoDragon_sprite").gameObject;

        countMax = UnityEngine.Random.Range(2, 4);
        //StartCoroutine(Think());
    }

    private void Update()
    {
        hideHorizental = hidePos.position.x - this.transform.position.x;

        if(Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Hide());
        }
    } 

    private void Check()
    {
        isWall = Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayor);
    }

    public override IEnumerator Think()
    {
        Debug.Log("������ . . . ");

        Check();

        //if (GameManager.Instance.gameState != GameManager.GameState.Boss || isDead || Time.timeScale == 0) yield return null;

        if (hp > 0)
        {
            Debug.Log("...");
            distanceFromPlayer = Vector2.Distance(player.position, transform.position);
            horizental = player.position.x - transform.position.x;
            FlipToPlayer(horizental);

            if (attackCount_2 < 3 || hp > maxHp / 2)
            {
                if (attackCount_1 < countMax)
                {
                    animator.SetInteger("AnimState", 0);
                    if (distanceFromPlayer < meleeattackRange)
                    {
                        Debug.Log("�÷��̾� ���� ���� ���� �� -> ���� ����");
                        isMove = false;
                        rb.velocity = new Vector2(0, 0);
                        StartCoroutine(MeleeAttack());
                        yield break;
                    }
                    else if (distanceFromPlayer > breathRange && breathCount < 1)
                    {
                        Debug.Log("�÷��̾� �ָ� ���� -> �극�� ����");
                        rb.velocity = new Vector2(0, 0);
                        StartCoroutine(Breath());
                        yield break;
                    }
                    else if (distanceFromPlayer > meleeattackRange && breathCount >= 1)
                    {
                        breathCount = 0;
                        StartCoroutine(ChasePlayer());
                        yield break;
                    }
                }
                else
                {
                    if (!isLavaCooldown)
                    {
                        rb.velocity = new Vector2(0, 0);
                        attackCount_1 = 0;
                        StartCoroutine(LavaFlood());
                        yield break;
                    }
                    else
                    {
                        attackCount_1 = 0;
                        Debug.Log("��� ���� ��Ÿ����");
                    }
                }
            }
            else if (hp < maxHp/2) // hp 50% �̸��ΰ��
            {
                rb.velocity = new Vector2(0, 0); rb.velocity = new Vector2(0, 0);
                lavafloodCount = 0;
                StartCoroutine(Hide());
                yield break;
            }
        }
     
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Think());
    }

    private void FlipToPlayer(float playerPosition)
    {
        if (playerPosition < 0 && facingRight)
        {
            moveDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if (playerPosition > 0 && !facingRight)
        {
            moveDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private void FlipToHidePos(float hidePosition)
    {
        if (hidePosition < 0 && facingRight)
        {
            moveDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if (hidePosition > 0 && !facingRight)
        {
            moveDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private IEnumerator ChasePlayer()
    {
        Debug.Log("Chase Player");

        int loopnum = 0;

        float yPos = Mathf.Abs(player.transform.position.y - gameObject.transform.position.y);

        if (yPos > 2f)
        {
            if (player.transform.position.y + 1.5f > gameObject.transform.position.y) // �÷��̾ ���� ���� ��
            {
                Debug.Log("���� �̵�");
                while (player.transform.position.y + 1f > gameObject.transform.position.y)
                {
                    rb.velocity = new Vector2(0, speed);

                    yield return wait0Dot1;

                    loopnum++;
                    if (loopnum++ > 10000)
                        throw new Exception("Infinite Loop");
                }
            }
            else if (player.transform.position.y + 1.5f < gameObject.transform.position.y) // �÷��̾ �Ʒ��� ���� ��
            {
                Debug.Log("�Ʒ��� �̵�");
                while (player.transform.position.y + 1f < gameObject.transform.position.y)
                {
                    Debug.Log("�Ʒ��� �̵���");
                    rb.velocity = new Vector2(0, speed * -1f);

                    yield return wait0Dot1;

                    loopnum++;
                    if (loopnum++ > 10000)
                        throw new Exception("Infinite Loop");
                }
            }
        }
        else if (!isWall && !animator.GetBool("Hit")) //��ü ���� ������ �̵� ������ ���
        {
            Debug.Log("�÷��̾�� �̵�");
            animator.SetInteger("AnimState", 1);
            if (moveDirection > 0)
            {
                while (player.transform.position.x > gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);

                    yield return wait0Dot1;

                    loopnum++;
                    if (loopnum++ > 10000)
                        throw new Exception("Infinite Loop");
                }
            }
            else
            {
                while (player.transform.position.x < gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);

                    yield return wait0Dot1;

                    loopnum++;
                    if (loopnum++ > 10000)
                        throw new Exception("Infinite Loop");
                }
            }          
        }
        else if (!isWall || animator.GetBool("Hit"))
        {
            Debug.Log("�̵��Ұ� ����");
            animator.SetInteger("AnimState", 0);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        StartCoroutine(Think());

        yield return null;
    }

    private IEnumerator MeleeAttack()
    {
        attackCount_1++;

        canAct = false;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("MeleeAttack");
        yield return wait1;
        attackCount_1++;

        yield return wait1;

        StartCoroutine(Think());
    }

    private IEnumerator Breath()
    {
        attackCount_1++;
        breathCount++;

        yield return wait1;

        StartCoroutine(Think());

        yield return null;
    }

    private IEnumerator LavaFlood()
    {
        Debug.Log("��� ����");

        isLavaCooldown = true;

        lavafloodCount++;
        attackCount_2++;

        CameraShake.Instance.OnShakeCamera();

        for (int i = 0; i < firstLava.Length; i++)
        {
            firstLava[i].SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < secondLava.Length; i++)
        {
            secondLava[i].SetActive(true);
        }

        StartCoroutine(Think());
        yield return new WaitForSeconds(cooldownTime);
        isLavaCooldown = false;

        yield return null;
    }

    private IEnumerator Hide()
    {
        attackCount_2++;
        Debug.Log("����");

        int loopnum=0;

        FlipToHidePos(hideHorizental);

        while(hidePos.transform.position.x != this.transform.position.x)
        {
            if (moveDirection > 0 && hidePos.transform.position.x < this.transform.position.x)
            {
                rb.velocity = new Vector2(0, 0);
                break;
            }
            else if (moveDirection < 0 && hidePos.transform.position.x > this.transform.position.x)
            {
                rb.velocity = new Vector2(0, 0);
                break;
            }

            rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);

            yield return wait0Dot1;
        
            loopnum++;
            if (loopnum++ > 10000)
                throw new Exception("Infinite Loop");
        }

        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        rb.velocity = new Vector2(0, speed * -1f);

        yield return wait1;

        rb.velocity = new Vector2(0, 0);

        this_sprite.SetActive(false);

        StartCoroutine(SurpriseAttack());
        yield return null;
    }

    private IEnumerator SurpriseAttack()
    {
        Debug.Log("�������");

        int loopnum = 0;

        Vector3 currentPos = gameObject.transform.position;
        transform.position = new Vector3(player.position.x, currentPos.y, currentPos.z);

        while (player.transform.position.y + 2f > gameObject.transform.position.y)
        {
            if(hidePos.transform.position.y < gameObject.transform.position.y)
            {
                this_sprite.SetActive(true);
            }

            rb.velocity = new Vector2(0, 5f);

            yield return wait0Dot1;

            loopnum++;
            if (loopnum++ > 10000)
                throw new Exception("Infinite Loop");
        }

        yield return wait0Dot1;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        StartCoroutine(Think());

        yield return null;
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        canDamage = false;
        spriteRenderer.material = flashMaterial;
        hp -= dmg;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;
        if (hp <= 0)
        {
            StartCoroutine(Death());
            yield break;
        }
        canDamage = true;
    }

    private IEnumerator Death()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("Hit", true);
        canDamage = false;
        isDead = true;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeattackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, breathRange);
    }
}
