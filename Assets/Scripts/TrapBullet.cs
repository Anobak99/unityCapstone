using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBullet : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    Rigidbody2D rb;
    private bool isMove;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        isMove = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if(isMove)
        {
            rb.velocity = direction * speed;
        }
        else { rb.velocity = Vector2.zero; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isMove = false;
            StartCoroutine(OnHit());
        }
    }

    private IEnumerator OnHit()
    {
        yield return new WaitForSeconds(0.2f);
        transform.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        transform.localPosition = Vector2.zero;
    }
}
