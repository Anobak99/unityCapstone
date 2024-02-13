using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public SaveData currentData;
    private string filePath;

    // Start is called before the first frame update
    void Start()
    {
        //파일 경로 설정(User\Username\AppData\LocalLow\DefaultCompany\unityCapstone\fileName)
        filePath = Path.Combine(Application.persistentDataPath, currentData.fileName);
    }

    public void SaveData()
    {
        GameManager.Instance.SavePlayerInfo(currentData);
        SwitchManager.Instance.SaveSwitchInfo(currentData);

        string saveData = JsonUtility.ToJson(currentData);
        File.WriteAllText(filePath, saveData);
    }

    public void LoadData()
    {
        string loadData = File.ReadAllText(filePath + currentData.fileName);
        currentData = JsonUtility.FromJson<SaveData>(loadData);

        GameManager.Instance.LoadPlayerInfo(currentData);
        SwitchManager.Instance.LoadSwitchInfo(currentData);
    }
}
