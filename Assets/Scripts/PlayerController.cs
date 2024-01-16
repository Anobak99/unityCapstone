using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    public float moveSpeed;

    public float gravity = 5;
    [SerializeField] private float vertical;
    public float jumpPower;
    public float djumpPower;
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
    //��������, ����Ű �Է� �� �Է� ���ӽð�, ���� �����ϸ� ����
    private float jumpBufferTime = 0.2f;
    [SerializeField]private float jumpBufferCounter;
    public bool jumpPressed;

    Rigidbody2D rigid;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();       
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        var DashInput = Input.GetButtonDown("Dash");

        
        rigid.velocity = new Vector2(horizontal * moveSpeed, rigid.velocity.y);

        // ��� 
        if (DashInput && canDash)
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
            if (vertical < 10) vertical += 5f;
            else { vertical += 0.2f; }

            if (jumpTime >= maxJumpTime || !jumpPressed)
            {
                if (vertical > 5) vertical = 5f;
                jumpTime = 0f;
                isJumping = false;
            }

            rigid.velocity = new Vector2(rigid.velocity.x, vertical);
        }


        Setgravity();
        Flip();
        Jump();
    }

    private void FixedUpdate()
    {
        
    }

    // ĳ���� �¿� ����
    void Flip()
    {
        if (horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // �ٴ� üũ
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // ���� �Լ�
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
            jumpPressed = true;

            //if (IsGrounded())
            //{
            //    isJumping = true;
            //    jumpTImeCounter = jumpTime;
            //    rigid.velocity = new Vector2(rigid.velocity.x, jumpPower); // �⺻ ����            
            //    doubleJump = true;
            //}
            //else if (doubleJump)
            //{
            //    rigid.velocity = new Vector2(rigid.velocity.x, djumpPower);
            //    doubleJump = false;
            //}
        }

        //if (Input.GetKey(KeyCode.Space) && isJumping == true)
        //{
        //    if (jumpTImeCounter > 0)
        //    {
        //        rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        //        jumpTImeCounter -= Time.deltaTime;
        //    }
        //    else
        //    {
        //        isJumping = false;
        //    }
        //}


        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpPressed = false;
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }

    void Setgravity()
    {
        //������ �߷¿� ������ ���� ����
        if (isJumping) { rigid.gravityScale = 0; }
        else { rigid.gravityScale = gravity; }
    }
}
