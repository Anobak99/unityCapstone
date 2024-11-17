using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.U2D;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

public class BossBattle : MonoBehaviour 
{
    [SerializeField] private CameraFollow cam;
    [SerializeField] private CinemachineVirtualCamera roomCam;
    [SerializeField] private CinemachineVirtualCamera bossCam;
    [SerializeField] private CinemachineVirtualCamera cutSceneCam;
    [SerializeField] private UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera mainCam;
    public BoxCollider2D camPosition;
    private BoxCollider2D preCamBox;
    public float camSpeed;
    public GameObject BossObject;
    public GameObject Player;
    public int bossNum;
    [SerializeField] private List<GameObject> bossDoor;
    [SerializeField] private List<ParticleSystem> bossParticles;
    public bool camChange;

    public PlayableDirector director;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!DataManager.Instance.currentData.boss[bossNum] && !BossObject.activeSelf)
        //{
        //    if(collision.CompareTag("Player"))
        //    {
        //        if(camChange)
        //        {
        //            bossCam.Follow = GameManager.Instance.player.transform;
        //            bossCam.Priority = 11;
        //            preCamBox = cam.camBound;
        //            cam.camBound = camPosition;
        //        }
        //        GameManager.Instance.gameState = GameManager.GameState.Event;
        //        BossObject.SetActive(true);
        //        foreach (GameObject door in bossDoor)
        //        {
        //            door.SetActive(true);
        //        }
        //        StartCoroutine(DoorEffect());
        //    }
        //}
        
        if (collision.CompareTag("Player"))
        {
             StartCoroutine(BossBattleEnter());
        }
    }

    IEnumerator BossBattleEnter()
    {
        if (!DataManager.Instance.currentData.boss[bossNum] && !BossObject.activeSelf)
        {
            if (camChange)
            {
                //bossCam.Follow = GameManager.Instance.player.transform;
                bossCam.Priority = 11;
                preCamBox = cam.camBound;
                cam.camBound = camPosition;
            }
            yield return StartCoroutine(BossCutScene());
            foreach (GameObject door in bossDoor)
            {
                door.SetActive(true);
            }
            StartCoroutine(DoorEffect());
        }
    }

    IEnumerator BossCutScene()
    {
        Debug.Log("cut");
        GameManager.Instance.gameState = GameManager.GameState.Event;
        BossObject.SetActive(true);
        Player.GetComponent<PlayerController>().canAct = false;
        mainCam.enabled = false;

        // 타임라인 시작
        director.Play();

        // 컷씬이 끝날 때까지 대기
        yield return new WaitForSeconds((float)director.duration);

        
        cutSceneCam.Priority = 9;
        if(BossObject.GetComponent<Animator>() != null)
            Destroy(BossObject.GetComponent<Animator>());
        yield return new WaitForSeconds(0.5f);
        Player.GetComponent<PlayerController>().canAct = true;
        BossObject.GetComponent<Boss>().enabled = true;
        mainCam.enabled = true;
    }

    IEnumerator DoorEffect()
    {
        foreach (GameObject door in bossDoor)
        {
            door.SetActive(true);
        }
        foreach (ParticleSystem door in bossParticles)
        {
            door.Play();
        }
        yield return new WaitForSeconds(1f);
        if(!DataManager.Instance.currentData.boss[bossNum])
            StartCoroutine(DoorEffect());
    }

    public void BossDead()
    {
        GameManager.Instance.gameState = GameManager.GameState.Field;
        if (camChange)
        {
            bossCam.Priority = 9;
            cam.camBound = preCamBox;
        }
        foreach(GameObject door in bossDoor)
        {
            door.SetActive(false);
        }
        DataManager.Instance.currentData.boss[bossNum] = true;
        DataManager.Instance.currentData.doorSwitch[bossNum] = true;
        DataManager.Instance.currentData.openedDoor[bossNum] = true;
    }
}
