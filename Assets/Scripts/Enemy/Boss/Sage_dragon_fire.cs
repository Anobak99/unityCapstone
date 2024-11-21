using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sage_dragon_fire : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;

    public void OnEnable()
    {
        rb.AddForce(Vector2.left * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.PlayerHit(1);
        }
        else if (collision.CompareTag("Block"))
        {
            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

}
