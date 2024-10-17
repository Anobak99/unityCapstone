using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootStep : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float volume = 1;

    public void PlaySound()
    {
        SoundManager.PlaySound(SoundType.FOOTSTEP, volume);
    }
}
