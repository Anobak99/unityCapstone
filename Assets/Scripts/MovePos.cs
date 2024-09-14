using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePos : MonoBehaviour
{
    [SerializeField] private Vector2 m_Pos;
    [SerializeField] private string curScene;
    private Vector2 nextPos;
    [SerializeField] private GameObject playerObj;
    [HideInInspector] public bool isMoved;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance.currentScene == curScene)
            {
                GameManager.Instance.nextScene = true;
                collision.gameObject.SetActive(false);
                nextPos = new Vector2(GameManager.Instance.player.transform.position.x + m_Pos.x,
            GameManager.Instance.player.transform.position.y + m_Pos.y);
                GameManager.Instance.CamOff();
                playerObj.SetActive(true);
                playerObj.transform.position = nextPos;
            }
        }
    }
}
