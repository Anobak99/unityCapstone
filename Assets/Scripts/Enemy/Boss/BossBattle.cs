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

    // Start is called before the first frame update
    void Start()
    {
       cam = FindObjectOfType<CameraFollow>();
       preCamBox = cam.camBound;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!SwitchManager.Instance.boss[bossNum] && !BossObject.activeSelf)
        {
            if(collision.CompareTag("Player"))
            {
                GameManager.Instance.gameState = GameManager.GameState.Boss;
                cam.camBound = camPosition;
                BossObject.SetActive(true);
            }
        }
    }

    public void BossDead()
    {
        GameManager.Instance.gameState = GameManager.GameState.Field;
        cam.camBound = preCamBox;
        SwitchManager.Instance.boss[bossNum] = true;
    }

}
