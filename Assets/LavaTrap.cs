using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrap : MonoBehaviour
{
    [SerializeField] private Transform[] m_Pos; // 함정에 피격되었을 때 이동할 위치
    [SerializeField] private GameObject playerObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (DataManager.instance.currentData.abilities[2]) return; // 흑요석피부 활성화시 리턴

            if (collision.gameObject.GetComponent<PlayerController>().isObsidianSkin) return;
            GameManager.Instance.PlayerHit(1);
            StartCoroutine(MovePos());
        }
    }

    IEnumerator MovePos()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().isRespawn = true;
        UIManager.Instance.blackScreen.SetActive(true);

        playerObj.transform.position = CalcDistance().position;

        yield return new WaitForSeconds(1f);

        UIManager.Instance.blackScreen.SetActive(false);
        GameManager.Instance.player.GetComponent<PlayerController>().isRespawn = false;
    }

    private Transform CalcDistance()
    {
        Transform playerPos = playerObj.transform;
        Transform movePos = null;
        float minDistance = 100;
        float distance;
        for (int i = 0; i < m_Pos.Length; i++)
        {
            distance = Vector2.Distance(playerPos.position, m_Pos[i].position);
            if(minDistance > distance)
            {
                minDistance = distance;
                movePos = m_Pos[i];
            }
        }

        return movePos;
    }
}
