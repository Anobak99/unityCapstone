using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sage_summon_dragon : MonoBehaviour
{
    public enum direction {left, right};
    [SerializeField] private direction facingDir;
    [SerializeField] private Animator dragon_anim;
    [SerializeField] private Animator head_anim;
    [SerializeField] private Transform firePos;

    [SerializeField] private Sage_move boss_move;
    [SerializeField] private Enemy_Pool objectPool;
    private bool Fire;

    public IEnumerator Summon()
    {
        
        dragon_anim.SetTrigger("appear");
        yield return new WaitForSeconds(2f);
        dragon_anim.SetTrigger("ready");
        head_anim.SetTrigger("ready");
        SoundManager.PlaySound(SoundType.CASTLETOP, 0.5f, 4);
        yield return new WaitForSeconds(3f);
        dragon_anim.SetTrigger("breath");
        head_anim.SetTrigger("breath");
        Fire = true;
        StartCoroutine(FireBreath());
        if (facingDir == direction.left)
        {
            boss_move.lowerDragon = true;
        }
        else
        {
            boss_move.uppderDragon = true;
        }
        yield return new WaitForSeconds(4f);
        Fire = false;
        yield return new WaitForSeconds(0.5f);
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

    public IEnumerator FireBreath()
    {
        if(Fire)
        {
            if (facingDir == direction.left)
            {
                objectPool.GetObject(firePos.position, "DownDragon");
            }
            else
            {
                objectPool.GetObject(firePos.position, "UpDragon");
            }
            yield return new WaitForSeconds(0.15f);
            // 브레스 사운드
            SoundManager.PlaySound(SoundType.CASTLETOP, 1f, 5);
            StartCoroutine(FireBreath());
        }
    }

    public void StopAct()
    {
        StopAllCoroutines();
    }
}
