using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    private float length;
    float startPosition;
    public float parallaxFactor;

    void Start()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        startPosition = transform.position.x;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxFactor; //0 = move with cam | 1 = don't move | 0.5 = move half speed
        float movement = cam.transform.position.x * (1 - parallaxFactor);
        transform.position = new Vector3(distance + startPosition, cam.transform.position.y, transform.position.z);

        //if background reached the end of its length, move Position
        if(movement > startPosition + length) { startPosition += length; }
        else if(movement < startPosition - length) { startPosition -= length; }
    }
}
