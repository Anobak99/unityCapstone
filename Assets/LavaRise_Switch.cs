using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRise_Switch : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private GameObject wall;
    public GameObject lavaRise;

    [SerializeField] private int switchNum = 0;
    [SerializeField] private int doorkeyNum = 3;

    private bool isPlayer = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();

        if (SwitchManager.Instance.volcanoSwitch[switchNum] == true)
        {
            col.enabled = false;
        }
    }

    private void Update()
    {
        if (isPlayer && Input.GetKeyDown(KeyCode.G))
        {
            animator.SetBool("Lever", true);
            SoundManager.PlaySound(SoundType.SFX, 1, 7);
            CameraShake.Instance.OnShakeCamera(0.5f, 0.5f);
            RisingLava.lavaSwitch = false;
            SwitchManager.Instance.volcanoSwitch[switchNum] = true;
            SwitchManager.Instance.openedDoor[doorkeyNum] = true;
            col.enabled = false;
            wall.SetActive(false);
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
