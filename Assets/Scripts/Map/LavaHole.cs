using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LavaHole : MonoBehaviour
{
    [SerializeField] private GameObject lavaHole;
    private BoxCollider2D col;
    public int key_num = 2;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            CameraShake.Instance.OnShakeCamera(0.5f, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pickupable")
        {
            col.enabled = false;
            //collision.GetComponent<ObjectPosSave>().SaveObjectPos();
            //CameraShake.Instance.OnShakeCamera(0.5f, 0.5f);
            SoundManager.PlaySound(SoundType.SFX, 1, 6);
            lavaHole.SetActive(false);
            DataManager.Instance.currentData.openedDoor[key_num] = true;
            GameObject lava = GameObject.Find("Lava_Door");
            lava.SetActive(false);
        }
    }
}
