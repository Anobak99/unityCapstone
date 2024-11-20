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

    [SerializeField] private int switchNum;
    private int currentHeight;            // ���� ��� ����
    private bool rising = true;           // ����� ��� ������ ����

    void Awake()
    {        
        // ó�� ������ ���� ���� ���̿��� ����
        currentHeight = minHeight;
        StartCoroutine(UpdateLava());
    }

    private void OnEnable()
    {
        if (SwitchManager.Instance.volcano_Switch[switchNum] == true)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator UpdateLava()
    {
        while (true)
        {
            if (SwitchManager.Instance.volcano_Switch[switchNum] == true)
            {
                RemoveLavaTiles();
                break;
            }

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
            else if (!rising)
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

    private void OnDisable()
    {
        StopCoroutine(UpdateLava());
    }
}
