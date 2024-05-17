using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    [SerializeField] private string nextScene;
    [SerializeField] private string curScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !SceneManager.GetSceneByName(nextScene).isLoaded)
        {
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
            //StartCoroutine(UIManager.Instance.screenFader.FadeAndLoadScene(ScreenFader.FadeDirection.In, nextScene));
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.Instance.currentScene == curScene)
        {
            SceneManager.UnloadSceneAsync(nextScene);
        }
    }

}
