using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SavePoint : MonoBehaviour
{
    public GameObject text;
    public Vector2 savedPos;
    public int savePoint_Id; //Id for this object.
    private int curId; //current Id for seraching another savePoint.
    private bool isWorked;
    [SerializeField] GameObject playerObj;
    [SerializeField] private GameObject savepointScreen;
    [SerializeField] private GameObject mapScreen;
    [SerializeField] private GameObject saveBtn;

    private bool closePlayer;

    private void Awake()
    {
        if(GameManager.Instance.isRespawn)
        {       
            playerObj.SetActive(true);
        }
    }

    private void Update()
    {
        if(mapScreen.activeSelf)
        {

            if (Input.GetButtonDown("Cancel"))
            {
                MoveToSave();
            }

            if (Input.GetButtonDown("Attack"))
            {
                MoveToSave();
                if (curId != savePoint_Id)
                {
                    StartCoroutine(MapManager.Instance.WarpSave(curId));
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
                NextSavePoint();
            
            if(Input.GetKeyDown(KeyCode.LeftArrow))
                PreSavePoint();
        }

        if (closePlayer &&Input.GetKeyDown(KeyCode.G) && !isWorked)
        {
            if (mapScreen.activeSelf)
                return;

            GameManager.Instance.PlayerHeal(GameManager.Instance.maxHp);
            SavePointScreen();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            closePlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            closePlayer = false;
        }
    }


    IEnumerator ShowText()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(2f);
        text.SetActive(false);
    }
    public void SavingData()
    {
        GameManager.Instance.respawnScene = GameManager.Instance.currentScene;
        GameManager.Instance.respawnPoint = savedPos;
        DataManager.Instance.SaveData();
        SavePointScreen();
        StartCoroutine(ShowText());
    }

    public void MoveToSave()
    {
        if(mapScreen.activeSelf)
        {
            mapScreen.SetActive(false);
            isWorked = false;
            GameManager.Instance.playerController.anim.SetTrigger("CastEnd");
            MapManager.Instance.ChangeCamPos(MapManager.Instance.curSaveInfo[savePoint_Id].map_Pos);
            GameManager.Instance.gameState = GameManager.GameState.Field;
            return;
        }
        else
        {
            curId = savePoint_Id;
            savepointScreen.SetActive(false);
            GameManager.Instance.gameState = GameManager.GameState.Menu;
            mapScreen.SetActive(true);
            MapManager.Instance.ChangeCamPos(MapManager.Instance.curSaveInfo[curId].map_Pos);
        }
    }

    private void NextSavePoint()
    {
        do
        {
            curId++;
            if (curId >= MapManager.Instance.curSaveInfo.Length)
                curId = 0;
        } while (!MapManager.Instance.CheckSavePoint(curId));
        MapManager.Instance.ChangeCamPos(MapManager.Instance.curSaveInfo[curId].map_Pos);
        Debug.Log("moveRight");
    }

    private void PreSavePoint()
    {
        do
        {
            curId--;
            if (curId <= 0)
                curId = MapManager.Instance.curSaveInfo.Length - 1;
        } while (!MapManager.Instance.CheckSavePoint(curId));
        MapManager.Instance.ChangeCamPos(MapManager.Instance.curSaveInfo[curId].map_Pos);
        Debug.Log("moveLeft");
    }

    public void SavePointScreen()
    {
        if (savepointScreen.activeSelf)
        {
            isWorked = false;
            GameManager.Instance.playerController.anim.SetTrigger("CastEnd");
            GameManager.Instance.gameState = GameManager.GameState.Field;
            savepointScreen.SetActive(false);
        }
        else
        {
            isWorked = true;
            StartCoroutine(PlayerPray());
        }
    }

    public IEnumerator PlayerPray()
    {
        GameManager.Instance.gameState = GameManager.GameState.Menu;
        GameManager.Instance.playerController.rigid.velocity = Vector2.zero;
        GameManager.Instance.playerController.anim.SetTrigger("isCast");
        yield return new WaitForSeconds(0.5f);
        savepointScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(saveBtn);
    }
}
