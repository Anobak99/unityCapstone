using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float horizontal;
    public bool jumpPressed;
    public float jumpBufferTime = 0.2f;
    [SerializeField] public float jumpBufferCounter;
    public bool dashInput;


    protected virtual void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        jump();
        Dash();
    }

    void jump()
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
}
