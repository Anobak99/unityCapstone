using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public Vector2[] locations;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = locations[0]; //0�� ��Ҹ� ���� ��ġ�� ����
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, locations[i]) < 0.02f) //���� ��ġ�� ���� ��ġ������ �Ÿ��� ����Ͽ� ������ ����ó��
        {
            i++;
            if (i == locations.Length) i = 0; //��� ��Ҹ� ������ ��� ó�� ��ҷ� �̵�
        }

        transform.position = Vector2.MoveTowards(transform.position, locations[i], speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //�÷��̾ �÷��� ���� �� ���� ��
            if (collision.transform.position.y > gameObject.transform.position.y + 0.5f)
            {
                collision.transform.parent = gameObject.transform;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
