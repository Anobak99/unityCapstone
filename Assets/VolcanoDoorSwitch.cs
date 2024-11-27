using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolcanoDoorSwitch : MonoBehaviour
{
    [SerializeField] private int switchNum;
    [SerializeField] private int doorNum;

    [HideInInspector] public Animator animator;
    private BoxCollider2D col;

    private bool isPlayer = false;
    private bool isUse = false;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        if(DataManager.instance.currentData.volcano_Switch[switchNum])
        {
            col.enabled = false;
        }
    }

    private void Update()
    {
        if (!isUse && isPlayer && Input.GetKeyDown(KeyCode.G))
        {
            animator.SetBool("Lever", true);
            if (!SceneManager.GetSceneByName("Volcano_8").isLoaded) // 열려고하는  문 오브젝트의 씬이 활성화되지 않았을 때
            {
                SceneManager.LoadSceneAsync("Volcano_8", LoadSceneMode.Additive);
                SoundManager.PlaySound(SoundType.SFX, 1, 7);

                GameObject door = GameObject.Find("Door");
                door.SetActive(false);
                col.enabled = false;

                DataManager.instance.currentData.volcano_Switch[switchNum] = true;
                DataManager.instance.currentData.volcano_SwitchDoor[doorNum] = true;
                //SceneManager.UnloadSceneAsync("Volcano_8");
                isUse = true;
            }
            else
            {
                SoundManager.PlaySound(SoundType.SFX, 1, 7);
                GameObject door = GameObject.Find("Door");

                door.SetActive(false);
                col.enabled = false;

                DataManager.instance.currentData.volcano_Switch[switchNum] = true;
                DataManager.instance.currentData.volcano_SwitchDoor[doorNum] = true;
                //SceneManager.UnloadSceneAsync("Volcano_8");
                isUse = true;
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
