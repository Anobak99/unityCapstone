using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boss1_bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float jumpHeight;
    public float downPoint;
    public bool isAttack3;

    public IEnumerator Jump()
    {
        ///animator.Play("Idle");
        rb.gravityScale = 0f;
        if(!isAttack3)
        {
            transform.localPosition = new Vector2(0f, -0.5f);
            downPoint = transform.localPosition.x + Random.Range(-5f, 5f);
        }
        else
        {
            transform.localPosition = new Vector2(0f, 0.5f);
        }
        rb.velocity = new Vector2(downPoint, jumpHeight);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector2(downPoint, 0f);
        rb.gravityScale = 10f;
    }

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
        if (collision.CompareTag("Player") || collision.CompareTag("Ground"))
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
        isAttack3 = false;
        gameObject.SetActive(false);
    }
}
