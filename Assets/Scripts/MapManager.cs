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

    //���� �̻��
    public void MapSystem(int width, int height)
    {
        //mapGrid = new Tilemap[width, height];
        currentX = 0;
        currentY = 0;
    }

    // Call this method when the player enters a new room
    // �ش� ���� ���� Ÿ���� ������ ���̵��� ��ȯ
    public void EnterRoom(Vector3Int pos)
    {
        currentX = pos.x;
        currentY = pos.y;
        if(!HasVisited(currentX, currentY))
        {
            maptile.SetColor(pos, new Color(255, 255, 255, 255));
        }
        //������ �÷��̾� ��ġ ����
        location = playerIcon.transform;
        location.position = new Vector3(currentX + 0.5f, currentY + 0.5f, 0f);
    }

    // Call this method to check if a room has been visited
    // �ش� ���� �湮�ߴ� ���� �ִ��� Ȯ��
    public bool HasVisited(float x, float y)
    {
        //return mapGrid[x, y];
        return false;
    }
}
