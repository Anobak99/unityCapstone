using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolcanoKey : MonoBehaviour
{
    [SerializeField] private Item item;
    public Dialogue dialogue;
    private Button acceptButton;

    private void Awake()
    {
        acceptButton = DialogueManager.Instance.acceptButton;
    }

    private void Start()
    {
        acceptButton.onClick.AddListener(AbilityAccept);
    }
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                DialogueManager.Instance.StartDialogue(dialogue);
            }
        }
    }

    public void AbilityAccept()
    {
        GetKey();
    }

    void GetKey()
    {
        DialogueManager.Instance.sentences.Enqueue(item.itemName +"¿ª(∏¶) »πµÊ«ﬂ¥Ÿ.");
        DialogueManager.Instance.HideChoices();
        DialogueManager.Instance.DisplayNextSentence();
        InventoryManager.instance.AddItem(item);
        gameObject.SetActive(false);
    }
}
