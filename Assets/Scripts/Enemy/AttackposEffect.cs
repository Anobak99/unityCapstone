using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackposEffect : MonoBehaviour
{
    public GameObject[] Attack;
    public Transform player;


    private void Start()
    {
        StartCoroutine(AtkEffectOn());
    }

    private void Update()
    {
 
    }


    IEnumerator AtkEffectOn()
    {
        while (true)
        {
            float closestDistance = Mathf.Infinity;
            GameObject closestAttackPos = null;

            foreach (GameObject obj in Attack)
            {
                float distance = Vector2.Distance(player.position, obj.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestAttackPos = obj;
                }
            }

            SpriteRenderer AtkPosSprite = closestAttackPos.GetComponent<SpriteRenderer>();
            AtkPosSprite.enabled = true;

            yield return new WaitForSeconds(1f);

            AtkPosSprite.enabled = false;

            yield return new WaitForSeconds(2f);
        }
    }
}
