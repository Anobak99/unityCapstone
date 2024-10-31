using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public GameObject text;
    public Vector2 savedPos;
    [SerializeField] GameObject playerObj;

    private void Awake()
    {
        if(GameManager.Instance.isRespawn)
        {
            playerObj.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                GameManager.Instance.respawnScene = GameManager.Instance.currentScene;
                GameManager.Instance.respawnPoint = savedPos;
                DataManager.Instance.SaveData();
                GameManager.Instance.PlayerHeal(GameManager.Instance.maxHp);
                StartCoroutine(ShowText());
            }
        }
    }

    IEnumerator ShowText()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(2f);
        text.SetActive(false);
    }
}
