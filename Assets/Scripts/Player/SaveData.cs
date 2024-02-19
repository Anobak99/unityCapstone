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

    public bool[] doorSwitch = new bool[10]; //문을 여는 열쇠
    public bool[] openedDoor = new bool[10]; //열린 문
    public bool[] openSwitchDoor = new bool[10]; //스위치로 여는 문
    public bool[] abilities = new bool[10]; //능력 잠금해제

}
