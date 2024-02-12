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

    // Start is called before the first frame update
    void Start()
    {
        maxHp = 5;
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPlayerComp()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }
    public void PlayerHit(int dmg)
    {
        playerController.TakeDamage(dmg);
    }

    public void RespawnPlayer()
    {
        player.transform.position = respawnPoint;
        hp = maxHp;
        playerController.Respawn();
        StartCoroutine(UIManager.Instance.DeactivateDeathMassage());
    }
}
