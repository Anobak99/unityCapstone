using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TrapBullet : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    private bool isMove;

    private void OnEnable()
    {
        isMove = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if(isMove)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isMove = false;
            GameManager.Instance.PlayerHit(1);
            StartCoroutine(OnHit());
        }

        if(collision.CompareTag("Ground") || collision.CompareTag("Platform") || collision.CompareTag("Block"))
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
