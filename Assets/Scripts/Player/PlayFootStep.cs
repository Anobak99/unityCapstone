using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootStep : MonoBehaviour
{
    [SerializeField] private int soundNum = 0;
    [SerializeField, Range(0, 1)] private float volume = 1;

    public void PlaySound()
    {
        SoundManager.PlaySound(SoundType.FOOTSTEP, volume, soundNum);
    }
}
