using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoDoor : MonoBehaviour
{
    [SerializeField] private int doorNum;

    private void OnEnable()
    {
        if (DataManager.Instance.currentData.volcano_SwitchDoor[doorNum])
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (DataManager.Instance.currentData.volcano_SwitchDoor[doorNum])
            {
                gameObject.SetActive(false);
            }
        }
    }
}
