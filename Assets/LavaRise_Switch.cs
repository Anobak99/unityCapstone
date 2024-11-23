using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRise_Switch : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private GameObject wall;
    public GameObject lavaRise;

    [SerializeField] private int switchNum;
    [SerializeField] private int doorNum;

    private bool isUse = false;
    private bool isPlayer = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();

        if (DataManager.instance.currentData.volcano_Switch[switchNum] == true)
        {
            col.enabled = false;
        }
    }

    private void Update()
    {
        if (!isUse && isPlayer && Input.GetKeyDown(KeyCode.G))
        {
            UseSwitch();
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

    private void UseSwitch()
    {
        isUse = true;
        animator.SetBool("Lever", true);
        SoundManager.PlaySound(SoundType.SFX, 1, 7);
        CameraShake.Instance.OnShakeCamera(0.5f, 0.5f);
        DataManager.instance.currentData.volcano_Switch[switchNum] = true;
        DataManager.instance.currentData.volcano_SwitchDoor[doorNum] = true;
        col.enabled = false;
        wall.SetActive(false);
    }
}
