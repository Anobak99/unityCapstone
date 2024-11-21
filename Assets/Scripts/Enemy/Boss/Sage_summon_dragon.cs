using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sage_summon_dragon : MonoBehaviour
{
    public enum direction {left, right};
    public direction facingDir;
    public Animator dragon_anim;
    public Animator head_anim;

    public Sage_move boss_move;

    public IEnumerator Summon()
    {
        
        dragon_anim.SetTrigger("appear");
        yield return new WaitForSeconds(2f);
        dragon_anim.SetTrigger("ready");
        head_anim.SetTrigger("ready");
        yield return new WaitForSeconds(3f);
        dragon_anim.SetTrigger("breath");
        head_anim.SetTrigger("breath");
        Debug.Log("Fire!");
        if (facingDir == direction.left)
        {
            boss_move.lowerDragon = true;
        }
        else
        {
            boss_move.uppderDragon = true;
        }
        yield return new WaitForSeconds(5f);
        if (facingDir == direction.left)
        {
            boss_move.lowerDragon = false;
        }
        else
        {
            boss_move.uppderDragon = false;
        }
        dragon_anim.SetTrigger("disappear");
        head_anim.SetTrigger("idle");
        yield return new WaitForSeconds(3f);
        boss_move.summonEnd = true;
    }
}
