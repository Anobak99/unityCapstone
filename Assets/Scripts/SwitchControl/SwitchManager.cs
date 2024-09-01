using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    //���� ������ ���Ǵ� �� ����ġ���� �����ϱ� ���� ��ũ��Ʈ
    public bool[] doorSwitch = new bool[10]; //���� ���� ����
    public bool[] openedDoor = new bool[10]; //���� ��
    public bool[] openSwitchDoor = new bool[10]; //����ġ�� ���� ��
    public bool[] abilities = new bool[10]; //�ɷ� �������
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

    //�̺�Ʈ ����ġ ���� ����
    public SaveData SaveSwitchInfo(SaveData saveData)
    {
        saveData.doorSwitch = doorSwitch;
        saveData.openedDoor = openedDoor;
        saveData.openSwitchDoor = openSwitchDoor;
        saveData.abilities = abilities;

        return saveData;
    }

    //�̺�Ʈ ����ġ ���� �ҷ�����
    public void LoadSwitchInfo(SaveData loadData)
    {
        doorSwitch = loadData.doorSwitch;
        openedDoor = loadData.openedDoor;
        openSwitchDoor = loadData.openSwitchDoor;
        abilities = loadData.abilities;
    }
}
