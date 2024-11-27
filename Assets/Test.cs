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
        nextScene = GameManager.Instance.respawnScene;
        //GameManager.Instance.LoadPlayerInfo(DataManager.Instance.currentData);
        MapManager.Instance.LoadMapInfo();
        GameManager.Instance.isRespawn = true;
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
    }
}
