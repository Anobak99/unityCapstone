using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{
    public float liftingForce = 5f; // 들기 힘
    public float holdRange = 1.5f;
    public Transform holdPosition;   // 들고 있는 위치

    private Rigidbody2D heldObject;  // 들고 있는 물체의 Rigidbody2D
    private bool isHolding = false;  // 현재 물건을 들고 있는지 여부

    void Update()
    {
        // 물건 들기/놓기 토글
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isHolding)
            {
                ReleaseObject();
            }
            else
            {
                PickUpObject();
            }
        }

        // 물건을 들고 있는 경우 위치 업데이트
        if (isHolding)
        {
            UpdateHeldObjectPosition();
        }
    }

    void PickUpObject()
    {
        // 근처에 있는 물체 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, holdRange);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Pickupable"))
            {
                // 들고 있는 물체 설정
                heldObject = col.GetComponent<Rigidbody2D>();
                heldObject.mass = 0;
                col.transform.SetParent(transform);
                if (heldObject != null)
                {
                    isHolding = true;
                    heldObject.gravityScale = 0f;
                }               
                break;
            }
        }
        
    }

    void ReleaseObject()
    {
        // 물체 놓기
        if (heldObject != null)
        {
            heldObject.mass = 1;
            heldObject.transform.SetParent(null);
            heldObject.gravityScale = 1f;
            heldObject = null;
            isHolding = false;
        }
    }

    void UpdateHeldObjectPosition()
    {
        // 들고 있는 물체를 들고 있는 위치로 이동
        if (heldObject != null)
        {
            heldObject.velocity = Vector2.zero;
            heldObject.angularVelocity = 0f;
            heldObject.MovePosition(holdPosition.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, holdRange);
    }
}
