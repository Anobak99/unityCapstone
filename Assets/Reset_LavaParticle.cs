using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_LavaParticle : MonoBehaviour
{
    // �ʱ� ��ġ�� ������ ����
    private Vector3 initialPosition;

    void Awake()
    {
        // ������Ʈ�� ���� ��ġ�� ����
        initialPosition = transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� ����� ��ҽ��ϴ�.");
        }
    }

    private void OnDisable()
    {
        transform.localPosition = initialPosition;
    }
}
