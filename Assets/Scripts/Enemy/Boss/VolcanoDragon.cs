using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VolcanoDragon : Boss
{
    #region BossState
    private enum BossState { Idle, ChasePlayer, MeleeAttack, FireBreath, LavaFlood, LavaEruption, Hide}
    private BossState currentState = BossState.Idle;
    #endregion

    #region Move
    [Header ("Move")]
    private float moveDirection = 1;
    private bool facingRight = true;
    [SerializeField] private float speed;
    private float distanceFromPlayer;
    private float horizentalDistance;
    private float verticalDistance;
    [SerializeField] private BossBattle battle;
    [Space(10f)]
    #endregion

    #region Object  
    [Header("Object")]
    public GameObject fireBreath;
    public GameObject eruption;
    public GameObject SurpriseAttack_sign; // 기습 공격 위치 전조 알림
    public GameObject this_sprite;
    public GameObject Ablity_Get_Effect;
    [Space(10f)]
    #endregion

    #region LavaFlood
    [Header("Lava Flood")]
    [SerializeField] private GameObject lava;
    public Tilemap lavaTilemap;           // 용암 타일맵
    public TileBase lavaTile;             // 용암 타일
    private bool rising = true;           // 용암이 상승 중인지 여부
    private bool isLavaflood = false;     // 용암 분출 코루틴이 동작중인지 확인
    public float riseSpeed = 0.1f;        // 용암이 차오르는 속도 (초당 타일 높이)
    private int currentHeight;            // 현재 용암 높이
    public int maxHeight = 10;            // 용암이 도달할 최대 높이 (타일 단위)
    public int minHeight = 0;             // 용암이 도달할 최저 높이 (타일 단위)
    public int tilemapWidth = 10;         // 타일맵의 가로 길이 (x축 범위)
    public int tilemapWidth_start;        // 타일맵의 시작 x 위치
    [Space(10f)]
    #endregion

    #region Hide
    [Header("Hide")]
    [SerializeField] private Transform hidePos; // 숨는 위치
    private float hideHorizental;
    [Space(10f)]
    #endregion

    #region CoolTime
    [SerializeField] private float HidecooldownTime;
    private bool IsHideCoolingDown;
    #endregion

    #region Count
    private int meleeAtk_Count = 0;
    private int meleeAtk_countMax;
    private int breath_Count = 0;
    private int breath_countMax;
    private int lavaflood_Count = 0;
    private int lavaflood_countMax;
    private int LavaEruption_Count = 0;
    private int LavaEruption_countMax;
    private int SurpriseAttack_countMax;
    #endregion

    #region Range
    [Header("Range")]
    [SerializeField] private float viewRange;
    [SerializeField] private float meleeattackRange;
    [SerializeField] private float breathRangeStart;
    [SerializeField] private float breathRangeEnd;
    #endregion

    #region WaitForSec
    private readonly WaitForSeconds wait0Dot1 = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds wait1 = new WaitForSeconds(1f);
    private readonly WaitForSeconds wait2 = new WaitForSeconds(2f);
    #endregion

    private void Start()
    {
        currentHeight = minHeight; // 용암의 초기 위치는 최저로 설정

        meleeAtk_countMax = UnityEngine.Random.Range(2, 3);
        breath_countMax = UnityEngine.Random.Range(2, 3);
        lavaflood_countMax = UnityEngine.Random.Range(2, 3);
        LavaEruption_countMax = UnityEngine.Random.Range(1, 3);       
    }

    public override IEnumerator Think()
    {      
        if (GameManager.Instance.gameState != GameManager.GameState.Boss || isDead || Time.timeScale == 0) yield return null;

        if (canAct && player != null && !GameManager.Instance.isDead)
        {            
            Debug.Log("생각중 . . . ");
            rb.velocity = new Vector2(0, 0);
            horizentalDistance = player.position.x - transform.position.x;            
            FlipToPlayer(horizentalDistance);

                switch (currentState)
                {
                    case BossState.Idle:
                        if (!IsHideCoolingDown && hp < maxHp / 2)
                        // 피가 50% 아래인 경우 -> 숨기
                        {
                            yield return StartCoroutine(TransitionToState(BossState.Hide));
                        }
                        else if (IsMeleeAttackRange())
                            // 근접 공격 범위 안 -> 근접 공격
                        {
                            yield return StartCoroutine(TransitionToState(BossState.MeleeAttack));
                        }
                        else if (IsBreathRange())
                            // 브레스 범위 안에 있을 경우 -> 브레스 
                        {
                            yield return StartCoroutine(TransitionToState(BossState.FireBreath));
                        }
                        else if (Vector2.Distance(transform.position, player.position) < viewRange)
                        {
                            yield return StartCoroutine(TransitionToState(BossState.ChasePlayer));
                        }

                    break;

                    case BossState.ChasePlayer:
                            yield return StartCoroutine(ChasePlayer());
                            yield return StartCoroutine(TransitionToState(BossState.Idle));

                    break;
                        
                    case BossState.MeleeAttack:
                        yield return StartCoroutine(MeleeAttack());
                        yield return new WaitForSeconds(1f); // 공격 후 딜레이

                        if (IsMeleeAttackRange())
                        {
                            yield return StartCoroutine(TransitionToState(BossState.MeleeAttack));
                        }
                        else if (!IsHideCoolingDown && hp < maxHp / 2)
                        // 피가 50% 아래인 경우 -> 숨기
                        {
                            yield return StartCoroutine(TransitionToState(BossState.Hide));
                        }
                        else if (meleeAtk_Count == meleeAtk_countMax) 
                            // 근접 공격 카운트 맥스면 -> 용암 분출
                        {
                            Debug.Log("근접 공격 full count");
                            meleeAtk_Count = 0;
                            yield return StartCoroutine(TransitionToState(BossState.LavaEruption));
                        }
                        else if (IsBreathRange())
                            // 브레스 범위 안에 있을 경우 -> 브레스 
                        {
                            yield return StartCoroutine(TransitionToState(BossState.FireBreath));
                        }
                        else if (!IsMeleeAttackRange())
                            // 근접공격 범위 밖이면 -> Idle
                        {
                            yield return StartCoroutine(TransitionToState(BossState.Idle));
                        }

                    break;

                    case BossState.FireBreath:
                        yield return StartCoroutine(Breath());
                        yield return new WaitForSeconds(0.5f); // 화염 브레스 후 딜레이

                        if (breath_Count == breath_countMax)
                        {
                            breath_Count = 0;
                            yield return StartCoroutine(TransitionToState(BossState.ChasePlayer));
                        }
                        else if (!IsHideCoolingDown && hp < maxHp / 2)
                        // 피가 50% 아래인 경우 -> 숨기
                        {
                            yield return StartCoroutine(TransitionToState(BossState.Hide));
                        }
                        else if (Vector2.Distance(transform.position, player.position) < meleeattackRange)
                        // 근접 공격 범위 안에 있을 경우 -> 근접 공격
                        {
                            yield return StartCoroutine(TransitionToState(BossState.MeleeAttack));
                        }
                        else if (player.position.y < 68f)
                        // 플레어이의 위치가 아래 쪽인 경우 -> 용암 분화
                        {
                            yield return StartCoroutine(TransitionToState(BossState.LavaFlood));
                        }
                        else if (!IsBreathRange() || breath_Count == breath_countMax)
                            // 브레스 카운트 맥스면 / Y축 위치가 다른 경우 -> 체이스 플레이어
                        {
                            Debug.Log("브레스 full count");
                            breath_countMax = 0;
                            yield return StartCoroutine(TransitionToState(BossState.ChasePlayer));
                        }

                    break;

                    case BossState.LavaFlood:
                        if (isLavaflood) yield return StartCoroutine(TransitionToState(BossState.Idle));
                        StartCoroutine(LavaFlood());
                        yield return new WaitForSeconds(1f);

                        if (lavaflood_Count == lavaflood_countMax)
                        {
                            Debug.Log("용암 분출 full count");
                            lavaflood_Count = 0;
                            yield return StartCoroutine(TransitionToState(BossState.LavaEruption));
                        }
                        else if (!IsHideCoolingDown && hp < maxHp/2) 
                            // 피가 50% 아래인 경우 -> 숨기
                        {
                            yield return StartCoroutine(TransitionToState(BossState.Hide));
                        }
                        else if (Vector2.Distance(transform.position, player.position) < meleeattackRange) 
                            // 근접 공격 범위 안에 있을 경우 -> 근접 공격
                        {
                            yield return StartCoroutine(TransitionToState(BossState.MeleeAttack));
                        }
                        else if (Vector2.Distance(transform.position, player.position) < breathRangeStart) 
                            // 브레스 범위 안에 있을 경우 -> 브레스 
                        {
                            yield return StartCoroutine(TransitionToState(BossState.FireBreath));
                        }
                        else
                        {
                            yield return StartCoroutine(TransitionToState(BossState.ChasePlayer));
                        }

                    break;

                    case BossState.LavaEruption:
                        yield return StartCoroutine(LavaEruption());
                        yield return wait1;
                        if (LavaEruption_Count == LavaEruption_countMax)
                        {
                            LavaEruption_Count = 0;
                            yield return StartCoroutine(TransitionToState(BossState.ChasePlayer));
                        }
                        else if (Vector2.Distance(transform.position, player.position) < meleeattackRange)
                        // 근접 공격 범위 안에 있을 경우 -> 근접 공격
                        {
                            yield return StartCoroutine(TransitionToState(BossState.MeleeAttack));
                        }
                        yield return StartCoroutine(TransitionToState(BossState.Idle));

                    break;

                    case BossState.Hide:
                        yield return StartCoroutine(Hide());
                        yield return new WaitForSeconds(1f); // 기습 공격 후 딜레이
                        yield return StartCoroutine(TransitionToState(BossState.Idle));

                    break;            
            }
            
        }
        yield return new WaitForSeconds(0.05f);
        yield return StartCoroutine(Think());
    }

    IEnumerator TransitionToState(BossState newState)
    {
        canAct = false;
        yield return new WaitForSeconds(0.5f); // 전환 딜레이
        currentState = newState;
        canAct = true;
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

    private bool IsMeleeAttackRange()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        verticalDistance = player.position.y - transform.position.y;
        if (distance < meleeattackRange && verticalDistance > -1f && verticalDistance < 2.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsBreathRange()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        verticalDistance = player.position.y - transform.position.y;

        if (distance > breathRangeStart && distance < breathRangeEnd && verticalDistance > -1f && verticalDistance < 2.1f)
        {
            return true;
        }
        else
        {
            return false;
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
                while (player.transform.position.y > gameObject.transform.position.y)
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
                while (player.transform.position.y < gameObject.transform.position.y)
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
            distanceFromPlayer = Vector2.Distance(player.position, transform.position);
            while (distanceFromPlayer > 8f)
            {
                distanceFromPlayer = Vector2.Distance(player.position, transform.position);
                rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);

                yield return wait0Dot1;

                loopnum++;
                if (loopnum++ > 10000)
                    throw new Exception("Infinite Loop");
            }
        }
        else if (animator.GetBool("Hit"))
        {
            Debug.Log("이동불가 상태");
            rb.velocity = new Vector2(0, 0);
        }
        rb.velocity = new Vector2(0, 0);
    }

    private IEnumerator MeleeAttack()
    {
        Debug.Log("근접 공격");
        meleeAtk_Count++;

        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack");

        yield return wait1;
    }

    private IEnumerator Breath()
    {
        Debug.Log("브레스");
        breath_Count++;

        FlipToPlayer(horizentalDistance);
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Breath");
        animator.SetBool("IsBreath", true);

        yield return new WaitForSeconds(3f); // 브레스 지속시간

        animator.SetBool("IsBreath", false);
        fireBreath.SetActive(false);
    }

    private IEnumerator LavaFlood()
    {
        Debug.Log("용암 분출");
        isLavaflood = true;
        lavaflood_Count++;

        animator.SetTrigger("LavaFlood");
        CameraShake.Instance.OnShakeCamera();

        
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
                    yield return wait1;
                }
            }
            else if (!rising)
            {
                // 용암을 아래로 내리게 함
                if (currentHeight > minHeight)
                {
                    currentHeight--;
                    UpdateLavaTiles();
                    yield return new WaitForSeconds(riseSpeed / 2f);
                }
                else
                {
                    break;
                }
            }

        }
        rising = true;
        isLavaflood = false;
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

    private IEnumerator LavaEruption()
    {
        Debug.Log("Lava Eruption");
        LavaEruption_Count++;

        animator.SetTrigger("LavaFlood");
        CameraShake.Instance.OnShakeCamera(1f, 0.5f);
        yield return wait1;
        eruption.SetActive(true);
        yield return wait2;
        eruption.SetActive(false);
    }

    private IEnumerator Hide()
    {
        Debug.Log("숨기");

        int loopnum=0;
        SurpriseAttack_countMax = UnityEngine.Random.Range(2, 3);

        hideHorizental = hidePos.position.x - this.transform.position.x;
        FlipToHidePos(hideHorizental);       

        while(hidePos.transform.position.y+1f < this.transform.position.y)
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
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, hidePos.position, speed * Time.deltaTime);

            //rb.velocity = new Vector2(moveDirection * speed * 1.5f, rb.velocity.y);

            yield return new WaitForSeconds(0.025f);
        
            loopnum++;
            if (loopnum++ > 10000)
                throw new Exception("Infinite Loop");
        }

        rb.velocity = new Vector2(0, 0);
        this_sprite.SetActive(false);

        yield return StartCoroutine(SurpriseAttack());
    }

    private IEnumerator SurpriseAttack()
    {
        int loopnum = 0;

        for (int i = 0; i < SurpriseAttack_countMax; i++)
        {
            Debug.Log("Surprise Attack !");
            yield return wait1;
            SoundManager.PlaySound(SoundType.SFX, 1, 6);
            CameraShake.Instance.OnShakeCamera(1f, 0.5f);
            SurpriseAttack_sign.transform.position = new Vector3(player.position.x, player.position.y, SurpriseAttack_sign.transform.position.z);
            SurpriseAttack_sign.SetActive(true);
            gameObject.transform.position = new Vector3(player.position.x, hidePos.position.y -10f, gameObject.transform.position.z);
            yield return new WaitForSeconds(1f);
            SurpriseAttack_sign.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            this_sprite.SetActive(true);
            while (gameObject.transform.position.y < 86f)
            {
                if (hidePos.transform.position.y < gameObject.transform.position.y)
                {
                    if (!this_sprite.activeSelf)
                    {
                        this_sprite.SetActive(true);
                    }
                    if (!animator.GetBool("IsSurpriseAtk"))
                    {
                        animator.SetTrigger("SurpriseAtk");
                        animator.SetBool("IsSurpriseAtk", true);
                    }
                }

                rb.velocity = new Vector2(0, 10f);

                yield return wait0Dot1;

                loopnum++;
                if (loopnum++ > 10000)
                    throw new Exception("Infinite Loop");
            }

            animator.SetBool("IsSurpriseAtk", false);
            yield return wait1;
            this_sprite.SetActive(false);
        }

        gameObject.transform.position = hidePos.position;
        this_sprite.SetActive(true);

        while (gameObject.transform.position.y < 70.5f)
        {
            rb.velocity = new Vector2(0, 5f);

            yield return wait0Dot1;

            loopnum++;
            if (loopnum++ > 10000)
                throw new Exception("Infinite Loop");
        }
        rb.velocity = new Vector2(0, 0f);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        StartCoroutine(HideCooldown());
    }

    IEnumerator HideCooldown()
    {
        IsHideCoolingDown = true;

        float elapsedTime = 0f;

        while (elapsedTime < HidecooldownTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        IsHideCoolingDown = false; // 쿨타임 종료
        Debug.Log("Cooldown finished. You can use the skill again.");
    }

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {

        //animator.SetTrigger("Hit");
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
        canDamage = true;

    }

    private IEnumerator Death()
    {
        StopAllCoroutines();
        lava.SetActive(false);
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Death");
        Ablity_Get_Effect.SetActive(true);
        canDamage = false;
        isDead = true;
        battle.BossDead();
        yield return new WaitForSeconds(1f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeattackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, breathRangeStart);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, breathRangeEnd);
    }
}
