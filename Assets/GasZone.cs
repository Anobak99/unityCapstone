using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasZone : MonoBehaviour
{
    public Vector2 forceDirection = new Vector2(-1, 0); // ������ ���� �и� ����
    public float pushForce = 5f; // �и��� ���� ũ��

    void OnTriggerStay2D(Collider2D other)
    {
        // �÷��̾����� Ȯ�� (Tag�� ���� ����)
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            // �÷��̾ �����ϸ� forceDirection �������� ���� ����
            playerRb.AddForce(forceDirection.normalized * pushForce);
        }
    }
}
