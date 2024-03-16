using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aiPath;

    private void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f) // 속도가 양수 = 플레이어가 오른쪽에 있는 경우
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f) // 속도가 음수 = 플레이어가 왼쪽애 있는 경우
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
