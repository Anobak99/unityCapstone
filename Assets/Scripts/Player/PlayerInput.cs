using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float horizontal;
    public bool jumpPressed;
    public float jumpBufferTime = 0.2f;
    [SerializeField] public float jumpBufferCounter;
    public bool dashInput;
    public bool attackInput;
    public bool fireballInput;


    //void Awake()
    //{
    //    var objs = FindObjectsOfType<PlayerInput>();
    //    if (objs.Length != 1)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    DontDestroyOnLoad(gameObject);
    //}

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Jump();
        Dash();
        Attack();
        Pause();
        FireBall();
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
}
