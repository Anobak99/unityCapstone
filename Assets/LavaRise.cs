using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LavaRise : MonoBehaviour
{
    public float riseSpeed = 2f;       // ����� ����ϴ� �ӵ�
    public float maxHeight = 5f;       // ����� ������ �ִ� ���� (������)
    public float minHeight = 1f;       // ����� ������ ���� ���� (������)
    public float waitTime = 2f;        // ��°� �ϰ� ������ ��� �ð�


    void Awake()
    {
        StartCoroutine(ResizeLava());
    }

    private IEnumerator ResizeLava()
    {
        Vector3 scale = transform.localScale;

        // ����� ����� ��
        while (scale.y <= maxHeight)
        {
            Debug.Log("���");
            scale.y += riseSpeed * Time.deltaTime;
            transform.localScale = scale;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(waitTime);

        while (scale.y >= minHeight)
        {
            Debug.Log("�ϰ�");
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
