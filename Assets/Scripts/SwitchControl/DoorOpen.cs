using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public int num;

    private void OnEnable()
    {
        if (DataManager.Instance.currentData.openedDoor[num])
        {          
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (DataManager.Instance.currentData.doorSwitch[num])
            {
                DataManager.Instance.currentData.openedDoor[num] = true;
                gameObject.SetActive(false);
            }
        }
    }
}
