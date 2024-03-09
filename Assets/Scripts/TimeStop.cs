using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    private float speed;
    private bool RestoreTime;

    private void Start()
    {
        RestoreTime = false;
    }

    private void Update()
    {      
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    StopTime(0.05f, 10, 0.1f);
        //}

        if (RestoreTime)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1f;
                RestoreTime = false;
            }
        }
    }

    public void StopTime(float ChangeTime, int RestoreSpeed, float Delay)
    {
        speed = RestoreSpeed;
        Debug.Log("Time Stop");
        if(Delay > 0)
        {
            StopCoroutine(StartTimeAgain(Delay));
            StartCoroutine(StartTimeAgain(Delay));
        }
        else
        {
            RestoreTime = true;
        }

        Time.timeScale = ChangeTime;
    }

    IEnumerator StartTimeAgain(float amt)
    {
        yield return new WaitForSecondsRealtime(amt);
        RestoreTime = true;        
    }
    
}
