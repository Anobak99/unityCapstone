using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_SensorPlatform : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rigidbody = collision.collider.GetComponent<Rigidbody2D>();

        if (rigidbody != null)
        {
            Debug.Log("���� ������");
            
            // ���⿡ ���ϴ� ������ �߰��ϼ���.
        }
    }
}
