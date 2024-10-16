using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStatue : MonoBehaviour
{
    public Dialogue dialogue;

    public int anbility_num;

    bool closePlayer = false;

    public void Update()
    {
        if (closePlayer && Input.GetKeyDown(KeyCode.G))
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            closePlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            closePlayer = false;
        }
    }

    public void GetAbility()
    {
        Debug.Log("´É·Â È¹µæ");
        CameraShake.Instance.OnShakeCamera(0.5f, 2f);
        DataManager.Instance.currentData.abilities[anbility_num] = true;
    }



}
