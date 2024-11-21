using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage_orb : MonoBehaviour
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
            GameManager.Instance.PlayerHit(1);
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
