using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoKey : MonoBehaviour
{
    public int key_num;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("SignOn");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("Å° È¹µæ");
                DataManager.Instance.currentData.doorSwitch[key_num] = true;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("SignOff");
        }
    }
}
