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


    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.currentScene == nextScene)
        {
            GameManager.Instance.player.transform.position = startPoint.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.currentScene = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(nextScene);
        }
    }
}
