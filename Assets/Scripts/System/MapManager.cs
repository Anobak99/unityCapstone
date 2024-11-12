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
        DontDestroyOnLoad(gameObject);
    }

    public void FindMap()
    {

    }

    //방에 진입 시
    public void EnterRoom(Vector3Int pos)
    {
        currentX = pos.x;
        currentY = pos.y;
        //방문한 적이 없다면 지도 갱신
        if(!HasVisited(currentX, currentY))
        {
            maptile.SetColor(pos, new Color(255, 255, 255, 255));
            DataManager.Instance.currentData.mapData[currentX, currentY] = true;
        }
        //지도의 플레이어 위치 갱신
        location = playerIcon.transform;
        location.position = new Vector3(currentX + 0.5f, currentY + 0.5f, 0f);
        location = mapCam.transform;
        location.position = new Vector3(currentX + 0.5f, currentY + 0.5f, -10f);
    }

    // 해당 방이 방문했던 적이 있는지 확인
    public bool HasVisited(int x, int y)
    {
        if (DataManager.Instance.currentData.mapData[x, y])
            return true;
        else
            return false;
    }

    //저장된 파일에서 지도 이미지 갱신
    public void LoadMapInfo()
    {
        Vector3Int pos;
        for(int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                pos = new Vector3Int(i, j, 0);
                if (HasVisited(i, j))
                    maptile.SetColor(pos, new Color(255, 255, 255, 255));
                else
                    maptile.SetColor(pos, new Color(255, 255, 255, 0));
            }
        }
    }
}
