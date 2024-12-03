using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap maptile; // 2D grid representing the map
    public GameObject playerIcon;
    public GameObject mapCam;
    private Transform location;
    private int currentX, currentY; // Current position of the player

    [SerializeField] private TileBase[] icon_Images;
    [SerializeField] Tilemap iconTile;
    [Serializable]
    public struct SaveInfo
    {
        public int id;
        public string saved_scene;
        public Vector2Int map_Pos;
        public Vector2 saved_pos;
    }
    [SerializeField] public SaveInfo[] curSaveInfo;

    private static MapManager instance;

    public static MapManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<MapManager>();
                if (obj != null)
                {
                    instance = obj;
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        var objs = FindObjectsOfType<MapManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void FindMap()
    {

    }

    //�濡 ���� ��
    public void EnterRoom(Vector3Int pos, int id)
    {
        currentX = pos.x;
        currentY = pos.y;
        //�湮�� ���� ���ٸ� ���� ����
        if(!HasVisited(currentX, currentY))
        {
            maptile.SetColor(pos, new Color(255, 255, 255, 255));
            DataManager.Instance.currentData.mapData[currentX, currentY] = true;
            if(id == 1)
            {
                SetMapIcon(pos, 1);
            }
        }
        //������ �÷��̾� ��ġ ����
        location = playerIcon.transform;
        location.position = new Vector3(currentX + 0.5f, currentY + 0.5f, 0f);
        location = mapCam.transform;
        location.position = new Vector3(currentX + 0.5f, currentY + 0.5f, -10f);
    }

    // �ش� ���� �湮�ߴ� ���� �ִ��� Ȯ��
    public bool HasVisited(int x, int y)
    {
        if (DataManager.Instance.currentData.mapData[x, y])
            return true;
        else
            return false;
    }

    public void ChangeCamPos(Vector2 pos)
    {
        location = playerIcon.transform;
        location.position = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0f);
        location = mapCam.transform;
        location.position = new Vector3(pos.x + 0.5f, pos.y + 0.5f, -10f);
    }

    public void SetMapIcon(Vector3Int pos, int num)
    {
        iconTile.SetTile(pos, icon_Images[num]);
    }

    public bool CheckSavePoint(int id)
    {
        if (HasVisited(curSaveInfo[id].map_Pos.x, curSaveInfo[id].map_Pos.y))
            return true;
        else
            return false;
    }

    public IEnumerator WarpSave(int id)
    {
        GameManager.Instance.posToLoad = curSaveInfo[id].saved_pos;
        GameManager.Instance.nextScene = true;
        GameManager.Instance.isRespawn = true;

        GameManager.Instance.gameState = GameManager.GameState.Event;
        StartCoroutine(UIManager.Instance.screenFader.Fade(ScreenFader.FadeDirection.In, 0f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(GameManager.Instance.ChangeScene(curSaveInfo[id].saved_scene));
    }

    //����� ���Ͽ��� ���� �̹��� ����
    public void LoadMapInfo()
    {
        Vector3Int pos;
        for(int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                pos = new Vector3Int(i, j, 0);
                if (HasVisited(i, j))
                    maptile.SetColor(pos, new Color(255, 255, 255, 255));
                else
                    maptile.SetColor(pos, new Color(255, 255, 255, 0));
            }
        }

        for(int i = 0; i < curSaveInfo.Length; i++)
        {
            pos = new Vector3Int(curSaveInfo[i].map_Pos.x, curSaveInfo[i].map_Pos.y, 0);
            if (HasVisited(pos.x, pos.y))
            {
                SetMapIcon(pos, 1);
            }
        }
    }
}
