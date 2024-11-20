using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActiveEnvent : MonoBehaviour
{
    public GameObject childObject; // 활성화할 자식 오브젝트를 지정

    public void ActivateChildObject()
    {
        if (childObject != null)
        {
            childObject.SetActive(true);
        }
    }
}
