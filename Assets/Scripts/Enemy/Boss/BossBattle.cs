using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour 
{
    [SerializeField] private CameraFollow cam;
    [SerializeField] private CinemachineVirtualCamera roomCam;
    [SerializeField] private CinemachineVirtualCamera bossCam;
    public BoxCollider2D camPosition;
    private BoxCollider2D preCamBox;
    public float camSpeed;
    public GameObject BossObject;
    public int bossNum;
    public ParticleSystem doorEffect1;
    public ParticleSystem doorEffect2;
    public GameObject Door1;
    public GameObject Door2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!SwitchManager.Instance.boss[bossNum] && !BossObject.activeSelf)
        {
            if(collision.CompareTag("Player"))
            {
                bossCam.Follow = GameManager.Instance.player.transform;
                bossCam.Priority = 11;
                preCamBox = cam.camBound;
                GameManager.Instance.gameState = GameManager.GameState.Event;
                cam.camBound = camPosition;
                BossObject.SetActive(true);
                Door1.SetActive(true);
                Door2.SetActive(true);
                StartCoroutine(DoorEffect());
            }
        }
    }

    IEnumerator DoorEffect()
    {
        doorEffect1.Play();
        doorEffect2.Play();
        yield return new WaitForSeconds(1f);
        if(!SwitchManager.Instance.boss[bossNum])
            StartCoroutine(DoorEffect());
    }

    public void BossDead()
    {
        GameManager.Instance.gameState = GameManager.GameState.Field;
        bossCam.Priority = 9;
        cam.camBound = preCamBox;
        Door1.SetActive(false);
        Door2.SetActive(false);
        SwitchManager.Instance.boss[bossNum] = true;
        SwitchManager.Instance.doorSwitch[bossNum] = true;
        SwitchManager.Instance.openedDoor[bossNum] = true;
    }

}
