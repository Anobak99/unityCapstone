using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drako_fire : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;


    public void OnEnable()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        animator.SetBool("Explo", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Block"))
        {
            animator.SetBool("Explo", true);
            GameManager.Instance.PlayerHit(1);
            StartCoroutine(Hit());
        }
    }

    IEnumerator Hit()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
