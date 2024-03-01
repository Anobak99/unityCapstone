using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public int num;

    private void OnEnable()
    {
        if (SwitchManager.Instance.openedDoor[num])
        {          
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (SwitchManager.Instance.doorSwitch[num])
            {
                SwitchManager.Instance.openedDoor[num] = true;
                gameObject.SetActive(false);
            }
        }
    }
}
