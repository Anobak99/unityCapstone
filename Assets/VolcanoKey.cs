using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolcanoKey : MonoBehaviour
{
    [SerializeField] private int key_num;
    [SerializeField] private Item item;
    public Dialogue dialogue;
    private Button acceptButton;
    Animator animator;

    private void Awake()
    {
        acceptButton = DialogueManager.Instance.acceptButton;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        acceptButton.onClick.AddListener(AbilityAccept);
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("SignOn");
        }
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("SignOff");
        }
    }

    public void AbilityAccept()
    {
        GetKey();
    }

    void GetKey()
    {
        Debug.Log("≈∞ »πµÊ");
        DialogueManager.Instance.sentences.Enqueue("≈∞∏¶ »πµÊ«ﬂ¥Ÿ.");
        DialogueManager.Instance.HideChoices();
        DialogueManager.Instance.DisplayNextSentence();
        InventoryManager.instance.AddItem(item);
        DataManager.Instance.currentData.doorSwitch[key_num] = true;
        gameObject.SetActive(false);
    }
}
