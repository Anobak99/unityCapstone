using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTrap : MonoBehaviour
{
    public GameObject objectPrefab;
    private List<GameObject> pool;
    private RaycastHit2D hit;
    public LayerMask hitLayor;
    private bool canShot;
    public float shotCycle; //발사 주기
    public float scanRange; //인식 범위

    public enum Direction //바라보는 방향
    {
        left, right, up, down
    }
    public Direction direction;

    void Start()
    {
        pool = new List<GameObject>();
        canShot = true;
    }

    void Update()
    {
        switch (direction)
        {
            case Direction.left:
                hit = Physics2D.Raycast(transform.position, Vector2.left, scanRange, hitLayor);
                break;
            case Direction.right:
                hit = Physics2D.Raycast(transform.position, Vector2.right, scanRange, hitLayor);
                break;
            case Direction.up:
                hit = Physics2D.Raycast(transform.position, Vector2.up, scanRange, hitLayor);
                break;
            case Direction.down:
                hit = Physics2D.Raycast(transform.position, Vector2.down, scanRange, hitLayor);
                break;
            default:
                break;
        }

        if (hit)
        {
            if (canShot) StartCoroutine(Shot(shotCycle));
        }
    }

    private IEnumerator Shot(float t)
    {
        canShot = false;

        TrapBullet bullet;
        GameObject select = null;

        foreach (GameObject item in pool)
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if(!select)
        {
            select = Instantiate(objectPrefab, transform);
            pool.Add(select);
        }

        bullet = select.GetComponent<TrapBullet>();

        switch(direction)
        {
            case Direction.left:
                bullet.direction = Vector2.left;
                break;
            case Direction.right:
                bullet.direction = Vector2.right;
                break;
            case Direction.up:
                bullet.direction = Vector2.up;
                break;
            case Direction.down:
                bullet.direction = Vector2.down;
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(t);

        canShot = true;
    }
}
