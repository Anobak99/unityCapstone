using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    //게임 내에서 사용되는 맵 스위치들을 관리하기 위한 스크립트
    public bool[] doorSwitch = new bool[10]; //문을 여는 열쇠
    public bool[] openedDoor = new bool[10]; //열린 문
    public bool[] openSwitchDoor = new bool[10]; //스위치로 여는 문
    public bool[] abilities = new bool[10]; //능력 잠금해제
    public bool[] boss = new bool[2];

    private static SwitchManager instance;

    public static SwitchManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SwitchManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<SwitchManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        var objs = FindObjectsOfType<SwitchManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    //이벤트 스위치 정보 저장
    public SaveData SaveSwitchInfo(SaveData saveData)
    {
        saveData.doorSwitch = doorSwitch;
        saveData.openedDoor = openedDoor;
        saveData.openSwitchDoor = openSwitchDoor;
        saveData.abilities = abilities;

        return saveData;
    }

    //이벤트 스위치 정보 불러오기
    public void LoadSwitchInfo(SaveData loadData)
    {
        doorSwitch = loadData.doorSwitch;
        openedDoor = loadData.openedDoor;
        openSwitchDoor = loadData.openSwitchDoor;
        abilities = loadData.abilities;
    }
}
