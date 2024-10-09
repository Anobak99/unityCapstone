using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class RisingLava : MonoBehaviour
{
    public Tilemap lavaTilemap;           // ��� Ÿ�ϸ�
    public TileBase lavaTile;             // ��� Ÿ��
    public float riseSpeed = 0.5f;        // ����� �������� �ӵ� (�ʴ� Ÿ�� ����)
    public int maxHeight = 10;            // ����� ������ �ִ� ���� (Ÿ�� ����)
    public int minHeight = 0;             // ����� ������ ���� ���� (Ÿ�� ����)
    public float waitTime = 2f;           // ��°� �ϰ� ������ ��� �ð�
    public int tilemapWidth = 10;         // Ÿ�ϸ��� ���� ���� (x�� ����)
    public int tilemapWidth_start;        // Ÿ�ϸ��� ���� x ��ġ
    public static bool lavaSwitch = true;

    private int currentHeight;            // ���� ��� ����
    private bool rising = true;           // ����� ��� ������ ����

    void Start()
    {
        // ó�� ������ ���� ���� ���̿��� ����
        currentHeight = minHeight;
        StartCoroutine(UpdateLava());
    }

    IEnumerator UpdateLava()
    {
        while (true)
        {
            if (rising )
            {
                // ����� ���� �������� ��
                if (currentHeight < maxHeight)
                {
                    currentHeight++;
                    UpdateLavaTiles();
                    yield return new WaitForSeconds(riseSpeed);
                }
                else
                {
                    // �ִ� ���̿� �������� �� ��� ���߰� �ϰ�
                    rising = false;
                    yield return new WaitForSeconds(waitTime);
                }
            }
            else if (!rising && lavaSwitch)
            {
                // ����� �Ʒ��� ������ ��
                if (currentHeight > minHeight)
                {
                    currentHeight--;
                    UpdateLavaTiles();
                    yield return new WaitForSeconds(riseSpeed);
                }
                else
                {
                    // ���� ���̿� �������� �� ��� ���߰� ���
                    rising = true;
                    yield return new WaitForSeconds(waitTime);
                }
            }
            else if (!lavaSwitch)
            {
                RemoveLavaTiles();
                break;
            }
        }

        yield return null;
    }

    void UpdateLavaTiles()
    {
        // Ÿ�ϸʿ��� �ּ� ���̺��� ���� ���̱��� Ÿ���� ����
        for (int y = minHeight; y <= maxHeight; y++)
        {
            for (int x = tilemapWidth_start; x < tilemapWidth; x++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (y <= currentHeight)
                {
                    // ���� ���� ������ Ÿ�Ͽ� ��� Ÿ���� ����
                    lavaTilemap.SetTile(tilePosition, lavaTile);
                }
                else
                {
                    // ���� ���� ���� Ÿ���� ����
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
                    // ���� ���� ������ Ÿ�Ͽ� ��� Ÿ���� ����
                    lavaTilemap.SetTile(tilePosition, null);
                }
                else
                {
                    // ���� ���� ���� Ÿ���� ����
                    lavaTilemap.SetTile(tilePosition, null);
                }
            }
        }
    }
}
