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

    // ���
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashTime = 0.5f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;

    private float maxJumpTime = 0.3f;
    //�ڿ���Ÿ��, ������ ��� ���� ���� ������ �ð� 
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
        base.Update(); //�÷��̾� Ű �Է�
        rigid.velocity = new Vector2(horizontal * moveSpeed, rigid.velocity.y);

        // ��� 
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


        // ����
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

    // ĳ���� �¿� ����
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

    // �ٴ� üũ
    private bool IsGrounded()
    {
        col = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.8f, 0.1f), 0, groundLayer);
        if(col == null) return false; //�ٴڿ� �ƹ��͵� ���� ��

        if (col.CompareTag("Platform")) //�ٴ��� �÷����� ��
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
        //������ �߷¿� ������ ���� ����
        if (isJumping) { rigid.gravityScale = 0; }
        else { rigid.gravityScale = gravity; }
    }
}
