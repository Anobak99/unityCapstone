using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Coroutine shakeCoroutine;
    public float shakeTime;
    public float shakeIntensity;

    private void Awake()
    {
        Instance = this;
    }

    public void OnShakeCamera(float shakeTime = 0.5f, float shakeIntensity = 1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeByPosition());
    }

    private IEnumerator ShakeByPosition()
    {
        Vector3 startPosition = transform.position;
        float totalShakeTime = shakeTime;

        while (shakeTime > 0.0f)
        {
            float currentIntensity = Mathf.Lerp(shakeIntensity, 0, 1 - (shakeTime / totalShakeTime));
            transform.position = startPosition + Random.insideUnitSphere * currentIntensity;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;
        shakeCoroutine = null;
    }

}
