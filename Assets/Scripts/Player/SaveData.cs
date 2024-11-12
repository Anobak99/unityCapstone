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

    public bool[] volcanoSwitch = new bool[10]; // 화산 스위치
    public bool[] doorSwitch = new bool[10]; //문을 여는 열쇠
    public bool[] openedDoor = new bool[10]; //열린 문
    public bool[] openSwitchDoor = new bool[10]; //스위치로 여는 문
    public bool[] abilities = new bool[10]; //능력 잠금해제
    public bool[,] mapData = new bool[10, 10]; //미니맵 정보
    public bool[] boss = new bool[3]; //처치한 보스
}
