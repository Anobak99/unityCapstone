using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drako_fire : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;


    public void OnEnable()
    {
        animator.SetBool("Explo", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("Explo", true);
            GameManager.Instance.PlayerHit(1);
            StartCoroutine(Hit());
        }
        else if (collision.CompareTag("Block"))
        {
            animator.SetBool("Explo", true);
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
