using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    public static CameraShake Instance => instance;
    

    public float shakeTime;
    public float shakeIntensity;

    public CameraShake()
    {
        instance = this;
    }


    public void OnShakeCamera(float shakeTime =0.5f, float shakeIntensity =1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");
    }

    private IEnumerator ShakeByPosition()
    {
        Vector3 startPosition = transform.position;

        while ( shakeTime > 0.0f)
        {

            // 초기 위치로부터 구 범위 * shakeIntensity의 범위 안에서 카메라 위치 변화
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;
    }

    private IEnumerator ShakeByRotation()
    {
        Vector3 startRotation = transform.eulerAngles;

        float power = 10f;

        while ( shakeTime > 0.0f)
        {
            float x = 0; //Random.Range(-1f, 1f);
            float y = 0; //Random.Range(-1f, 1f);
            float z = Random.Range(-1f, 1f);
            transform.rotation = Quaternion.Euler(startRotation + new Vector3(x, y, z) * shakeIntensity * power);

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.rotation = Quaternion.Euler(startRotation);
    }
    
}
