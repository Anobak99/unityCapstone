using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashTest : MonoBehaviour
{
    // 플래시 이펙트 테스트
    [SerializeField] private FlashEffect flashEffect;
    [SerializeField] public Color color = Color.red;
    //

    private void Start()
    {
        flashEffect = GetComponent<FlashEffect>();
    }

    public void Flash()
    {
        flashEffect.Flash(color);
    }
}
