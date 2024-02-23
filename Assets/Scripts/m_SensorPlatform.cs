using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_SensorPlatform : MonoBehaviour
{
    CameraShake cs;

    private void Awake()
    {
        cs = GetComponent<CameraShake>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rigidbody = collision.collider.GetComponent<Rigidbody2D>();

        if (rigidbody != null)
        {
            Debug.Log("¹«°Ô °¨ÁöµÊ");

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CameraShake.Instance.OnShakeCamera(0.1f, 0.1f);
    }


}
