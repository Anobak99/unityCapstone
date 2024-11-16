using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mage_orb : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;


    public void OnEnable()
    {
        animator.Play("Mage_orb_idle");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.PlayerHit(2);
        }
        else if (collision.CompareTag("Block"))
        {
            animator.SetTrigger("Explo");
            StartCoroutine(Hit());
        }
    }

    IEnumerator Hit()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
    }
}
