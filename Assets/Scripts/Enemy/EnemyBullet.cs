using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public Rigidbody2D rb;
    public Vector2 moveDir;
    private Animator animator;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(OnHit());
        }
        if (collision.CompareTag("Ground") || collision.CompareTag("Platform"))
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(OnHit());
        }
    }

    private IEnumerator OnHit()
    {
        animator.SetTrigger("Explo");
        yield return new WaitForSeconds(0.2f);
        transform.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        transform.localPosition = Vector2.zero;
    }
}
