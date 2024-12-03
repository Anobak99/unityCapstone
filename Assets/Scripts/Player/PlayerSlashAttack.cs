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
        // �θ� ������Ʈ�� ��ũ��Ʈ�� �����ͼ� �迭 ����
        if (player != null)
        {
            player.DetectEnemy(collisionList.ToArray()); // ����Ʈ�� �迭�� ��ȯ�Ͽ� ����
        }

        // ���� �� ����Ʈ �ʱ�ȭ
        collisionList.Clear();
    }
}
