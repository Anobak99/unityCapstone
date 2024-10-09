using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public string nextScene;
    void Start()
    {
        DataManager.Instance.FileCheck("saveFile1.json");
        nextScene = GameManager.Instance.respawnScene;
        DataManager.Instance.SaveData();
        MapManager.Instance.LoadMapInfo();
        //SceneManager.UnloadSceneAsync("MainMenu");
        GameManager.Instance.isRespawn = true;
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
    }
}
