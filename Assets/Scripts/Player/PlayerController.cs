using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerInput
{
    public float moveSpeed;

    public float gravity = 5;
    [SerializeField] private float vertical;
    public float jumpPower;
    public float djumpPower;
    public float maxJumpPower;
    private float jumpTImeCounter;
    public float jumpTime;
    private bool isJumping;
    private bool doubleJump;

    // 대시
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashTime = 0.5f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;

    private float maxJumpTime = 0.3f;
    //코요테타임, 땅에서 벗어난 이후 점프 가능한 시간 
    private float coyoteTime = 0.2f; 
    [SerializeField] private float coyoteTimeCounter;

    Rigidbody2D rigid;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    Collider2D col;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();       
    }

    protected override void Update()
    {
        base.Update(); //플레이어 키 입력
        rigid.velocity = new Vector2(horizontal * moveSpeed, rigid.velocity.y);

        // 대시 
        if (dashInput && canDash)
        {
            Debug.Log("Dash");
            isDashing = true;
            canDash = false;
            dashingDir = new Vector2(horizontal, Input.GetAxisRaw("Vertical"));
            if (dashingDir == Vector2.zero)
            {
                dashingDir = new Vector2(transform.localScale.x, 0);
            }

            StartCoroutine(StopDashing());
        }

        if (isDashing)
        {
            rigid.velocity = dashingDir.normalized * dashSpeed;
            return;
        }

        if (IsGrounded())
        {
            canDash = true;
        }


        // 점프
        if (IsGrounded()) { coyoteTimeCounter = coyoteTime; }
        else { if (coyoteTimeCounter > 0) coyoteTimeCounter -= Time.deltaTime; }


        if (jumpBufferCounter > 0) jumpBufferCounter -= Time.deltaTime;

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            isJumping = true;
        }

        if (isJumping)
        {
            jumpTime += Time.deltaTime;
            if(vertical < maxJumpPower)
            {
                if (vertical < 10) vertical += 5f;
                else vertical += 0.2f;
            }

            if (jumpTime >= maxJumpTime || !jumpPressed)
            {
                if (vertical > 5f) vertical = 5f;
                else vertical = 0f;
                jumpTime = 0f;
                isJumping = false;
            }

            rigid.velocity = new Vector2(rigid.velocity.x, vertical);
        }


        Setgravity();
        Flip();
    }

    private void FixedUpdate()
    {
        
    }

    // 캐릭터 좌우 반전
    void Flip()
    {
        if (horizontal > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        }
    }

    // 바닥 체크
    private bool IsGrounded()
    {
        col = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.8f, 0.1f), 0, groundLayer);
        if(col == null) return false; //바닥에 아무것도 없을 시

        if (col.CompareTag("Platform")) //바닥이 플랫폼일 시
        {
            SpecialPlatform platform = col.GetComponent<SpecialPlatform>();
            platform.OnStand();
        }

        return true;
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }

    public IEnumerator ChangeScene(Vector2 exitDir, float delay)
    {
        if(exitDir.y > 0)
        {
            rigid.velocity = jumpPower * exitDir;
        }
        if(exitDir.x != 0)
        {
            horizontal = exitDir.x > 0 ? 1 : -1;
        }

        Flip();
        yield return new WaitForSeconds(delay);
    }

    void Setgravity()
    {
        //점프시 중력에 영향을 받지 않음
        if (isJumping) { rigid.gravityScale = 0; }
        else { rigid.gravityScale = gravity; }
    }
}
