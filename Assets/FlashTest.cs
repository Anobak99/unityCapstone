using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashTest : MonoBehaviour
{
    // �÷��� ����Ʈ �׽�Ʈ
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
