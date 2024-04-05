using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequentialMapBlocks : MonoBehaviour
{
    public float activationDelay = 2f; // 발판이 활성화되는 시간 간격
    public float deactivationDelay = 1f; // 발판이 비활성화되는 시간 간격
    public bool isActive = false; // 발판의 초기 활성화 상태
    public SequentialMapBlocks nextBlocks; // 다음 발판의 참조

    void Start()
    {
        StartCoroutine(ActivateDeactivateBlocks());
    }

    IEnumerator ActivateDeactivateBlocks()
    {
        while (true)
        {
            // 발판이 활성화되어 있을 때 비활성화로 전환
            if (isActive)
            {
                yield return new WaitForSeconds(deactivationDelay);
                gameObject.SetActive(false);
                isActive = false;

                // 다음 발판 활성화
                if (nextBlocks != null)
                    nextBlocks.ActivateBlocks();
            }
            else // 발판이 비활성화되어 있을 때 활성화로 전환
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