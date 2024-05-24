using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public string nextScene;
    void Start()
    {
        nextScene = GameManager.Instance.respawnScene;
        SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
    }
}
