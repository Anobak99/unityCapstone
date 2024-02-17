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
    public PlayerController playerController;
    public int maxHp;
    public int hp;
    public Vector2 respawnPoint;
    public string respawnScene;
    public bool isRespawn;
    [HideInInspector] public bool isDead;

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

    public void SetPlayerComp() //�÷��̾� ������Ʈ ����
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        if(currentScene == respawnScene && isRespawn)
        {
            player.transform.position = respawnPoint;
            isRespawn = false;
        }
        Camera.main.transform.position = player.transform.position;
        StartCoroutine(UIManager.Instance.screenFader.Fade(ScreenFader.FadeDirection.Out));
    }
    

    public void PlayerHit(int dmg) //�÷��̾� �ǰ�ó��
    {
        if(playerController.canDamage) //�÷��̾ �ǰ� ������ ���
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

    public void RespawnPlayer() //�÷��̾� ��Ȱ
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

    //�÷��̾� ���� ����
    public SaveData SavePlayerInfo(SaveData saveData)
    {
        saveData.maxHp = maxHp;
        saveData.saveScene = respawnScene;
        saveData.savePosition = respawnPoint;

        return saveData;
    }

    //�÷��̾� ���� �ҷ�����
    public void LoadPlayerInfo(SaveData loadData)
    {
        maxHp = loadData.maxHp;
        respawnPoint = loadData.savePosition;
        respawnScene = loadData.saveScene;
    }
}
