using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap maptile; // 2D grid representing the map
    public GameObject playerIcon;
    Transform location;
    private float currentX, currentY; // Current position of the player

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
                else
                {
                    var newObj = new GameObject().AddComponent<MapManager>();
                    instance = newObj;
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

    //현재 미사용
    public void MapSystem(int width, int height)
    {
        //mapGrid = new Tilemap[width, height];
        currentX = 0;
        currentY = 0;
    }

    // Call this method when the player enters a new room
    // 해당 방의 지도 타일을 투명에서 보이도록 전환
    public void EnterRoom(Vector3Int pos)
    {
        currentX = pos.x;
        currentY = pos.y;
        if(!HasVisited(currentX, currentY))
        {
            maptile.SetColor(pos, new Color(255, 255, 255, 255));
        }
        //지도의 플레이어 위치 갱신
        location = playerIcon.transform;
        location.position = new Vector3(currentX + 0.5f, currentY + 0.5f, 0f);
    }

    // Call this method to check if a room has been visited
    // 해당 방이 방문했던 적이 있는지 확인
    public bool HasVisited(float x, float y)
    {
        //return mapGrid[x, y];
        return false;
    }
}
