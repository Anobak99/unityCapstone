using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    public float moveSpeed;

    public float jumpPower;
    public float djumpPower;
    private float jumpTImeCounter;
    public float jumpTime;
    private bool isJumping;
    private bool doubleJump;

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

        Flip();
        Jump();
    }

    private void FixedUpdate()
    {
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
            if (IsGrounded())
            {
                isJumping = true;
                jumpTImeCounter = jumpTime;
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower); // 기본 점프            
                doubleJump = true;
            }
            else if (doubleJump)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, djumpPower);
                doubleJump = false;
            }
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTImeCounter > 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
                jumpTImeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }


        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }
}
