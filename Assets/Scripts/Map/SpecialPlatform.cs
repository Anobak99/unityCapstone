using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialPlatform : MonoBehaviour
{
    //특수 기능이 있는 플랫폼들의 기능을 담당하는 스크립트

    private BoxCollider2D col;
    SpriteRenderer spRend;
    private bool stand; //플레이어가 서 있는지 여부
    public float speed; //플랫폼 이동속도
    public Vector2[] locations; //플랫폼이 이동할 좌표
    int i = 0;
    public enum Type { move, disable };
    public Type type;
    public float time; //플랫폼이 사라지기까지의 시간

    
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        spRend = GetComponent<SpriteRenderer>();
        if (type == Type.move) transform.position = locations[0]; //0번 장소를 시작 위치로 설정
    }

    private void FixedUpdate()
    {
        if (type == Type.move)
        {
            if (Vector2.Distance(transform.position, locations[i]) < 0.02f) //현재 위치와 다음 위치까지의 거리를 계산하여 가까우면 도착처리
            {
                i++;
                if (i == locations.Length) i = 0; //모든 장소를 돌았을 경우 처음 장소로 이동
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
                //플레이어가 플랫폼 위에 서 있을 시
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
        //지정된 시간 후 플랫폼의 투명도를 0, 충돌판정을 trigger로 한다
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
