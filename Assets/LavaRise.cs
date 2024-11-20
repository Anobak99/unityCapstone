using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LavaRise : MonoBehaviour
{
    public float riseSpeed = 2f;       // 용암이 상승하는 속도
    public float maxHeight = 5f;       // 용암이 도달할 최대 높이 (스케일)
    public float minHeight = 1f;       // 용암이 도달할 최저 높이 (스케일)
    public float waitTime = 2f;        // 상승과 하강 사이의 대기 시간


    void Awake()
    {
        StartCoroutine(ResizeLava());
    }

    private IEnumerator ResizeLava()
    {
        Vector3 scale = transform.localScale;

        // 용암이 상승할 때
        while (scale.y <= maxHeight)
        {
            Debug.Log("상승");
            scale.y += riseSpeed * Time.deltaTime;
            transform.localScale = scale;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(waitTime);

        while (scale.y >= minHeight)
        {
            Debug.Log("하강");
            scale.y -= riseSpeed * Time.deltaTime;
            transform.localScale = scale;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(waitTime);

        transform.localScale = scale;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ResizeLava());
    }

}
