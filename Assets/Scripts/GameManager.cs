using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private CameraFollow cam;
    public GameObject mainCam;
    public PlayerController playerController;
    public BoxCollider2D camCollider;
    public int maxHp;
    public int hp;
    public Vector2 respawnPoint;
    public string respawnScene;
    private bool isRespawn;
    [HideInInspector] public bool isDead;
    public enum GameState
    {
        Field, Menu, Boss, Event
    }
    public GameState gameState;

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
        cam = mainCam.GetComponentInChildren<CameraFollow>();
        if (currentScene == respawnScene && isRespawn)
        {
            player.transform.position = respawnPoint;
            isRespawn = false;
        }
        cam.camBound = camCollider; 
        cam.cameraMove = false;
        cam.ChangeCameraPos(new Vector3(player.transform.position.x, player.transform.position.y, -10));
        StartCoroutine(UIManager.Instance.screenFader.Fade(ScreenFader.FadeDirection.Out, 0.3f));
    }
    
    public void CamOff()
    {
        mainCam.SetActive(false);
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

    public void PlayerHeal(int num)
    {
        hp = Mathf.Clamp(hp + num, 0, maxHp);
        UIManager.Instance.UpdateHealth(hp, maxHp);
    }

    public void RespawnPlayer() //플레이어 부활
    {
        if(!isRespawn)
        {
            isRespawn = true;
            currentScene = respawnScene;
            SceneManager.LoadScene(respawnScene);
            hp = maxHp;
            UIManager.Instance.UpdateHealth(hp, maxHp);
            playerController.Respawn();
            isDead = false;
            StartCoroutine(UIManager.Instance.DeactivateDeathMassage());
            SetPlayerComp();
        }
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
