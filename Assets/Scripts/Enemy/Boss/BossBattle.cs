using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour 
{
    private CameraFollow cam;
    public BoxCollider2D camPosition;
    private BoxCollider2D preCamBox;
    public float camSpeed;
    public GameObject BossObject;
    private int bossNum;
    public GameObject Door1;
    public GameObject Door2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!SwitchManager.Instance.boss[bossNum] && !BossObject.activeSelf)
        {
            if(collision.CompareTag("Player"))
            {
                cam = FindObjectOfType<CameraFollow>();
                preCamBox = cam.camBound;
                GameManager.Instance.gameState = GameManager.GameState.Event;
                cam.camBound = camPosition;
                BossObject.SetActive(true);
                Door1.SetActive(true);
                Door2.SetActive(true);
            }
        }
    }

    public void BossDead()
    {
        GameManager.Instance.gameState = GameManager.GameState.Field;
        cam.camBound = preCamBox;
        Door1.SetActive(false);
        Door2.SetActive(false);
        SwitchManager.Instance.boss[bossNum] = true;
    }

}
