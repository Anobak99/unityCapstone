using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathKey : MonoBehaviour
{
    public int num;

    private void OnEnable()
    {
        if (DataManager.Instance.currentData.doorSwitch[num])
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DataManager.Instance.currentData.doorSwitch[num] = true;
            gameObject.SetActive(false);
        }
    }
}
