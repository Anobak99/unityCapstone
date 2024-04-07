using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap[] maptile; // 2D grid representing the map
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

    public void MapSystem(int width, int height)
    {
        //mapGrid = new Tilemap[width, height];
        currentX = 0;
        currentY = 0;
    }

    // Call this method when the player enters a new room
    public void EnterRoom(int x, int y)
    {
        currentX = x;
        currentY = y;   
        //mapGrid[x, y] = true; // Mark the room as visited
        maptile[x].color = new Color(255, 255, 255, 255);
    }

    // Call this method to check if a room has been visited
    public bool HasVisited(int x, int y)
    {
        //return mapGrid[x, y];
        return true;
    }
}
