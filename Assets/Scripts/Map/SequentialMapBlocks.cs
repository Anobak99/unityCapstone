using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequentialMapBlocks : MonoBehaviour
{
    public float activationDelay = 2f; // ������ Ȱ��ȭ�Ǵ� �ð� ����
    public float deactivationDelay = 1f; // ������ ��Ȱ��ȭ�Ǵ� �ð� ����
    public bool isActive = false; // ������ �ʱ� Ȱ��ȭ ����
    public SequentialMapBlocks nextBlocks; // ���� ������ ����

    void Start()
    {
        StartCoroutine(ActivateDeactivateBlocks());
    }

    IEnumerator ActivateDeactivateBlocks()
    {
        while (true)
        {
            // ������ Ȱ��ȭ�Ǿ� ���� �� ��Ȱ��ȭ�� ��ȯ
            if (isActive)
            {
                yield return new WaitForSeconds(deactivationDelay);
                gameObject.SetActive(false);
                isActive = false;

                // ���� ���� Ȱ��ȭ
                if (nextBlocks != null)
                    nextBlocks.ActivateBlocks();
            }
            else // ������ ��Ȱ��ȭ�Ǿ� ���� �� Ȱ��ȭ�� ��ȯ
            {
                yield return new WaitForSeconds(activationDelay);
                gameObject.SetActive(true);
                isActive = true;
            }
        }
    }

    public void ActivateBlocks()
    {
        StartCoroutine(ActivateDeactivateBlocks());
    }
}