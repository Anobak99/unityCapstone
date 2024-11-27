using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageUp : MonoBehaviour
{
    [SerializeField] private int statusId;
    [SerializeField] private int playerDamageUp;
    [SerializeField] private GameObject AttackUPtext;
    [SerializeField] private PlayerController player;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        if (DataManager.instance.currentData.attackUpItem[statusId])
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.PlaySound(SoundType.SFX, 1f, 9);
            DataManager.instance.currentData.attackUpItem[statusId] = true;

            spriteRenderer.enabled = false;
            StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        AttackUPtext.SetActive(true);

        yield return new WaitForSeconds(2f);

        AttackUPtext.SetActive(false);
        gameObject.SetActive(false);
    }
}
