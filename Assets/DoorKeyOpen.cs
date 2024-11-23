using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorKeyOpen : MonoBehaviour
{
    public int num;

    public Dialogue dialogueHasKey;
    public Dialogue dialogueDontHaveKey;
    public Item item;

    private Button acceptButton;
    private Button declineButton;

    private void Awake()
    {
        acceptButton = DialogueManager.Instance.acceptButton;
        declineButton = DialogueManager.Instance.declineButton;

        acceptButton.onClick.AddListener(Accept);
        acceptButton.onClick.AddListener(Decline);
    }

    private void OnEnable()
    {
        if (DataManager.Instance.currentData.openedDoor[num])
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.G))
        {
            if(InventoryManager.instance.Items.Contains(item))
            {
                DialogueManager.Instance.StartDialogue(dialogueHasKey);
            }
            else
            {
                DialogueManager.Instance.StartDialogue(dialogueDontHaveKey);
            }
        }
    }

    private void Accept()
    {
        UseKey();
    }

    private void Decline()
    {
        DialogueManager.Instance.DisplayNextSentence();
    }

    private void UseKey()
    {
        DialogueManager.Instance.sentences.Enqueue("열쇠를 사용했다.");
        DialogueManager.Instance.DisplayNextSentence();
        InventoryManager.instance.RemoveItem(item);
        DataManager.instance.currentData.openedDoor[num] = true;
        gameObject.SetActive(false);
    }
}
