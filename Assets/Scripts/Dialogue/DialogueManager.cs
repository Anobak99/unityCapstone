using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    #region Dialogue
    [Header("Dialogue")]
    public GameObject dialogueBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    #endregion

    #region Button
    [Header("Button")]
    public Button continueButton;
    public Button acceptButton;
    public Button declineButton;
    #endregion

    public Queue<string> sentences;

    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<DialogueManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<DialogueManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        var objs = FindObjectsOfType<DialogueManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        sentences = new Queue<string>();


        continueButton.onClick.AddListener(Continue);
        acceptButton.onClick.AddListener(AbilityAccept);
        declineButton.onClick.AddListener(AbilityDecline);

        acceptButton.gameObject.SetActive(false); 
        declineButton.gameObject.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (!dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(true);
        }

        nameText.text = dialogue.name;

        if (sentences != null)
        {
            sentences.Clear();
        }

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        // ������ Ʈ���� Ȯ��
        if (sentence == "<<�ɷ¼���>>")
        {
            // ����/���� ��ư ǥ��
            ShowChoices();
            return; // ���� ������ ������� ����
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        Debug.Log("��ȭ�� ��");
        dialogueBox.SetActive(false);
    }

    // ����/���� �������� ǥ���ϴ� �޼���
    void ShowChoices()
    {
        acceptButton.gameObject.SetActive(true);
        declineButton.gameObject.SetActive(true);
    }

    public void Continue()
    {
        DisplayNextSentence();
    }

    public void AbilityAccept()
    {
        sentences.Enqueue("�����ϼ̽��ϴ�."); 
        HideChoices();
        DisplayNextSentence(); 
    }

    public void AbilityDecline()
    {
        sentences.Enqueue("�����ϼ̽��ϴ�.");
        HideChoices();
        DisplayNextSentence(); 
    }

    // �������� ����� �޼���
    void HideChoices()
    {
        acceptButton.gameObject.SetActive(false);
        declineButton.gameObject.SetActive(false);
    }
}
