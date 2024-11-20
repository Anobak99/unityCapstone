using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialPlatform : MonoBehaviour
{
    //Ư�� ����� �ִ� �÷������� ����� ����ϴ� ��ũ��Ʈ

    private BoxCollider2D col;
    SpriteRenderer spRend;
    private bool stand; //�÷��̾ �� �ִ��� ����
    public float speed; //�÷��� �̵��ӵ�
    public Vector2[] locations; //�÷����� �̵��� ��ǥ
    int i = 0;
    public enum Type { move, disable };
    public Type type;
    public float time; //�÷����� ������������ �ð�

    
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        spRend = GetComponent<SpriteRenderer>();
        if (type == Type.move) transform.position = locations[0]; //0�� ��Ҹ� ���� ��ġ�� ����
    }

    private void FixedUpdate()
    {
        if (type == Type.move)
        {
            if (Vector2.Distance(transform.position, locations[i]) < 0.02f) //���� ��ġ�� ���� ��ġ������ �Ÿ��� ����Ͽ� ������ ����ó��
            {
                i++;
                if (i == locations.Length) i = 0; //��� ��Ҹ� ������ ��� ó�� ��ҷ� �̵�
            }

            transform.position = Vector2.MoveTowards(transform.position, locations[i], speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!stand && type == Type.move)
        {
            if (collision.collider.CompareTag("Player"))
            {
                //�÷��̾ �÷��� ���� �� ���� ��
                if (col.bounds.min.x < collision.collider.bounds.max.x && col.bounds.max.x > collision.collider.bounds.min.x)
                {
                    if (collision.collider.transform.position.y > transform.position.y + transform.localScale.y / 2)
                    {
                        collision.transform.parent = gameObject.transform;
                        stand = true;
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(stand && type == Type.move)
        {
            if (collision.collider.CompareTag("Player"))
            {
                collision.transform.parent = null;
                stand = false;
            }
        }
    }

    public void OnStand()
    {
        if (!stand && type == Type.disable)
        {
            stand = true;
            StartCoroutine(Disable(time));
        }
    }

    private IEnumerator Disable(float time)
    {
        //������ �ð� �� �÷����� ������ 0, �浹������ trigger�� �Ѵ�
        yield return new WaitForSeconds(time);
        col.isTrigger = true;
        spRend.color = new Color(255, 255, 255, 0);
        gameObject.layer = 0;

        yield return new WaitForSeconds(1f);
        col.isTrigger = false;
        spRend.color = new Color(255, 255, 255, 255);
        gameObject.layer = 7;

        stand = false;
    }
}
