using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCrater : MonoBehaviour
{
    public GameObject platform;
    public GameObject lava;

    public float platform_speed = 2f;           // �÷����� �ӵ�
    public float lava_speed = 2f;               // ��� ���� �ӵ�
    public float waitTime = 2f;                 // ��°� �ϰ� ������ ��� �ð�

    public float platform_maxHeight = 5f;       // �÷����� ������ �ִ� ���� (������)
    public float platform_minHeight = 1f;       // �÷����� ������ ���� ���� (������)
    public float lava_maxHeight = 5f;           // ����� ������ �ִ� ���� (������)
    public float lava_minHeight = 1f;           // ����� ������ ���� ���� (������)


    void Start()
    {
        StartCoroutine(PlatformMove());
        StartCoroutine(LavaMove());
    }

    IEnumerator PlatformMove()
    {
        Vector3 pos = platform.transform.position;

        // ����� ����� ��
        while (pos.y <= platform_maxHeight)
        {
            Debug.Log("���");
            pos.y += platform_speed * Time.deltaTime;
            platform.transform.position = pos;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(waitTime);

        while (pos.y >= platform_minHeight)
        {
            Debug.Log("�ϰ�");
            pos.y -= platform_speed * Time.deltaTime;
            platform.transform.position = pos;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(waitTime);

        platform.transform.position = pos;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(PlatformMove());
    }

    private IEnumerator LavaMove()
    {
        Vector3 scale = lava.transform.localScale;

        // ����� ����� ��
        while (scale.x <= lava_maxHeight)
        {
            Debug.Log("���");
            scale.x += lava_speed * Time.deltaTime;
            lava.transform.localScale = scale;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(waitTime);

        while (scale.x >= lava_minHeight)
        {
            Debug.Log("�ϰ�");
            scale.x -= lava_speed * Time.deltaTime;
            lava.transform.localScale = scale;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(waitTime);

        lava.transform.localScale = scale;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(LavaMove());
    }
}

