using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaySoundEnter : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField, Range(0, 1)] private float volume = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(sound);
        SoundManager.EnemyPlaySound(sound, volume, 0);
    }
}
