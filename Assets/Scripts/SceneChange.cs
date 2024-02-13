using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    [SerializeField] private string nextScene;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Vector2 exitDirection;
    [SerializeField] private float exitTime;

    [SerializeField] GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SetPlayerComp();
        if (GameManager.Instance.currentScene == nextScene)
        {
            if(exitDirection.x > 0)
            {
                GameManager.Instance.player.transform.localScale = new Vector2(1, 1);
            }
            else if (exitDirection.x < 0)
            {
                GameManager.Instance.player.transform.localScale = new Vector2(-1, 1);
            }
            GameManager.Instance.player.transform.position = startPoint.position;
        }
        ActiveEnemy();
        StartCoroutine(UIManager.Instance.screenFader.Fade(ScreenFader.FadeDirection.Out));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ActiveEnemy()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.currentScene = SceneManager.GetActiveScene().name;

            StartCoroutine(UIManager.Instance.screenFader.FadeAndLoadScene(ScreenFader.FadeDirection.In, nextScene));
        }
    }

}
