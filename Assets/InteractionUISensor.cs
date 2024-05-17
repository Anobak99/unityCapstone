using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUISensor : MonoBehaviour
{
    private SpriteRenderer InteractionImg;


    void Start()
    {
        InteractionImg = gameObject.GetComponent<SpriteRenderer>();
        InteractionImg.enabled = false;
    }  

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InteractionImg.enabled = false;
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
