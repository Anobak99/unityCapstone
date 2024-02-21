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
            Debug.Log("무게 감지됨");
            
            // 여기에 원하는 로직을 추가하세요.
        }
    }
}
