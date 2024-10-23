using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasZone : MonoBehaviour
{
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    public Vector2 forceDirection = new Vector2(-1, 0); // 가스를 통해 밀릴 방향
    public float pushForce = 5f; // 밀리는 힘의 크기

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
        // 플레이어인지 확인 (Tag로 구분 가능)
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            // 플레이어가 존재하면 forceDirection 방향으로 힘을 가함
            playerRb.AddForce(forceDirection.normalized * pushForce);
        }
    }

    // 게임 오브젝트가 비활성화되면 코루틴 중지
    void OnDisable()
    {
        StopCoroutine(ToggleColliderCoroutine());
    }
}
