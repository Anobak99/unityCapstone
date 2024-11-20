using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCrater : MonoBehaviour
{
    public GameObject platform;
    public GameObject lava;

    public float platform_speed = 2f;           // 플랫폼의 속도
    public float lava_speed = 2f;               // 용암 분출 속도
    public float waitTime = 2f;                 // 상승과 하강 사이의 대기 시간

    public float platform_maxHeight = 5f;       // 플랫폼이 도달할 최대 높이 (포지션)
    public float platform_minHeight = 1f;       // 플랫폼이 도달할 최저 높이 (포지션)
    public float lava_maxHeight = 5f;           // 용암이 도달할 최대 높이 (스케일)
    public float lava_minHeight = 1f;           // 용암이 도달할 최저 높이 (스케일)


    void Start()
    {
        StartCoroutine(PlatformMove());
        StartCoroutine(LavaMove());
    }

    IEnumerator PlatformMove()
    {
        Vector3 pos = platform.transform.position;

        // 용암이 상승할 때
        while (pos.y <= platform_maxHeight)
        {
            Debug.Log("상승");
            pos.y += platform_speed * Time.deltaTime;
            platform.transform.position = pos;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(waitTime);

        while (pos.y >= platform_minHeight)
        {
            Debug.Log("하강");
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

        // 용암이 상승할 때
        while (scale.x <= lava_maxHeight)
        {
            Debug.Log("상승");
            scale.x += lava_speed * Time.deltaTime;
            lava.transform.localScale = scale;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(waitTime);

        while (scale.x >= lava_minHeight)
        {
            Debug.Log("하강");
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

