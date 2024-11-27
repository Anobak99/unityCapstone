using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sage_move : Boss
{
    private bool facingRight = false;
    private float horizental;

    private int attackCount; //공격횟수
    private int patternNum = 0;
    private int patternNum2 = 1;
    [SerializeField] private bool phase1 = false;
    [SerializeField] private bool phase2 = false;
    [SerializeField] private float jumpHeight;
    [SerializeField] private BossBattle battle;

    public Vector2[] warpPos_list;
    [SerializeField] private GameObject[] magic_Pos;
    private int warpNum;
    private int preWarpNum;
    [SerializeField] private GameObject downDragon;
    [SerializeField] private Animator downAttack;
    [SerializeField] private Sage_summon_dragon upFire;
    [SerializeField] private Sage_summon_dragon downFire;
    [SerializeField] private Enemy_Pool object_Pool;
    [HideInInspector] public bool uppderDragon;
    [HideInInspector] public bool lowerDragon;
    public bool summonEnd = true;

    private readonly WaitForSeconds wait1 = new(1f);
    private readonly WaitForSeconds wait2 = new(2f);

    public override IEnumerator Think()
    {

        if (GameManager.Instance.gameState != GameManager.GameState.Boss || isDead || Time.timeScale == 0) yield return null;

        if (canAct && player != null && !GameManager.Instance.isDead)
        {
            horizental = player.position.x - transform.position.x;
            FlipToPlayer(horizental);

            switch (patternNum)
            {
                case 0:
                    act2 = StartCoroutine(Attack1());
                    yield break;
                case 1:
                    act2 = StartCoroutine(Attack2());
                    yield break;
                default:
                    break;
            }

            yield return new WaitForSeconds(0.1f);
            act1 = StartCoroutine(Think());
        }
    }


    private void FlipToPlayer(float playerPosition)
    {
        if (playerPosition < 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if (playerPosition > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    IEnumerator Attack1() //마법 공격
    {
        GameObject bullet;
        Mage_orb magic;

        canAct = false;
        for(int i = 0; i < 5; i++)
        {
            SoundManager.PlaySound(SoundType.CASTLETOP, 1f, 6);
            bullet = object_Pool.GetObject(magic_Pos[i%3].transform.position, "Attack1");
            magic = bullet.GetComponent<Mage_orb>();
            Vector2 dirVec = player.transform.position - transform.position;
            magic.rb.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.2f);
        }
        //object_Pool.GetObject(magic_Pos[1].transform.position, "Attack1");
        //object_Pool.GetObject(magic_Pos[2].transform.position, "Attack1");
        yield return wait2;
        attackCount++;
        act2 = StartCoroutine(Warp());
    }

    IEnumerator Attack2() //마법 공격2
    {
        GameObject bullet;
        Mage_orb magic;

        canAct = false;
        SoundManager.PlaySound(SoundType.CASTLETOP, 1f, 7);
        bullet = object_Pool.GetObject(transform.position, "Attack2");
        magic = bullet.GetComponent<Mage_orb>();
        Vector2 dirVec = player.transform.position - transform.position;
        magic.rb.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);
        yield return wait2;
        act2 = StartCoroutine(Warp());
    }

    IEnumerator Attack3() //상단 드래곤 소환
    {
        canAct = false;
        canDamage = false;
        summonEnd = false;
        animator.SetTrigger("Warp");
        yield return wait1;
        transform.position = warpPos_list[5];
        animator.SetTrigger("Appear");
        yield return wait1;
        canDamage = true;
        StartCoroutine(upFire.Summon());
        yield return wait2;
        patternNum = 0;
        patternNum2++;
        act2 = StartCoroutine(Warp());
    }

    IEnumerator Attack4() //하단 드래곤 소환
    {
        canAct = false;
        canDamage = false;
        summonEnd = false;
        animator.SetTrigger("Warp");
        yield return wait1;
        transform.position = warpPos_list[4];
        animator.SetTrigger("Appear");
        yield return wait1;
        canDamage = true;
        StartCoroutine(downFire.Summon());
        yield return wait2;
        patternNum = 0;
        patternNum2--;
        act2 = StartCoroutine(Warp());
    }

    IEnumerator Attack5() //드래곤 내려찍기
    {
        downDragon.transform.position = new Vector2(267f, 180f);
        downAttack.SetTrigger("appear");
        yield return wait2;
        downDragon.transform.position = new Vector2(player.position.x, downDragon.transform.position.y);
        yield return new WaitForSeconds(0.5f);
        downAttack.SetTrigger("attack");
    }

    IEnumerator Damaged() //다음 페이즈로
    {
        canAct = false;
        animator.SetBool("hit", true);
        if (facingRight)
            rb.velocity = Vector2.left * 1.5f;
        else
            rb.velocity = Vector2.right * 1.5f;

        yield return new WaitForSeconds(2f);
        if (!phase1)
            phase1 = true;
        else if (!phase2)
            phase2 = true;
        animator.SetBool("hit", false);

        act1 = StartCoroutine(Warp());
    }

    IEnumerator Warp()
    {
        canDamage = false;
        animator.SetTrigger("Warp");
        yield return wait1;
        if(uppderDragon)
        {
            warpNum = Random.Range(0, 2);
            while(warpNum == preWarpNum)
                warpNum = Random.Range(0, 2);
            transform.position = warpPos_list[warpNum];
            preWarpNum = warpNum;
        }
        else if(lowerDragon)
        {
            warpNum = Random.Range(2, 4);
            while (warpNum == preWarpNum)
                warpNum = Random.Range(2, 4);
            transform.position = warpPos_list[warpNum];
            preWarpNum = warpNum;
        }
        else
        {
            warpNum = Random.Range(0, 4);
            while (warpNum == preWarpNum)
                warpNum = Random.Range(0, 4);
            transform.position = warpPos_list[warpNum];
            preWarpNum = warpNum;
        }
        warpNum = 9;
        horizental = player.position.x - transform.position.x;
        FlipToPlayer(horizental);
        animator.SetTrigger("Appear");
        canDamage = true;
        yield return wait1;
        

        if (phase2)
        {
            if (summonEnd && patternNum2 == 0)
            {
                act2 = StartCoroutine(Attack3());
                yield break;
            }
            else if (summonEnd && patternNum2 == 1)
            {
                act2 = StartCoroutine(Attack4());
                yield break;
            }
        }

        if (patternNum == 0 && attackCount == 2)
        {
            if (phase1)
                StartCoroutine(Attack5());
            patternNum = 1;
            attackCount = 0;
        }
        else if (patternNum == 1)
            patternNum = 0;

        
        canAct = true;
        act1 = StartCoroutine(Think());
    }

    

    public override IEnumerator TakeDamage(int dmg, Vector2 attackPos)
    {
        canDamage = false;
        spriteRenderer.material = flashMaterial;
        hp -= dmg;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defalutMaterial;

        if(!phase1 && hp < 70)
        {
            if (act1 != null)
            {
                StopCoroutine(act1);
            }
            if (act2 != null)
            {
                StopCoroutine(act2);
            }
            StartCoroutine(Damaged());
            yield break;
        }
        if (!phase2 && hp < 40)
        {
            if (act1 != null)
            {
                StopCoroutine(act1);
            }
            if (act2 != null)
            {
                StopCoroutine(act2);
            }
            StartCoroutine(Damaged());
            yield break;
        }

        if (hp <= 0)
        {
            StartCoroutine(Death());
            yield break;
        }
        canDamage = true;
    }

    private IEnumerator Death()
    {
        if (act1 != null)
        {
            StopCoroutine(act1);
        }
        if (act2 != null)
        {
            StopCoroutine(act2);
        }
        upFire.StopAct();
        downFire.StopAct();
        rb.velocity = Vector2.zero;
        canDamage = false;
        isDead = true;
        animator.SetBool("hit", true);
        if (facingRight)
            rb.velocity = Vector2.left * 1.5f;
        else
            rb.velocity = Vector2.right * 1.5f;

        yield return new WaitForSeconds(2f);
        battle.BossDead();
    }
}
