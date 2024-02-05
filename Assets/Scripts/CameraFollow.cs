using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float followSpeed;
    public float xOffSet;
    public float yOffSet;

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
            Vector3 newPos = new Vector3(player.transform.position.x + xOffSet, player.transform.position.y + 1f + yOffSet, -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }   
}
