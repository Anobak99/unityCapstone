using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class RisingLava : MonoBehaviour
{
    public Tilemap lavaTilemap;           // 용암 타일맵
    public TileBase lavaTile;             // 용암 타일
    public float riseSpeed = 0.5f;        // 용암이 차오르는 속도 (초당 타일 높이)
    public int maxHeight = 10;            // 용암이 도달할 최대 높이 (타일 단위)
    public int minHeight = 0;             // 용암이 도달할 최저 높이 (타일 단위)
    public float waitTime = 2f;           // 상승과 하강 사이의 대기 시간
    public int tilemapWidth = 10;         // 타일맵의 가로 길이 (x축 범위)
    public int tilemapWidth_start;        // 타일맵의 시작 x 위치
    public static bool lavaSwitch = true;

    private int currentHeight;            // 현재 용암 높이
    private bool rising = true;           // 용암이 상승 중인지 여부

    void Awake()
    {
        if (!lavaSwitch)
        {
            gameObject.SetActive(false);
            return;
        }
        // 처음 시작할 때는 최저 높이에서 시작
        currentHeight = minHeight;
        StartCoroutine(UpdateLava());
    }

    IEnumerator UpdateLava()
    {
        while (true)
        {
            if (!lavaSwitch)
            {
                RemoveLavaTiles();
                break;
            }

            if (rising )
            {
                // 용암을 위로 차오르게 함
                if (currentHeight < maxHeight)
                {
                    Debug.Log("Rise Lava");
                    currentHeight++;
                    UpdateLavaTiles();
                    yield return new WaitForSeconds(riseSpeed);
                }
                else
                {
                    // 최대 높이에 도달했을 때 잠시 멈추고 하강
                    rising = false;
                    yield return new WaitForSeconds(waitTime);
                }
            }
            else if (!rising && lavaSwitch)
            {
                // 용암을 아래로 내리게 함
                if (currentHeight > minHeight)
                {                 
                    currentHeight--;
                    UpdateLavaTiles();
                    yield return new WaitForSeconds(riseSpeed);
                }
                else
                {
                    // 최저 높이에 도달했을 때 잠시 멈추고 상승
                    rising = true;
                    yield return new WaitForSeconds(waitTime);
                }
            }
        }

        yield return null;
    }

    void UpdateLavaTiles()
    {
        // 타일맵에서 최소 높이부터 현재 높이까지 타일을 설정
        for (int y = minHeight; y <= maxHeight; y++)
        {
            for (int x = tilemapWidth_start; x < tilemapWidth; x++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (y <= currentHeight)
                {
                    Debug.Log("Rise Lava");
                    // 현재 높이 이하의 타일에 용암 타일을 설정
                    lavaTilemap.SetTile(tilePosition, lavaTile);
                }
                else
                {
                    // 현재 높이 위의 타일은 비우기
                    lavaTilemap.SetTile(tilePosition, null);
                }
            }
        }
    }

    void RemoveLavaTiles()
    {
        for (int y = minHeight; y <= maxHeight; y++)
        {
            for (int x = tilemapWidth_start; x < tilemapWidth; x++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (y <= currentHeight)
                {
                    Debug.Log("Remove Lava");
                    // 현재 높이 이하의 타일에 용암 타일을 설정
                    lavaTilemap.SetTile(tilePosition, null);
                }
                else
                {
                    // 현재 높이 위의 타일은 비우기
                    lavaTilemap.SetTile(tilePosition, null);
                }
            }
        }
    }

    private void OnDisable()
    {
        StopCoroutine(UpdateLava());
    }
}
