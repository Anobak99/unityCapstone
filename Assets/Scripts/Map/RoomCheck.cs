using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomCheck : MonoBehaviour
{
    public Vector3Int roomPos;
    [SerializeField] private CinemachineVirtualCamera roomCam;
    public BoxCollider2D camCollider;
    [SerializeField] private string curScene;
    [SerializeField] private int sceneId = 0;
    [SerializeField] GameObject roomCamera;
    [SerializeField] GameObject[] objects;
    [SerializeField] string areaName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            MapManager.Instance.EnterRoom(roomPos, sceneId);
            GameManager.Instance.camCollider = camCollider.GetComponent<BoxCollider2D>();
            if(GameManager.Instance.currentScene != curScene)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(curScene));
                GameManager.Instance.currentScene = SceneManager.GetActiveScene().name;
                GameManager.Instance.mainCam = roomCamera;
                DataManager.Instance.currentData.areaName = areaName;
                GameManager.Instance.nextScene = false;
                GameManager.Instance.SetPlayerComp();
                ActiveObject();
                roomCam.Follow = GameManager.Instance.player.transform;

                if(!SoundManager.instance.bgmSource.isPlaying)
                {
                    SoundManager.PlayBGMSound(DataManager.instance.currentData.areaName, 0.2f, 0);
                }
            }
        }
    }


    void ActiveObject()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(true);
        }
    }
}
