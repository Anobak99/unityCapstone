using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboAttack : StateMachineBehaviour
{

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Input.GetKey(KeyCode.A))
        {
            animator.SetTrigger("isCombo");
        }
    }
}
