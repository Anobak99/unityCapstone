using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityStatue : MonoBehaviour
{
    public Dialogue dialogueHasItem;
    public Dialogue dialogueDontHaveItem;
    [SerializeField] private Item item;
    private Button acceptButton;
    private Button declineButton; 

    public int anbility_num;

    bool closePlayer = false;

    private void Awake()
    {
        acceptButton = DialogueManager.Instance.acceptButton;
        declineButton = DialogueManager.Instance.declineButton;
    }

    private void Start()
    {
        acceptButton.onClick.AddListener(AbilityAccept);
        acceptButton.onClick.AddListener(AbilityDecline);
    }

    public void Update()
    {
        if (closePlayer && Input.GetKeyDown(KeyCode.G))
        {
            if (InventoryManager.instance.Items.Contains(item))
            {
                DialogueManager.Instance.StartDialogue(dialogueHasItem);
            }
            else
            {
                DialogueManager.Instance.StartDialogue(dialogueDontHaveItem);
            }
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

    public void AbilityAccept()
    {
        GetAbility();
    }

    public void AbilityDecline()
    {
        DialogueManager.Instance.DisplayNextSentence();
    }

    public void GetAbility()
    {
        Debug.Log("´É·Â È¹µæ");
        DialogueManager.Instance.sentences.Enqueue("È­¿°±¸ ±â¼úÀ» ¹è¿ü´Ù.");
        DialogueManager.Instance.DisplayNextSentence();
        InventoryManager.instance.RemoveItem(item);
        CameraShake.Instance.OnShakeCamera(0.5f, 2f);
        UIManager.Instance.systemScreen.SetActive(true);
        UIManager.Instance.AbilityImageOn(anbility_num);
        DataManager.instance.currentData.abilities[anbility_num] = true;
    }



}
