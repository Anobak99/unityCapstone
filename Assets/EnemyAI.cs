using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 1.5f; // 다음 웨이 포인트까지의 거리

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    Seeker seeker;
    Rigidbody2D rigid;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayor;

    private bool isGround;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigid = GetComponent<Rigidbody2D>();

        float attackDistance = Vector2.Distance(rigid.position, target.position);


        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        float attackDistance = Vector2.Distance(rigid.position, target.position);

        if (seeker.IsDone() && attackDistance < 20)
        {
            seeker.StartPath(rigid.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
            return;
        }
        else
        {
            reachedEndofPath = false;
        }

        float attackDistance = Vector2.Distance(rigid.position, target.position);

        Check();
        if (attackDistance > 5 && attackDistance < 20 )
        {
            EnemyPatrol();
        }
        else if(attackDistance < 5 && isGround)
        {
            EnemyPatrol();
        }
    }

    public void EnemyPatrol()
    {
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigid.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rigid.AddForce(force);

        float distance = Vector2.Distance(rigid.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }


        if (force.x >= 0.01f) // 속도가 양수 = 플레이어가 오른쪽에 있는 경우
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (force.x <= -0.01f) // 속도가 음수 = 플레이어가 왼쪽애 있는 경우
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void Check()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayor);
    }


}
