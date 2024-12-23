using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInput : Subject
{
    public float horizontal;
    public float vertical;
    public bool jumpPressed;
    public float jumpBufferTime = 0.2f;
    [SerializeField] public float jumpBufferCounter;
    public bool dashInput;
    public bool attackInput;
    public bool fireballInput;
    public bool superjumpInput;

    void Update()
    {
        if (DialogueManager.Instance.isDialogue) return;


        if (GameManager.Instance.gameState != GameManager.GameState.Field && GameManager.Instance.gameState != GameManager.GameState.Boss)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Jump();
        Dash();
        Attack();
        FireBall();
        SuperJump();
        Pause();
    }

    void Pause()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(UIManager.Instance.MapOpened())
            {
                UIManager.Instance.MapMenu();
            }
            UIManager.Instance.PauseMenu();
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
            jumpPressed = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumpPressed = false;
        }
    }

    void Dash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            dashInput = true;
        }

        if (Input.GetButtonUp("Dash"))
        {
            dashInput = false;
        }
    }

    void Attack()
    {
        if(Input.GetButtonDown("Attack"))
        {
            attackInput = true;
        }

        if(Input.GetButtonUp("Attack"))
        {
            attackInput = false;
        }
    }

    void FireBall()
    {
        if (Input.GetButtonDown("FireBall"))
        {
            fireballInput = true;
        }

        if (Input.GetButtonUp("FireBall"))
        {
            fireballInput = false;
        }
    }

    void SuperJump()
    {
        if (Input.GetButtonDown("SuperJump"))
        {
            superjumpInput = true;
        }

        if (Input.GetButtonUp("SuperJump"))
        {
            superjumpInput = false;
        }
    }
}
