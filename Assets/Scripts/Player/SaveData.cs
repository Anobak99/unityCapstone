using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public string fileName = "";
    public int maxHp;
    public string saveScene;
    public string areaName;
    public float savePosX;
    public float savePosY;

    #region Volcano Switch
    [Header("Volcano")]
    public bool[] volcano_Switch = new bool[5];
    public bool[] volcano_SwitchDoor = new bool[5];
    #endregion

    public bool[] doorSwitch = new bool[10]; //���� ���� ����
    public bool[] openedDoor = new bool[10]; //���� ��
    public bool[] openSwitchDoor = new bool[10]; //����ġ�� ���� ��
    public bool[] abilities = new bool[10]; //�ɷ� �������
    public bool[,] mapData = new bool[20, 20]; //�̴ϸ� ����
    public bool[] boss = new bool[4]; //óġ�� ����
}
