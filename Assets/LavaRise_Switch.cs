using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRise_Switch : MonoBehaviour
{
    [HideInInspector] public Animator animator;

    public GameObject lavaRise;
    private bool isPlayer = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isPlayer && Input.GetKeyDown(KeyCode.G))
        {
            animator.SetBool("Lever", true);
            CameraShake.Instance.OnShakeCamera(0.5f, 0.5f);
            RisingLava.lavaSwitch = false;
        }

        //if (!lavaSwitchOn) lavaRise.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isPlayer = true;
        }
    }
}
