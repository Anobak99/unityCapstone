using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<GameManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<GameManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    public string currentScene;
    public GameObject player;
    public PlayerController playerController;
    public int maxHp;
    public int hp;
    public Vector2 respawnPoint;
    public string respawnScene;
    private bool isRespawn;

    void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        maxHp = 5;
        hp = maxHp;
    }

    public void SetPlayerComp() //플레이어 컴포넌트 참조
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        if(currentScene == respawnScene && isRespawn)
        {
            player.transform.position = respawnPoint;
        }
    }
    public void PlayerHit(int dmg) //플레이어 피격처리
    {
        if(playerController.canDamage) //플레이어가 피격 가능일 경우
        {
            hp = Mathf.Clamp(hp - dmg, 0, maxHp);
            UIManager.Instance.UpdateHealth(hp, maxHp);
            playerController.TakeDamage(dmg);
        }
        
    }

    public void RespawnPlayer() //플레이어 부활
    {
        currentScene = respawnScene;
        StartCoroutine(UIManager.Instance.screenFader.FadeAndLoadScene(ScreenFader.FadeDirection.In, respawnScene));
        SetPlayerComp();
        hp = maxHp;
        UIManager.Instance.UpdateHealth(hp, maxHp);
        playerController.Respawn();
        isRespawn = false;
        StartCoroutine(UIManager.Instance.DeactivateDeathMassage());
    }

    //플레이어 정보 저장
    public SaveData SavePlayerInfo(SaveData saveData)
    {
        saveData.maxHp = maxHp;
        saveData.saveScene = respawnScene;
        saveData.savePosition = respawnPoint;

        return saveData;
    }

    //플레이어 정보 불러오기
    public void LoadPlayerInfo(SaveData loadData)
    {
        maxHp = loadData.maxHp;
        respawnPoint = loadData.savePosition;
        respawnScene = loadData.saveScene;
    }
}
