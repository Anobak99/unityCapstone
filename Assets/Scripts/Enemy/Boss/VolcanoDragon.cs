using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
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

    [SerializeField] private Transform hidePos; // 숨는 위치
    private GameObject this_sprite;
    public GameObject fireBreath;

    #region LavaFlood
    public Tilemap lavaTilemap;           // 용암 타일맵
    public TileBase lavaTile;             // 용암 타일
    private bool rising = true;           // 용암이 상승 중인지 여부
    public float riseSpeed = 0.5f;        // 용암이 차오르는 속도 (초당 타일 높이)
    private int currentHeight;            // 현재 용암 높이
    public int maxHeight = 10;            // 용암이 도달할 최대 높이 (타일 단위)
    public int minHeight = 0;             // 용암이 도달할 최저 높이 (타일 단위)
    public int tilemapWidth = 10;         // 타일맵의 가로 길이 (x축 범위)
    public int tilemapWidth_start;        // 타일맵의 시작 x 위치
    #endregion

    [SerializeField] private int attackCount_1; // 기본 공격 카운트
    [SerializeField] private int attackCount_2; // 특수 공격 카운트
    [SerializeField] private int lavafloodCount;
    [SerializeField] private int breathCount;
    [SerializeField] private int countMax;

    [SerializeField] private float meleeattackRange;
    [SerializeField] private float breathRange;

    #region WaitForSec
    private readonly WaitForSeconds wait0Dot1 = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds wait1 = new WaitForSeconds(1f);
    private readonly WaitForSeconds wait2 = new WaitForSeconds(2f);
    #endregion

    public float cooldownTime = 5f; // 쿨타임 설정 
    private bool isLavaCooldown = false; // 쿨타임 여부

    private void Awake()
    {
        currentHeight = minHeight; // 용암의 초기 위치는 최저로 설정
        this_sprite = transform.Find("VolcanoDragon_sprite").gameObject;

        countMax = UnityEngine.Random.Range(2, 4);
        //StartCoroutine(Think());
    }

    private void Update()
    {
        hideHorizental = hidePos.position.x - this.transform.position.x;

        if(Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(LavaFlood());
        }
    } 

    private void Check()
    {
        isWall = Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayor);
    }

    public override IEnumerator Think()
    {
        Debug.Log("생각중 . . . ");

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
                    if (distanceFromPlayer < meleeattackRange)
                    {
                        Debug.Log("플레이어 근접 공격 범위 안 -> 근접 공격");
                        isMove = false;
                        rb.velocity = new Vector2(0, 0);
                        StartCoroutine(MeleeAttack());
                        yield break;
                    }
                    else if (distanceFromPlayer > breathRange && transform.position.y < player.position.y + 2f && transform.position.y > player.position.y - 2f && breathCount < 1)
                    {
                        Debug.Log("플레이어 멀리 있음 -> 브레스 공격");
                        rb.velocity = new Vector2(0, 0);
                        StartCoroutine(Breath());
                        yield break;
                    }
                    else //(distanceFromPlayer > meleeattackRange && breathCount >= 1)
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
                        Debug.Log("용암 분출 쿨타임중");
                    }
                }
            }
            else if (hp < maxHp/2) // hp 50% 미만인경우
            {
                rb.velocity = new Vector2(0, 0); rb.velocity = new Vector2(0, 0);
                lavafloodCount = 0;
                StartCoroutine(Hide());
                yield break;
            }
            else
            {
                StartCoroutine(ChasePlayer());
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
            if (player.transform.position.y + 1.5f > gameObject.transform.position.y) // 플레이어가 위에 있을 때
            {
                Debug.Log("위로 이동");
                while (player.transform.position.y + 1f > gameObject.transform.position.y)
                {
                    rb.velocity = new Vector2(0, speed);

                    yield return wait0Dot1;

                    loopnum++;
                    if (loopnum++ > 10000)
                        throw new Exception("Infinite Loop");
                }
            }
            else if (player.transform.position.y + 1.5f < gameObject.transform.position.y) // 플레이어가 아래에 있을 때
            {
                Debug.Log("아래로 이동");
                while (player.transform.position.y + 1f < gameObject.transform.position.y)
                {
                    Debug.Log("아래로 이동중");
                    rb.velocity = new Vector2(0, speed * -1f);

                    yield return wait0Dot1;

                    loopnum++;
                    if (loopnum++ > 10000)
                        throw new Exception("Infinite Loop");
                }
            }
        }
        else if (!animator.GetBool("Hit")) //개체 앞의 지형이 이동 가능한 경우
        {
            Debug.Log("플레이어에게 이동");
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
            Debug.Log("이동불가 상태");
            animator.SetInteger("AnimState", 0);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        rb.velocity = new Vector2(0, 0);

        yield return wait2; 

        StartCoroutine(Think());

        yield return null;
    }

    private IEnumerator MeleeAttack()
    {
        attackCount_1++;

        canAct = false;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack");
        yield return wait2;

        yield return wait1;

        StartCoroutine(Think());
    }

    private IEnumerator Breath()
    {
        attackCount_1++;
        breathCount++;

        animator.SetTrigger("Breath");
        fireBreath.SetActive(true);

        yield return wait2;

        fireBreath.SetActive(false);

        StartCoroutine(Think());
    }

    private IEnumerator LavaFlood()
    {
        Debug.Log("용암 분출");

        isLavaCooldown = true;

        lavafloodCount++;
        attackCount_2++;

        animator.SetTrigger("LavaFlood");
        CameraShake.Instance.OnShakeCamera();
        yield return wait2;
        StartCoroutine(Think());
        while (true)
        {
            if (rising)
            {
                // 용암을 위로 차오르게 함
                if (currentHeight < maxHeight)
                {
                    currentHeight++;
                    UpdateLavaTiles();
                    yield return new WaitForSeconds(riseSpeed);
                }
                else
                {
                    // 최대 높이에 도달했을 때 잠시 멈추고 하강
                    rising = false;
                    yield return new WaitForSeconds(2f);
                }
            }
            else if (!rising)
            {
                // 용암을 아래로 내리게 함
                if (currentHeight > minHeight)
                {
                    currentHeight--;
                    UpdateLavaTiles();
                    yield return new WaitForSeconds(riseSpeed);
                }
                else
                {
                    break;
                }
            }

        }
        yield return new WaitForSeconds(cooldownTime);
        isLavaCooldown = false;
        rising = true;
    }

    void UpdateLavaTiles()
    {
        // 타일맵에서 최소 높이부터 현재 높이까지 타일을 설정
        for (int y = minHeight; y <= maxHeight; y++)
        {
            for (int x = tilemapWidth_start; x < tilemapWidth; x++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (y <= currentHeight)
                {
                    // 현재 높이 이하의 타일에 용암 타일을 설정
                    lavaTilemap.SetTile(tilePosition, lavaTile);
                }
                else
                {
                    // 현재 높이 위의 타일은 비우기
                    lavaTilemap.SetTile(tilePosition, null);
                }
            }
        }
    }

    private IEnumerator Hide()
    {
        attackCount_2++;
        Debug.Log("숨기");

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
        Debug.Log("기습공격");
        animator.SetBool("SurpriseAtk", true);

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
        animator.SetBool("SurpriseAtk", false);

        yield return wait2;

        StartCoroutine(Think());
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        animator.SetBool("Hit", true);
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

        yield return wait1;
        animator.SetBool("Hit", false);
        canDamage = true;
    }

    private IEnumerator Death()
    {
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Death");
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
