using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public SaveData currentData;
    private string filePath;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<DataManager>();
                if (obj != null)
                {
                    instance = obj;
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        var objs = FindObjectsOfType<DataManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }

    public bool FileCheck(string name)
    {
        currentData.fileName = name;
        //파일 경로 설정(User\Username\AppData\LocalLow\DefaultCompany\unityCapstone\fileName)
        filePath = Path.Combine(Application.persistentDataPath, currentData.fileName);

        if (File.Exists(filePath))
        {
            string loadData = File.ReadAllText(filePath);
            currentData = JsonConvert.DeserializeObject<SaveData>(loadData);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void FileDelete(string name)
    {
        //파일 경로 설정(User\Username\AppData\LocalLow\DefaultCompany\unityCapstone\fileName)
        filePath = Path.Combine(Application.persistentDataPath, currentData.fileName);
        currentData = new SaveData();

        if (File.Exists(filePath))
        {
            File.Delete(filePath);

            return;
        }
        else
        {
            return;
        }
    }

    public void SaveData()
    {
        GameManager.Instance.SavePlayerInfo(currentData);

        string saveData = JsonConvert.SerializeObject(currentData);
        File.WriteAllText(filePath, saveData);
    }

    public void LoadData()
    {
        string loadData = File.ReadAllText(filePath);
        currentData = JsonConvert.DeserializeObject<SaveData>(loadData);

        GameManager.Instance.LoadPlayerInfo(currentData);
    }
}
