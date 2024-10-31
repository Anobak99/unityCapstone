using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class StageChanger : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private string curScene;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 nextPos;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject==player)
        {
            if (GameManager.Instance.currentScene == curScene)
            {
                GameManager.Instance.posToLoad = nextPos;
                GameManager.Instance.nextScene = true;
                GameManager.Instance.isRespawn = true;
                StartCoroutine(nextStage());
            }
        }
    }

    IEnumerator nextStage()
    {
        GameManager.Instance.gameState = GameManager.GameState.Event;
        StartCoroutine(UIManager.Instance.screenFader.Fade(ScreenFader.FadeDirection.In, 0f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(GameManager.Instance.ChangeScene(nextScene));
    }
}

