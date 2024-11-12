using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    #region Move
    [Header("움직임")]
    private Rigidbody2D rigid;
    private Animator anim;
    private PlayerInput input;
    private GameObject currentOneWayPlatform; // onewayplatform 오브젝트
    private Collider2D playerCollider;
    public float moveSpeed;
    #endregion

    #region Jump
    [Header("점프")]
    [SerializeField] private float vertical; // 수직항력
    [SerializeField] private float jumpTime;
    [SerializeField] private float coyoteTimeCounter;
    public float gravity = 5;
    public float jumpPower;
    public float djumpPower;    
    private float jumpCounter;    
    private float maxJumpTime = 0.25f;
    private float coyoteTime = 0.2f;
    private bool isJumping;
    private bool doubleJump;
    #endregion

    #region Dash
    [Header("대시")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.1f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;
    #endregion

    #region Object Pickup
    [Header("잡기")]
    public Transform holdPosition;   // 들고 있는 위치
    private Rigidbody2D heldObject;  // 들고 있는 물체의 Rigidbody2D
    public float liftingForce = 5f; // 들기 힘
    public float holdRange = 1.5f;
    private bool isHolding = false;  // 현재 물건을 들고 있는지 여부
    #endregion

    #region Attack
    [Header("공격")]
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public int damage;        // 데미지 수치
    public float attackRange; // 공격 범위
    public float startTimeBtwAttack = 0.8f; // 공격 쿨타임 설정
    public float timeBtwAttack;  // 공격 쿨타임 (0이 되면 공격가능)
    #endregion

    #region FireBall
    [Header("파이어볼")]
    public float timeBtwFire;
    public float startTimeBtwFire = 2f; // 투사체 쿨타임
    #endregion

    #region 애니메이터 초기값 
    private bool isGrounded = true;
    private bool isJump;
    private bool isRun;
    private bool isDash;
    private bool isGrab;
    #endregion

    #region Checker
    [Header("체커")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private Vector2 boxSize = new Vector2(0.8f, 0.2f);
    private Collider2D col;

    public bool canDamage;
    private bool isDamaged;
    public bool canAct;
    private bool canFireBall;
    private bool isDead;
    public bool isRespawn;
    #endregion

    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
        input = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canDamage = true;
        canAct = true;
        isDead = false;
        isRespawn = false;

        isJump = anim.GetBool("isJump");
        isRun = anim.GetBool("isRun");
        isDash = anim.GetBool("isDash");
        isGrab = anim.GetBool("isGrab");
    }

    private void Update()
    {
        UpdateVariables();
        
        if(!canAct || isRespawn || isDead || DialogueManager.Instance.isDialogue ||Time.timeScale == 0f) 
        {
            ResetAnimeParameter();
            rigid.velocity = new Vector2(0, 0);
            return; 
        }

        Flip();
        Run();
        Jump();
        WhileJump();
        Setgravity();
        CheckAnime();

        // 아래키 입력->플랫폼 통과
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }

        // 대시 
        if (input.dashInput && canDash && !isHolding)
        {
            //Debug.Log("Dash");
            isDashing = true;
            canDash = false;
            canDamage = false;
            StartCoroutine(Dashing());
        }

        if (timeBtwAttack <= 0 && input.attackInput && !isDashing && !isHolding)
        {          
            StartCoroutine(Attack());
        }

        if (canFireBall && timeBtwFire <= 0 && input.fireballInput && !isDashing && !isHolding)
        {
            StartCoroutine(FireBall());
        }

        // 물건 들기/놓기 토글
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isHolding)
            {
                ReleaseObject();
            }
            else
            {
                PickUpObject();
            }
        }

        // 물건을 들고 있는 경우 위치 업데이트
        if (isHolding)
        {
            UpdateHeldObjectPosition();
        }

    }

    // 캐릭터 좌우 반전
    void Flip()
    {
        if (input.horizontal > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        else if (input.horizontal < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
    }

    // 바닥 체크
    private bool IsGrounded()
    {
        col = Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer);
        if (!col)
        {
            anim.SetBool("isGrounded", false);
            return false;
        }
        else if (col.CompareTag("Platform")) //바닥이 플랫폼일 시
        {
            SpecialPlatform platform = col.GetComponent<SpecialPlatform>();
            platform.OnStand();
        }

        anim.SetBool("isGrounded", true);
        return true;
    }

    private void Run()
    {
        if (!isDashing)
        {
            rigid.velocity = new Vector2(input.horizontal * moveSpeed, rigid.velocity.y);
            anim.SetBool("isRun", rigid.velocity.x != 0 && IsGrounded());
        }
    }

    void UpdateVariables()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            doubleJump = true;
        }
        else
        {
            if (coyoteTimeCounter > 0) coyoteTimeCounter -= Time.deltaTime;
        }

        if (SwitchManager.Instance.abilities[0]) canFireBall = true;

        if (input.jumpBufferCounter > 0) input.jumpBufferCounter -= Time.deltaTime;

        if(timeBtwAttack > 0) { timeBtwAttack -= Time.deltaTime; }

        if (timeBtwFire > 0) { timeBtwFire -= Time.deltaTime; }
    }

    void Jump()
    {
        if (coyoteTimeCounter > 0f && input.jumpBufferCounter > 0f && !isJumping && !isHolding)
        {
            //SoundManager.instance.PlaySfx(0);
            input.jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            isJumping = true;
            vertical = jumpPower;
            jumpCounter += 1;
        }

        if(!IsGrounded() && doubleJump && input.jumpBufferCounter > 0f && jumpCounter > 0f)
        {
            //SoundManager.instance.PlaySfx(0);
            //Debug.Log("double jump");
            input.jumpBufferCounter = 0f;
            isJumping = true;
            doubleJump = false;
            vertical = djumpPower;
            jumpCounter = 0;
        }
    }

    void WhileJump()
    {
        if (isJumping)
        {
            jumpTime += Time.deltaTime;
          

            if (jumpTime >= maxJumpTime || !input.jumpPressed)
            {
                vertical = 0f;
                jumpTime = 0f;
                isJumping = false;
            }

            rigid.velocity = new Vector2(rigid.velocity.x, vertical);
        }
        else
        {
            vertical = 0f;
        }

        if (vertical > 0f)
        {
            anim.SetBool("isJump", true);
        }
        else
        {
            anim.SetBool("isJump", false);
        }
    }

    private IEnumerator Dashing()
    {
        isDashing = true;
        canDash = false;
        anim.SetBool("isDash", true);
        SoundManager.PlaySound(SoundType.DASH, 0.4f);
        dashingDir = new Vector2(input.horizontal, 0f);
        if (dashingDir == Vector2.zero)
        {
            dashingDir = new Vector2(transform.localScale.x, 0);
        }
        rigid.velocity = dashingDir.normalized * dashSpeed;
        yield return new WaitForSeconds(dashTime);
        anim.SetBool("isDash", false);
        isDashing = false;
        yield return new WaitForSeconds(1f);
        if (!isDamaged) canDamage = true;
        canDash = true;
    }

    void PickUpObject()
    {
        // 근처에 있는 물체 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, holdRange);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Pickupable"))
            {
                // 들고 있는 물체 설정
                anim.SetBool("isGrab", true);
                heldObject = col.GetComponent<Rigidbody2D>();
                heldObject.mass = 0;
                col.transform.SetParent(transform);
                if (heldObject != null)
                {
                    isHolding = true;
                    heldObject.gravityScale = 0f;
                }
                break;
            }
        }

    }

    void ReleaseObject()
    {
        // 물체 놓기
        if (heldObject != null)
        {
            anim.SetBool("isGrab", false);
            heldObject.mass = 1000;
            heldObject.transform.SetParent(null);
            heldObject.gravityScale = 1f;
            heldObject = null;
            isHolding = false;
            SoundManager.PlaySound(SoundType.JUMP, 0.3f, 0);
        }
    }

    void UpdateHeldObjectPosition()
    {
        // 들고 있는 물체를 들고 있는 위치로 이동
        if (heldObject != null)
        {
            heldObject.velocity = Vector2.zero;
            heldObject.angularVelocity = 0f;
            heldObject.MovePosition(holdPosition.position);
        }
    }

    public bool holding()
    {
        return isHolding;
    }

    public void TakeDamage(int dmg)
    {
        if (canDamage)
        {           
            canAct = false;
            canDamage = false;
            isDamaged = true;
            rigid.velocity = Vector2.zero;
            StartCoroutine(UIManager.Instance.ShowBloodScreen());
            anim.SetBool("isRun", false);
            if(GameManager.Instance.hp <= 0)
            {
                StartCoroutine(Death());
            }
            else
            {
                StartCoroutine(InvinsibleTime());
            }
        }
    }

    private IEnumerator InvinsibleTime()
    {
        anim.SetTrigger("isHurt");
        yield return new WaitForSeconds(0.5f);
        canAct = true;
        yield return new WaitForSeconds(1f);
        canDamage = true;
        isDamaged = false;
    }

    private IEnumerator Death()
    {
        isDead = true;
        GameManager.Instance.isDead = true;
        Time.timeScale = 1f;
        anim.SetTrigger("isDead");
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(UIManager.Instance.ActivateDeathMassage());
    }

    public void Respawn()
    {
        if(isDead)
        {
            isDead = false;
            isDamaged = false;
            canAct = true;
            canDamage = true;
            anim.Play("Idle");
        }
    }

    public IEnumerator ChangeScene(Vector2 exitDir, float delay)
    {
        if(exitDir.y > 0)
        {
            rigid.velocity = jumpPower * exitDir;
        }
        if(exitDir.x != 0)
        {
            input.horizontal = exitDir.x > 0 ? 1 : -1;
        }

        Flip();
        yield return new WaitForSeconds(delay);
    }

    private IEnumerator Attack()
    {
        //SoundManager.instance.PlaySfx(1);
        canAct = false;
        anim.SetTrigger("isAttack");
        yield return null;
        canAct = true;

       
        timeBtwAttack = startTimeBtwAttack;
    }

    private IEnumerator FireBall()
    {
        canAct = false;
        anim.SetTrigger("FireBall");       

        yield return new WaitForSeconds(1f);

        canAct = true;
        timeBtwFire = startTimeBtwFire;
    }

    public void ShotFireBall()
    {
        PlayerBullet bullet;
        GameObject select = null;

        select = ObjectPoolManager.instance.GetFireBallObject(new Vector2(attackPos.position.x, attackPos.position.y), Quaternion.identity);
        select.transform.localScale = new Vector3(gameObject.transform.localScale.x, 1, 1);
        bullet = select.GetComponent<PlayerBullet>();
        if (gameObject.transform.localScale.x > 0)
        {
            bullet.rb.velocity = new Vector2(bullet.speed, 0);
        }
        else if (gameObject.transform.localScale.x < 0)
        {
            bullet.rb.velocity = new Vector2(bullet.speed * -1, 0);
        }
    }

    public void HitCheck()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].gameObject.tag == "Enemy") // 적과 충돌 시 데미지 처리
            {
                enemiesToDamage[i].GetComponent<Enemy>().Attacked(damage, transform.position);
                ObjectPoolManager.instance.GetEffectObject(enemiesToDamage[i].GetComponent<Enemy>().transform.position, 
                                                        enemiesToDamage[i].GetComponent<Enemy>().transform.rotation);
                SoundManager.PlaySound(SoundType.HURT, 0.3f, 1);
            }
            else if (enemiesToDamage[i].gameObject.tag == "Boss")
            {
                enemiesToDamage[i].GetComponent<Boss>().Attacked(damage, attackPos.position);
                ObjectPoolManager.instance.GetEffectObject(enemiesToDamage[i].GetComponent<Boss>().transform.position,
                                                        enemiesToDamage[i].GetComponent<Boss>().transform.rotation);
                SoundManager.PlaySound(SoundType.HURT, 0.3f, 1);
            }
            else if (enemiesToDamage[i].gameObject.tag == "EnemyDestroyableBullet")
            {
                Debug.Log("적의 투사체에 적중");
                enemiesToDamage[i].gameObject.SetActive(false);
            }
        }
    }



    void Setgravity()
    {
        //점프시 및 돌진시 중력에 영향을 받지 않음
        if (isJumping || isDashing) { rigid.gravityScale = 0; }
        else { rigid.gravityScale = gravity; }
    }

    // 현재 애니메이션 상태 확인
    void CheckAnime()
    {
      
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // 공격상태확인
        if (stateInfo.IsName("Attack") && IsGrounded())
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
        }
        else if (stateInfo.IsName("Attack2") && IsGrounded())
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
        }
        // 다른 애니메이션 상태에 대한 확인 코드 추가 가능
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    // one-way platform(아래키 눌러 플랫폼 아래로 이동)
    private IEnumerator DisableCollision()
    {
        if (currentOneWayPlatform.GetComponent<Collider2D>() != null)
        {
            Collider2D platformCollider = currentOneWayPlatform.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(playerCollider, platformCollider);
            yield return new WaitForSeconds(0.25f);
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }
        else if (currentOneWayPlatform.GetComponent<TilemapCollider2D>() != null)
        {
            TilemapCollider2D platformCollider = currentOneWayPlatform.GetComponent<TilemapCollider2D>();
            Physics2D.IgnoreCollision(playerCollider, platformCollider);
            yield return new WaitForSeconds(0.25f);
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canDamage)
        {
            if (collision.CompareTag("EnemyBody"))
            {
                GameManager.Instance.PlayerHit(1);
            }
            else if (collision.CompareTag("Trap"))
            {
                GameManager.Instance.PlayerHit(GameManager.Instance.maxHp);
            }
            else if (collision.CompareTag("Lava"))
            {
                GameManager.Instance.PlayerHit(1);
            }
        }
    }

    private void ResetAnimeParameter()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isJump", isJump);
        anim.SetBool("isRun", isRun);
        anim.SetBool("isDash", isDash);
        anim.SetBool("isGrab", isGrab);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.DrawCube(groundCheck.position, boxSize);
    }
}
