using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LavaHole : MonoBehaviour
{
    [SerializeField] private int switchNum;
    [SerializeField] private int doorNum;
    [SerializeField] private GameObject pickupableObj;
    [SerializeField] private GameObject lava;
    private BoxCollider2D col;
    

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        if (SwitchManager.Instance.volcano_Switch[switchNum])
            pickupableObj.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SwitchManager.Instance.volcano_Switch[switchNum]) return;

        if (collision.gameObject.tag == "Pickupable")
        {
            UseSwitch();       
        }
    }

    private void UseSwitch()
    {
        col.enabled = false;
        SoundManager.PlaySound(SoundType.SFX, 1, 6);        

        SwitchManager.Instance.volcano_Switch[switchNum] = true;
        SwitchManager.Instance.volcano_SwitchDoor[doorNum] = true;

        if (lava != null) lava.SetActive(false);
        GameObject lavaDoor = GameObject.Find("Lava Door");
        lavaDoor.SetActive(false);
    }
}