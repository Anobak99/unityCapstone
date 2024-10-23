using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPosSave : MonoBehaviour
{
    void Awake()
    {
        // ����� ��ġ�� ������ �� ��ġ�� �̵�
        if (PlayerPrefs.HasKey("ObjectPosX") && PlayerPrefs.HasKey("ObjectPosY") && PlayerPrefs.HasKey("ObjectPosZ"))
        {
            float x = PlayerPrefs.GetFloat("ObjectPosX");
            float y = PlayerPrefs.GetFloat("ObjectPosY");
            float z = PlayerPrefs.GetFloat("ObjectPosZ");
            transform.position = new Vector3(x, y, z);
        }
    }

    public void SaveObjectPos()
    {
        PlayerPrefs.SetFloat("ObjectPosX", transform.position.x);
        PlayerPrefs.SetFloat("ObjectPosY", transform.position.y);
        PlayerPrefs.SetFloat("ObjectPosZ", transform.position.z);
        PlayerPrefs.Save();
    }
}
