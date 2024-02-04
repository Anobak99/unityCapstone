using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public float gravity = 5;
    [SerializeField] private float vertical;
    public float jumpPower;
    public float djumpPower;
    public float maxJumpPower;
    public float jumpTime;
    private bool isJumping;
    private bool doubleJump;
    private float maxJumpTime = 0.25f;
    private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteTimeCounter;

    // 대시
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashTime = 0.5f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;

    private Rigidbody2D rigid;
    private Animator anim;
    private PlayerInput input;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private Collider2D col;

    private bool canDamage;
    private bool canAct;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canDamage = true;
        canAct = true;
    }

    private void Update()
    {
        UpdateJumpVariables();

        if(!canAct) { return; }
        Flip();
        Run();
        Jump();
        Setgravity();

        // 대시 
        if (input.dashInput && canDash)
        {
            Debug.Log("Dash");
            isDashing = true;
            canDash = false;
            StartCoroutine(Dashing());
        }

    }

    private void FixedUpdate()
    {

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
        col = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.8f, 0.5f), 0, groundLayer);
        if (!col)
        {
            anim.SetBool("isGrounded", false);
            return false;
        }
        
        if (col.CompareTag("Platform")) //바닥이 플랫폼일 시
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

    void UpdateJumpVariables()
    {
        if (IsGrounded())
        {
            isJumping = false;
            coyoteTimeCounter = coyoteTime;
            doubleJump = true;
        }
        else
        {
            if (coyoteTimeCounter > 0) coyoteTimeCounter -= Time.deltaTime;
        }

        if (input.jumpBufferCounter > 0) input.jumpBufferCounter -= Time.deltaTime;
    }

    void Jump()
    {
        if (coyoteTimeCounter > 0f && input.jumpBufferCounter > 0f && !isJumping)
        {
            input.jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            isJumping = true;
            vertical = jumpPower;
        }

        if(!IsGrounded() && doubleJump &&  input.jumpBufferCounter > 0f && coyoteTimeCounter < 0f)
        {
            input.jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            isJumping = true;
            doubleJump = false;
            vertical = djumpPower;
        }

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
    }

    public void TakeDamage(int dmg)
    {
        if (canDamage)
        {
            GameManager.Instance.hp -= dmg;
            canAct = false;
            StartCoroutine(InvinsibleTime());
        }
    }

    IEnumerator InvinsibleTime()
    {
        anim.SetTrigger("isHurt");
        yield return new WaitForSeconds(0.5f);
        canAct = true;
        yield return new WaitForSeconds(0.5f);
        canDamage = true;
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

    void Setgravity()
    {
        //점프시 및 돌진시 중력에 영향을 받지 않음
        if (isJumping || isDashing) { rigid.gravityScale = 0; }
        else { rigid.gravityScale = gravity; }
    }
}
