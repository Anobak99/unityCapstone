using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public BoxCollider2D camBound;
    public float followSpeed;
    private float xOffSet;
    private float yOffSet;
    private float halfHeight, halfWidth;


    private void Start()
    {        
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        if (player != null)
        {

            if(player.transform.localScale.x == 1)
            {
                xOffSet = 1.5f;
            }
            else
            {
                xOffSet = -1.5f;
            }
            Vector3 newPos = new Vector3(
                Mathf.Clamp(player.transform.position.x + xOffSet, camBound.bounds.min.x + halfWidth, camBound.bounds.max.x - halfWidth),
                Mathf.Clamp(player.transform.position.y + yOffSet, camBound.bounds.min.y + halfHeight, camBound.bounds.max.y - halfHeight),
                -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
        else
        {
            player = GameManager.Instance.player;
        }
    }

    public void ChangeCameraPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
