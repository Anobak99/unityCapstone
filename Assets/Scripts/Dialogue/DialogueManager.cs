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

        // 선택지 트리거 확인
        if (sentence == "<<능력선택>>")
        {
            // 수락/거절 버튼 표시
            ShowChoices();
            return; // 다음 문장은 출력하지 않음
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
        Debug.Log("대화문 끝");
        dialogueBox.SetActive(false);
    }

    // 수락/거절 선택지를 표시하는 메서드
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
        sentences.Enqueue("수락하셨습니다."); 
        HideChoices();
        DisplayNextSentence(); 
    }

    public void AbilityDecline()
    {
        sentences.Enqueue("거절하셨습니다.");
        HideChoices();
        DisplayNextSentence(); 
    }

    // 선택지를 숨기는 메서드
    void HideChoices()
    {
        acceptButton.gameObject.SetActive(false);
        declineButton.gameObject.SetActive(false);
    }
}
