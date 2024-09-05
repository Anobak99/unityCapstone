using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoor : MonoBehaviour
{
    public int num;

    private void Update()
    {
        if (DataManager.Instance.currentData.openSwitchDoor[num])
        {
            Debug.Log("Door Open");
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        
        if (DataManager.Instance.currentData.openSwitchDoor[num] == false)
        {            
            Debug.Log("Door Close");
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;       
        }
    }
}
