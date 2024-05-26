using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUISensor : MonoBehaviour
{
    private SpriteRenderer InteractionImg;
    private Transform parentTrans;


    void Start()
    {
        parentTrans = transform.parent.gameObject.GetComponent<Transform>();
        InteractionImg = gameObject.GetComponent<SpriteRenderer>();
        InteractionImg.enabled = false;
    }

    private void Update()
    {
        
    }

    void Flip()
    {
        if (parentTrans.localScale.x < 0)
        {
            if (transform.localScale.x > 0)
            {
                gameObject.transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
            else if (transform.localScale.x < 0)
            {
                gameObject.transform.localScale = new Vector2(transform.localScale.x * 1, transform.localScale.y);
            }
        }
        else if (parentTrans.localScale.x > 0)
        {
            if (transform.localScale.x > 0)
            {
                gameObject.transform.localScale = new Vector2(transform.localScale.x * 1, transform.localScale.y);
            }
            else if (transform.localScale.x < 0)
            {
                gameObject.transform.localScale = new Vector2(transform.localScale.x * -11, transform.localScale.y);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<ObjectPickUp>().holding() == true)
        {
            InteractionImg.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InteractionImg.enabled = false;
            Flip();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InteractionImg.enabled = true;
        }
    }
  
}
