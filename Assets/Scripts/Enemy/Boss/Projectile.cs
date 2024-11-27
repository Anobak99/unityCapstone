using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType { Straight, Homing, QuadraticHoming, CubicHoming }

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private GameObject lerpPrefab;
    [SerializeField]
    private ProjectileType projectileType;
    [SerializeField]
    private Transform owner;
    [SerializeField]
    private Transform target;

    private Vector3 start, end;
    private float t;

    private void Update()
    {
        // 숫자 키 입력을 통해 발사체 타입 변경
        // 1. Straight, 2. Homing, 3. QuadraticHoming, 4. CubicHoming
        if (int.TryParse(Input.inputString, out int index) && (index > 0 && index < 5))
        {
            // 모든 자식 오브젝트 삭제
            for (int i = 0; i < transform.childCount; ++i)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            projectileType = (ProjectileType)index - 1;
            t = 0.0f;
            start = owner.position;
            end = target.position;
        }

        // 한 단계씩 실행
        if (Input.GetKeyDown(KeyCode.P))
        {
            Process();
            t += 0.1f;
        }
    }

    private void Process()
    {
        if (t > 1.1f) return;

        switch (projectileType)
        {
            case ProjectileType.Straight:
                OnStraight();
                break;
            case ProjectileType.Homing:
                OnHoming();
                break;
            case ProjectileType.QuadraticHoming:
                OnQuadraticHoming();
                break;
            case ProjectileType.CubicHoming:
                OnCubicHoming();
                break;
        }
    }

    private void OnStraight()
    {
        Vector3 position = Lerp(start, end, t);
        Instantiate(projectilePrefab, position, Quaternion.identity, transform);
    }

    private void OnHoming()
    {
        end = target.position;

        Vector3 position = Lerp(start, end, t);
        Instantiate(projectilePrefab, position, Quaternion.identity, transform);
    }

    private void OnQuadraticHoming()
    {
        end = target.position;

        Vector3 point = new Vector3(-4f, 5f, 0f);

        Vector3 p1 = Lerp(start, point, t);
        Instantiate(lerpPrefab, p1, Quaternion.identity, transform).GetComponent<SpriteRenderer>().color = Color.red;

        Vector3 p2 = Lerp(point, end, t);
        Instantiate(lerpPrefab, p2, Quaternion.identity, transform).GetComponent<SpriteRenderer>().color = Color.yellow;

        Vector3 position = Lerp(p1, p2, t);
        Instantiate(projectilePrefab, position, Quaternion.identity, transform);
    }

    private void OnCubicHoming()
    {
        end = target.position;

        Vector3 point1 = new Vector3(-4f, 5f, 0f);
        Vector3 point2 = new Vector3(4f, -5f, 0f);

        Vector3 p1 = Lerp(start, point1, t);
        Instantiate(lerpPrefab, p1, Quaternion.identity, transform).GetComponent<SpriteRenderer>().color = Color.red;

        Vector3 p2 = Lerp(point1, point2, t);
        Instantiate(lerpPrefab, p2, Quaternion.identity, transform).GetComponent<SpriteRenderer>().color = Color.yellow;

        Vector3 p3 = Lerp(point2, end, t);
        Instantiate(lerpPrefab, p3, Quaternion.identity, transform).GetComponent<SpriteRenderer>().color = Color.green;

        Vector3 p12 = Lerp(p1, p2, t);
        Instantiate(lerpPrefab, p12, Quaternion.identity, transform).GetComponent<SpriteRenderer>().color = Color.blue;

        Vector3 p23 = Lerp(p2, p3, t);
        Instantiate(lerpPrefab, p23, Quaternion.identity, transform).GetComponent<SpriteRenderer>().color = Color.magenta;

        Vector3 position = Lerp(p12, p23, t);
        Instantiate(projectilePrefab, position, Quaternion.identity, transform);
    }

    private Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return a + (b - a) * t;
    }
}

