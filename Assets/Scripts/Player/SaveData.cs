using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public string fileName = "saveFile1";
    public int maxHp;
    public string saveScene;
    public Vector3 savePosition;

    public bool[] doorSwitch = new bool[10]; //���� ���� ����
    public bool[] openedDoor = new bool[10]; //���� ��
    public bool[] openSwitchDoor = new bool[10]; //����ġ�� ���� ��
    public bool[] abilities = new bool[10]; //�ɷ� �������

}
