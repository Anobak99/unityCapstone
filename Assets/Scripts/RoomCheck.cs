using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomCheck : MonoBehaviour
{
    public int roomNum;
    public BoxCollider2D camCollider;
    [SerializeField] private string curScene;
    [SerializeField] GameObject roomCamera;
    [SerializeField] GameObject[] objects;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            MapManager.Instance.EnterRoom(roomNum, 0);
            GameManager.Instance.camCollider = camCollider.GetComponent<BoxCollider2D>();
            Debug.Log(SceneManager.GetActiveScene().name);
            if(GameManager.Instance.currentScene != curScene)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(curScene));
                GameManager.Instance.currentScene = SceneManager.GetActiveScene().name;
                GameManager.Instance.mainCam = roomCamera;
                GameManager.Instance.SetPlayerComp();
                ActiveObject();
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
