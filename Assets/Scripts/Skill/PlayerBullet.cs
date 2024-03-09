using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public GameObject player;
    public Transform bullet;
    public float speed = 10f;
    public int damage = 10;
    public Rigidbody2D rb;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<Transform>();
    }

    void Start()
    {
        if (player.transform.localScale.x == 1)
        {
            rb.velocity = transform.right * speed;
        }
        else if (player.transform.localScale.x == -1)
        {
            rb.velocity = transform.right * speed *-1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().Attacked(damage, bullet.position);
        }

        if (collision.gameObject.tag == "Switch")
        {
            Debug.Log("Switch Active");
            SwitchManager.Instance.openSwitchDoor[collision.GetComponent<Switch>().num] = true;
        }
        //Destroy(gameObject);
    }
}
