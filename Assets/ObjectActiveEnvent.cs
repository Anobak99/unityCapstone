using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActiveEnvent : MonoBehaviour
{
    public GameObject childObject; // Ȱ��ȭ�� �ڽ� ������Ʈ�� ����

    public void ActivateChildObject()
    {
        if (childObject != null)
        {
            childObject.SetActive(true);
        }
    }
}
