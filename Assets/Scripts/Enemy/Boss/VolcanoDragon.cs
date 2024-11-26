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
    public GameObject SurpriseAttack_sign; // ��� ���� ��ġ ���� �˸�
    public GameObject this_sprite;
    public GameObject Ablity_Get_Effect;
    [Space(10f)]
    #endregion

    #region LavaFlood
    [Header("Lava Flood")]
    [SerializeField] private GameObject lava;
    public Tilemap lavaTilemap;           // ��� Ÿ�ϸ�
    public TileBase lavaTile;             // ��� Ÿ��
    private bool rising = true;           // ����� ��� ������ ����
    private bool isLavaflood = false;     // ��� ���� �ڷ�ƾ�� ���������� Ȯ��
    public float riseSpeed = 0.1f;        // ����� �������� �ӵ� (�ʴ� Ÿ�� ����)
    private int currentHeight;            // ���� ��� ����
    public int maxHeight = 10;            // ����� ������ �ִ� ���� (Ÿ�� ����)
    public int minHeight = 0;             // ����� ������ ���� ���� (Ÿ�� ����)
    public int tilemapWidth = 10;         // Ÿ�ϸ��� ���� ���� (x�� ����)
    public int tilemapWidth_start;        // Ÿ�ϸ��� ���� x ��ġ
    [Space(10f)]
    #endregion

    #region Hide
    [Header("Hide")]
    [SerializeField] private Transform hidePos; // ���� ��ġ
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
        currentHeight = minHeight; // ����� �ʱ� ��ġ�� ������ ����

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
            Debug.Log("������ . . . ");
            rb.velocity = new Vector2(0, 0);
            horizentalDistance = player.position.x - transform.position.x;            
            FlipToPlayer(horizentalDistance);

                switch (currentState)
                {
                    case BossState.Idle:
                        if (!IsHideCoolingDown && hp < maxHp / 2)
                        // �ǰ� 50% �Ʒ��� ��� -> ����
                        {
                            yield return StartCoroutine(TransitionToState(BossState.Hide));
                        }
                        else if (IsMeleeAttackRange())
                            // ���� ���� ���� �� -> ���� ����
                        {
                            yield return StartCoroutine(TransitionToState(BossState.MeleeAttack));
                        }
                        else if (IsBreathRange())
                            // �극�� ���� �ȿ� ���� ��� -> �극�� 
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
                        yield return new WaitForSeconds(1f); // ���� �� ������

                        if (IsMeleeAttackRange())
                        {
                            yield return StartCoroutine(TransitionToState(BossState.MeleeAttack));
                        }
                        else if (!IsHideCoolingDown && hp < maxHp / 2)
                        // �ǰ� 50% �Ʒ��� ��� -> ����
                        {
                            yield return StartCoroutine(TransitionToState(BossState.Hide));
                        }
                        else if (meleeAtk_Count == meleeAtk_countMax) 
                            // ���� ���� ī��Ʈ �ƽ��� -> ��� ����
                        {
                            Debug.Log("���� ���� full count");
                            meleeAtk_Count = 0;
                            yield return StartCoroutine(TransitionToState(BossState.LavaEruption));
                        }
                        else if (IsBreathRange())
                            // �극�� ���� �ȿ� ���� ��� -> �극�� 
                        {
                            yield return StartCoroutine(TransitionToState(BossState.FireBreath));
                        }
                        else if (!IsMeleeAttackRange())
                            // �������� ���� ���̸� -> Idle
                        {
                            yield return StartCoroutine(TransitionToState(BossState.Idle));
                        }

                    break;

                    case BossState.FireBreath:
                        yield return StartCoroutine(Breath());
                        yield return new WaitForSeconds(0.5f); // ȭ�� �극�� �� ������

                        if (breath_Count == breath_countMax)
                        {
                            breath_Count = 0;
                            yield return StartCoroutine(TransitionToState(BossState.ChasePlayer));
                        }
                        else if (!IsHideCoolingDown && hp < maxHp / 2)
                        // �ǰ� 50% �Ʒ��� ��� -> ����
                        {
                            yield return StartCoroutine(TransitionToState(BossState.Hide));
                        }
                        else if (Vector2.Distance(transform.position, player.position) < meleeattackRange)
                        // ���� ���� ���� �ȿ� ���� ��� -> ���� ����
                        {
                            yield return StartCoroutine(TransitionToState(BossState.MeleeAttack));
                        }
                        else if (player.position.y < 68f)
                        // �÷������� ��ġ�� �Ʒ� ���� ��� -> ��� ��ȭ
                        {
                            yield return StartCoroutine(TransitionToState(BossState.LavaFlood));
                        }
                        else if (!IsBreathRange() || breath_Count == breath_countMax)
                            // �극�� ī��Ʈ �ƽ��� / Y�� ��ġ�� �ٸ� ��� -> ü�̽� �÷��̾�
                        {
                            Debug.Log("�극�� full count");
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
                            Debug.Log("��� ���� full count");
                            lavaflood_Count = 0;
                            yield return StartCoroutine(TransitionToState(BossState.LavaEruption));
                        }
                        else if (!IsHideCoolingDown && hp < maxHp/2) 
                            // �ǰ� 50% �Ʒ��� ��� -> ����
                        {
                            yield return StartCoroutine(TransitionToState(BossState.Hide));
                        }
                        else if (Vector2.Distance(transform.position, player.position) < meleeattackRange) 
                            // ���� ���� ���� �ȿ� ���� ��� -> ���� ����
                        {
                            yield return StartCoroutine(TransitionToState(BossState.MeleeAttack));
                        }
                        else if (Vector2.Distance(transform.position, player.position) < breathRangeStart) 
                            // �극�� ���� �ȿ� ���� ��� -> �극�� 
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
                        // ���� ���� ���� �ȿ� ���� ��� -> ���� ����
                        {
                            yield return StartCoroutine(TransitionToState(BossState.MeleeAttack));
                        }
                        yield return StartCoroutine(TransitionToState(BossState.Idle));

                    break;

                    case BossState.Hide:
                        yield return StartCoroutine(Hide());
                        yield return new WaitForSeconds(1f); // ��� ���� �� ������
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
        yield return new WaitForSeconds(0.5f); // ��ȯ ������
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
            if (player.transform.position.y + 1.5f > gameObject.transform.position.y) // �÷��̾ ���� ���� ��
            {
                Debug.Log("���� �̵�");
                while (player.transform.position.y > gameObject.transform.position.y)
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
                while (player.transform.position.y < gameObject.transform.position.y)
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
        else if (!animator.GetBool("Hit")) //��ü ���� ������ �̵� ������ ���
        {
            Debug.Log("�÷��̾�� �̵�");
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
            Debug.Log("�̵��Ұ� ����");
            rb.velocity = new Vector2(0, 0);
        }
        rb.velocity = new Vector2(0, 0);
    }

    private IEnumerator MeleeAttack()
    {
        Debug.Log("���� ����");
        meleeAtk_Count++;

        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack");

        yield return wait1;
    }

    private IEnumerator Breath()
    {
        Debug.Log("�극��");
        breath_Count++;

        FlipToPlayer(horizentalDistance);
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Breath");
        animator.SetBool("IsBreath", true);

        yield return new WaitForSeconds(3f); // �극�� ���ӽð�

        animator.SetBool("IsBreath", false);
        fireBreath.SetActive(false);
    }

    private IEnumerator LavaFlood()
    {
        Debug.Log("��� ����");
        isLavaflood = true;
        lavaflood_Count++;

        animator.SetTrigger("LavaFlood");
        CameraShake.Instance.OnShakeCamera();

        
        while (true)
        {
            if (rising)
            {
                // ����� ���� �������� ��
                if (currentHeight < maxHeight)
                {                   
                    currentHeight++;
                    UpdateLavaTiles();
                    yield return new WaitForSeconds(riseSpeed);
                }
                else
                {
                    // �ִ� ���̿� �������� �� ��� ���߰� �ϰ�
                    rising = false;
                    yield return wait1;
                }
            }
            else if (!rising)
            {
                // ����� �Ʒ��� ������ ��
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
        // Ÿ�ϸʿ��� �ּ� ���̺��� ���� ���̱��� Ÿ���� ����
        for (int y = minHeight; y <= maxHeight; y++)
        {
            for (int x = tilemapWidth_start; x < tilemapWidth; x++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (y <= currentHeight)
                {
                    // ���� ���� ������ Ÿ�Ͽ� ��� Ÿ���� ����
                    lavaTilemap.SetTile(tilePosition, lavaTile);
                }
                else
                {
                    // ���� ���� ���� Ÿ���� ����
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
        Debug.Log("����");

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
            yield return null; // ���� �����ӱ��� ���
        }

        IsHideCoolingDown = false; // ��Ÿ�� ����
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
