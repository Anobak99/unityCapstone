using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private GameObject currentOneWayPlatform; // onewayplatform 오브젝트
    private BoxCollider2D playerCollider;

    public float moveSpeed;

    public float gravity = 5;
    [SerializeField] private float vertical;
    public float jumpPower;
    public float djumpPower;
    private float jumpCounter;
    [SerializeField] private float jumpTime;
    private bool isJumping;
    private bool doubleJump;
    private float maxJumpTime = 0.25f;
    private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteTimeCounter;

    // 대시
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.1f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;

    private Rigidbody2D rigid;
    private Animator anim;
    private PlayerInput input;

    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private Vector2 boxSize = new Vector2(0.8f, 0.2f);
    private Collider2D col;

    public float timeBtwAttack;  // 공격 쿨타임 (0이 되면 공격가능)
    public float startTimeBtwAttack = 0.8f; // 공격 쿨타임 설정

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange; // 공격 범위
    public int damage;        // 데미지 수치

    [HideInInspector] public bool canDamage;
    private bool isDamaged;
    private bool canAct;
    private bool isDead;

    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        input = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canDamage = true;
        canAct = true;
        isDead = false;
    }

    private void Update()
    {
        UpdateVariables();
        
        if(!canAct || isDead || Time.timeScale == 0f) { return; }

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
        if (input.dashInput && canDash)
        {
            Debug.Log("Dash");
            isDashing = true;
            canDash = false;
            canDamage = false;
            StartCoroutine(Dashing());
        }

        if (timeBtwAttack <= 0 && input.attackInput &&  !isDashing)
        {          
            StartCoroutine(Attack());
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

        if (input.jumpBufferCounter > 0) input.jumpBufferCounter -= Time.deltaTime;

        if(timeBtwAttack > 0) { timeBtwAttack -= Time.deltaTime; }
    }

    void Jump()
    {
        if (coyoteTimeCounter > 0f && input.jumpBufferCounter > 0f && !isJumping)
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
            Debug.Log("double jump");
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
        canDash = true;
        if(!isDamaged) canDamage = true;
    }

    public void TakeDamage(int dmg)
    {
        if (canDamage)
        {           
            canAct = false;
            canDamage = false;
            isDamaged = true;
            rigid.velocity = Vector2.zero;
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
        yield return new WaitForSeconds(2f);
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

    public void HitCheck()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].gameObject.tag == "Enemy") // 적과 충돌 시 데미지 처리
            {
                enemiesToDamage[i].GetComponent<Enemy>().Attacked(damage, transform.position);
            }
            else if (enemiesToDamage[i].gameObject.tag == "Boss")
            {
                enemiesToDamage[i].GetComponent<Boss>().Attacked(damage, attackPos.position);
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
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canDamage)
        {
            if (collision.CompareTag("EnemyBody"))
            {
                GameManager.Instance.PlayerHit(1);
            }
            else if(collision.CompareTag("Trap"))
                GameManager.Instance.PlayerHit(GameManager.Instance.maxHp);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.DrawCube(groundCheck.position, boxSize);
    }
}
