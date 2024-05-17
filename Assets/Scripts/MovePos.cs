using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePos : MonoBehaviour
{
    [SerializeField] private Vector2 m_Pos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(UIManager.Instance.screenFader.Fade(ScreenFader.FadeDirection.In, 0f));
        GameManager.Instance.CamOff();
        StartCoroutine(ChangePos());
    }

    private IEnumerator ChangePos()
    {
        yield return new WaitForSeconds(0f);

        GameManager.Instance.player.transform.position = new Vector2(GameManager.Instance.player.transform.position.x + m_Pos.x,
            GameManager.Instance.player.transform.position.y + m_Pos.y);
    }
}
