using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_SensorPlatform : MonoBehaviour
{
    public int doorNum = 1; //
  
    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rigidbody = collision.collider.GetComponent<Rigidbody2D>();

        if (rigidbody != null)
        {
            SwitchManager.Instance.openSwitchDoor[doorNum] = true;
            Debug.Log("¹«°Ô °¨ÁöµÊ");
        }       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
       
        SwitchManager.Instance.openSwitchDoor[doorNum] = false;
        Debug.Log("¹«°Ô »ç¶óÁü");
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CameraShake.Instance.OnShakeCamera(0.1f, 0.1f);
    }


}
