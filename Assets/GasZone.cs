using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasZone : MonoBehaviour
{
    public Vector2 forceDirection = new Vector2(-1, 0); // 가스를 통해 밀릴 방향
    public float pushForce = 5f; // 밀리는 힘의 크기

    void OnTriggerStay2D(Collider2D other)
    {
        // 플레이어인지 확인 (Tag로 구분 가능)
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            // 플레이어가 존재하면 forceDirection 방향으로 힘을 가함
            playerRb.AddForce(forceDirection.normalized * pushForce);
        }
    }
}
