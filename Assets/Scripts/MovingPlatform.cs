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
        transform.position = locations[0]; //0번 장소를 시작 위치로 설정
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, locations[i]) < 0.02f) //현재 위치와 다음 위치까지의 거리를 계산하여 가까우면 도착처리
        {
            i++;
            if (i == locations.Length) i = 0; //모든 장소를 돌았을 경우 처음 장소로 이동
        }

        transform.position = Vector2.MoveTowards(transform.position, locations[i], speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //플레이어가 플랫폼 위에 서 있을 시
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
