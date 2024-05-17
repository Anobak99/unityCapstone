using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{
    public float liftingForce = 5f; // ��� ��
    public float holdRange = 1.5f;
    public Transform holdPosition;   // ��� �ִ� ��ġ

    private Rigidbody2D heldObject;  // ��� �ִ� ��ü�� Rigidbody2D
    private bool isHolding = false;  // ���� ������ ��� �ִ��� ����

    void Update()
    {
        // ���� ���/���� ���
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

        // ������ ��� �ִ� ��� ��ġ ������Ʈ
        if (isHolding)
        {
            UpdateHeldObjectPosition();
        }
    }

    void PickUpObject()
    {
        // ��ó�� �ִ� ��ü ã��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, holdRange);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Pickupable"))
            {
                // ��� �ִ� ��ü ����
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
        // ��ü ����
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
        // ��� �ִ� ��ü�� ��� �ִ� ��ġ�� �̵�
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
