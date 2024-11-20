using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public string nextScene;
    void Start()
    {
        DataManager.Instance.FileCheck("saveFile1.json"); //���۸� ���� ����, ���� �׽�Ʈ�� �ڵ�
        StartCoroutine(UIManager.Instance.screenFader.Fade(ScreenFader.FadeDirection.In, 0f));
        DataManager.Instance.SaveData();
        nextScene = GameManager.Instance.respawnScene;
        MapManager.Instance.LoadMapInfo();
        //SceneManager.UnloadSceneAsync("MainMenu");
        GameManager.Instance.isRespawn = true;
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
    }
}