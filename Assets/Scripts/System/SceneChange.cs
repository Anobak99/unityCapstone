using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    [SerializeField] private string nextScene;
    [SerializeField] private string curScene;
    [SerializeField] private MovePos exit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !SceneManager.GetSceneByName(nextScene).isLoaded)
        {
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.Instance.currentScene == curScene && !GameManager.Instance.nextScene)
        {
            SceneManager.UnloadSceneAsync(nextScene);
        }
    }

}
