using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCrater : MonoBehaviour
{
    [SerializeField] private GameObject fire;
    public float waitTime;
    public float onTime;
    public float offTime;

    private void Awake()
    {
        StartCoroutine(Wait());
    }

    IEnumerator FireOnOff()
    {      
        fire.SetActive(true);

        yield return new WaitForSeconds(onTime);

        fire.SetActive(false);

        yield return new WaitForSeconds(offTime);

        StartCoroutine(FireOnOff());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(FireOnOff());
    }
}
