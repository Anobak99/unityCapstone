using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoor : MonoBehaviour
{
    public int num;

    private void Update()
    {
        if (SwitchManager.Instance.openSwitchDoor[num])
        {
            Debug.Log("Door Open");
            gameObject.SetActive(false);
        }
        
        if (SwitchManager.Instance.openSwitchDoor[num] == false)
        {
            Debug.Log("Door Close");
            gameObject.SetActive(true);
        }
    }
}
