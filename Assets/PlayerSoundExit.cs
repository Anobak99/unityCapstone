using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundExit : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField] private int soundNum = 0;
    [SerializeField, Range(0, 1)] private float volume = 1;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.PlaySound(sound, volume, soundNum);
    }
}
