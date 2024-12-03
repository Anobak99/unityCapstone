using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashAttack : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private List<Collider2D> collisionList = new List<Collider2D>();

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            collisionList.Add(collision);
        }
        SendCollisionsToParent();
    }

    private void SendCollisionsToParent()
    {
        // 부모 오브젝트의 스크립트를 가져와서 배열 전달
        if (player != null)
        {
            player.DetectEnemy(collisionList.ToArray()); // 리스트를 배열로 변환하여 전달
        }

        // 전달 후 리스트 초기화
        collisionList.Clear();
    }
}
