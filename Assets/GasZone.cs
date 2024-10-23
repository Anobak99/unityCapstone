using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasZone : MonoBehaviour
{
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    public Vector2 forceDirection = new Vector2(-1, 0); // ������ ���� �и� ����
    public float pushForce = 5f; // �и��� ���� ũ��

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
        StartCoroutine(ToggleColliderCoroutine());
    }

    IEnumerator ToggleColliderCoroutine()
    {
        while (true)
        {
            col.enabled = !col.enabled;
            animator.enabled = !animator.enabled;
            audioSource.enabled = !audioSource.enabled;

            yield return new WaitForSeconds(0.6f);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // �÷��̾����� Ȯ�� (Tag�� ���� ����)
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            // �÷��̾ �����ϸ� forceDirection �������� ���� ����
            playerRb.AddForce(forceDirection.normalized * pushForce);
        }
    }

    // ���� ������Ʈ�� ��Ȱ��ȭ�Ǹ� �ڷ�ƾ ����
    void OnDisable()
    {
        StopCoroutine(ToggleColliderCoroutine());
    }
}
