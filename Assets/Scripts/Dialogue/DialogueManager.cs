using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    #region Dialogue
    [Header("Dialogue")]
    public GameObject dialogueBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    [SerializeField] private float typeSpeed = 0.1f;
    #endregion

    #region Button
    [Header("Button")]
    public GameObject firstSelectButton;
    public GameObject leftSelectButton;
    public GameObject rightSelectButton;
    public Button continueButton;
    public Button acceptButton;
    public Button declineButton;
    #endregion

    public AudioClip clickSound;  // 선택 효과음
    private AudioSource audioSource;

    public bool isDialogue;
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

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        sentences = new Queue<string>();
        isDialogue = false;


        continueButton.onClick.AddListener(Continue);
        acceptButton.onClick.AddListener(Accept);
        declineButton.onClick.AddListener(Decline);

        acceptButton.gameObject.SetActive(false); 
        declineButton.gameObject.SetActive(false);
    }

    IEnumerator ButtonControl()
    {
        while(isDialogue)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (firstSelectButton.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(firstSelectButton);
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(leftSelectButton);
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (firstSelectButton.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(firstSelectButton);
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(rightSelectButton);
                }
            }
            yield return null;
        }
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.clip = clickSound;
            audioSource.Play();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogue = true;
        StartCoroutine(ButtonControl());
        
        if (!dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(true);
            dialogueBox.transform.SetAsLastSibling();
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
        if (sentence == "<<선택지>>")
        {
            // 수락/거절 버튼 표시
            ShowChoices();
            return; // 다음 문장은 출력하지 않음
        }

        StopCoroutine(TypeSentence(sentence));
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    void EndDialogue()
    {
        Debug.Log("대화문 끝");
        StopCoroutine(ButtonControl());
        dialogueBox.SetActive(false);
        isDialogue = false;
    }

    // 수락/거절 선택지를 표시하는 메서드
    void ShowChoices()
    {
        acceptButton.gameObject.SetActive(true);
        declineButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
    }

    public void Continue()
    {
        PlayClickSound();
        DisplayNextSentence();
    }

    public void Accept()
    {
        PlayClickSound();
        HideChoices();
        continueButton.gameObject.SetActive(true);
    }

    public void Decline()
    {
        PlayClickSound();
        HideChoices();
        continueButton.gameObject.SetActive(true);
    }

    // 선택지를 숨기는 메서드
    public void HideChoices()
    {
        acceptButton.gameObject.SetActive(false);
        declineButton.gameObject.SetActive(false);
    }
}
