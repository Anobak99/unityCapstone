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
    public Vector2 posToLoad;
    public string respawnScene;
    [HideInInspector] public bool isRespawn;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool nextScene;

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
        if (isRespawn)
        {
            player.transform.position = posToLoad;
            playerController.Respawn();
            isRespawn = false;
            StartCoroutine(UIManager.Instance.screenFader.Fade(ScreenFader.FadeDirection.Out, 0f));
            gameState = GameState.Field;
        }
        cam.cameraMove = true;
        cam.ChangeCameraPos(new Vector3(player.transform.position.x, player.transform.position.y, -10));
        
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
            currentScene = "";
            posToLoad = respawnPoint;
            DataManager.Instance.LoadData();
            MapManager.Instance.LoadMapInfo();
            StartCoroutine(ChangeScene(respawnScene));
            hp = maxHp;
            UIManager.Instance.UpdateHealth(hp, maxHp);
            isDead = false;
            StartCoroutine(UIManager.Instance.DeactivateDeathMassage());
        }
    }

    public void GoTitle()
    {
        player.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void UnloadAllScenes()
    {
        int count = SceneManager.sceneCount;
        for(int i = 0; i < count; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if(scene.name != "Map _Test_Noa")
               SceneManager.UnloadSceneAsync(scene);
        }
    }

    public IEnumerator ChangeScene(string sceneName)
    {
        UnloadAllScenes();

        SoundManager.StopBGMSound();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncOperation.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    //플레이어 정보 저장
    public SaveData SavePlayerInfo(SaveData saveData)
    {
        saveData.maxHp = maxHp;
        saveData.saveScene = respawnScene;
        saveData.savePosX = respawnPoint.x;
        saveData.savePosY = respawnPoint.y;

        return saveData;
    }

    //플레이어 정보 불러오기
    public void LoadPlayerInfo(SaveData loadData)
    {
        maxHp = loadData.maxHp;
        respawnPoint = new Vector2(loadData.savePosX, loadData.savePosY);
        posToLoad = respawnPoint;
        respawnScene = loadData.saveScene;
    }
}
