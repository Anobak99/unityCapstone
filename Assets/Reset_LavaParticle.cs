using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_LavaParticle : MonoBehaviour
{
    // 초기 위치를 저장할 변수
    private Vector3 initialPosition;

    void Awake()
    {
        // 오브젝트의 현재 위치를 저장
        initialPosition = transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어에게 용암이 닿았습니다.");
        }
    }

    private void OnDisable()
    {
        transform.localPosition = initialPosition;
    }
}
