using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{   
    private Animator animator;
    public Rigidbody2D rb;
    public float speed = 5f;

    public int damage = 1;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (collision.gameObject.tag == "Enemy")
        {
            rb.velocity = Vector2.zero;
            collision.GetComponent<Enemy>().Attacked(damage, gameObject.transform.position);
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
