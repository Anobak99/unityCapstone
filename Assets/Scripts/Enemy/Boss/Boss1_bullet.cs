using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Boss1_bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float jumpHeight;
    public float distanceFromPlayer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        //animator.Play("Idle");
        transform.localPosition = Vector2.zero;
        rb.AddForce(new Vector2(distanceFromPlayer, jumpHeight), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //animator.SetTrigger("Break");
            GameManager.Instance.PlayerHit(1);
            StartCoroutine(Hit());
        }

        if (collision.CompareTag("Ground"))
        {
            //animator.SetTrigger("Break");
            StartCoroutine(Hit());
        }
    }

    IEnumerator Hit()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
