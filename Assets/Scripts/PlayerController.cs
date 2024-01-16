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

    private float maxJumpTime = 0.3f;
    //코요테타임, 땅에서 벗어난 이후 점프 가능한 시간 
    private float coyoteTime = 0.2f; 
    [SerializeField] private float coyoteTimeCounter;
    //점프버퍼, 점프키 입력 후 입력 지속시간, 땅에 도착하면 점프
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

        Setgravity();
        Flip();
        Jump();
    }

    private void FixedUpdate()
    {
        
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
                if(vertical > 5) vertical = 5f;
                jumpTime = 0f;
                isJumping = false;
            }

            rigid.velocity = new Vector2(rigid.velocity.x, vertical);
        }
        rigid.velocity = new Vector2(horizontal * moveSpeed, rigid.velocity.y);
    }

    // 캐릭터 좌우 반전
    void Flip()
    {
        if (horizontal > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (horizontal < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    // 바닥 체크
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // 점프 함수
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
            //    rigid.velocity = new Vector2(rigid.velocity.x, jumpPower); // 기본 점프            
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

    void Setgravity()
    {
        //점프시 중력에 영향을 받지 않음
        if (isJumping) { rigid.gravityScale = 0; }
        else { rigid.gravityScale = gravity; }
    }
}
