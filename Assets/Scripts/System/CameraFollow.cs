using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public BoxCollider2D camBound;
    public float followSpeed;
    [SerializeField] private float xOffSet;
    [SerializeField] private float yOffSet;
    private float halfHeight, halfWidth;
    public bool cameraMove;
        
    private void Start()
    {        
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        if (player != null && cameraMove)
        {

            if(player.transform.localScale.x == 1)
            {
                xOffSet = 1.5f;
            }
            else
            {
                xOffSet = -1.5f;
            }
            //Vector3 newPos = new Vector3(
            //    Mathf.Clamp(player.transform.position.x + xOffSet, camBound.bounds.min.x, camBound.bounds.max.x),
            //    Mathf.Clamp(player.transform.position.y + yOffSet, camBound.bounds.min.y, camBound.bounds.max.y),
            //    -10f);
            Vector3 newPos = new Vector3(player.transform.position.x + xOffSet, player.transform.position.y, -10f);
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
        cameraMove = true;
    }

}
