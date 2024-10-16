using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolcanoDoorSwitch : MonoBehaviour
{
    public int key_num;
    [HideInInspector] public Animator animator;
    private BoxCollider2D col;

    private bool isPlayer = false;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isPlayer && Input.GetKeyDown(KeyCode.G))
        {
            animator.SetBool("Lever", true);
            if (!SceneManager.GetSceneByName("Volcano_8").isLoaded) // 열려고하는  문 오브젝트의 씬이 활성화되지 않았을 때
            {
                SceneManager.LoadSceneAsync("Volcano_8", LoadSceneMode.Additive);
                GameObject door = GameObject.Find("Door");
                door.SetActive(false);
                col.enabled = false;
                DataManager.Instance.currentData.openedDoor[key_num] = true;
                //SceneManager.UnloadSceneAsync("Volcano_8");
            }
            else
            {
                GameObject door = GameObject.Find("Door");
                door.SetActive(false);
                col.enabled = false;
                DataManager.Instance.currentData.openedDoor[key_num] = true;
                //SceneManager.UnloadSceneAsync("Volcano_8");
            }
            CameraShake.Instance.OnShakeCamera(0.5f, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayer = false;
        }
    }
}
