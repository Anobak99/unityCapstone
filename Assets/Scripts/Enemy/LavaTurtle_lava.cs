using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTurtle_lava : MonoBehaviour
{
    [SerializeField] private int dmg;

    void OnEnable()
    {
        StartCoroutine(DisableTime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어에게 용암이 닿았습니다.");           
        }
    }

    private IEnumerator DisableTime()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        transform.localPosition = Vector2.zero;
    }
}
