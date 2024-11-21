using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pool : MonoBehaviour, IObserver
{
    [SerializeField] Subject playerSubject;

    public GameObject effectPrefab; // 이펙트 프리팹
    public GameObject fireballPrefab; // 플레이어 파이어볼 프리팹
    public int initialPoolSize = 10; // 초기 풀 사이즈

    private Queue<GameObject> effectPool = new Queue<GameObject>();
    private Queue<GameObject> fireballPool = new Queue<GameObject>();

    private void OnEnable()
    {
        playerSubject.AddObsrver(this);
    }

    public GameObject GetEffectObject(Vector2 position, Quaternion rotation)
    {
        GameObject obj;

        if (effectPool.Count > 0 && !effectPool.Peek().activeInHierarchy)
        {
            obj = effectPool.Dequeue();
        }
        else
        {
            obj = Instantiate(effectPrefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        StartCoroutine(DeactivatePrefab(obj, 3f));
        effectPool.Enqueue(obj);

        return obj;
    }

    public GameObject GetFireBallObject(Vector2 position, Quaternion rotation)
    {
        GameObject obj;

        if (fireballPool.Count > 0 && !fireballPool.Peek().activeInHierarchy)
        {
            obj = fireballPool.Dequeue();
        }
        else
        {
            obj = Instantiate(fireballPrefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        fireballPool.Enqueue(obj);

        return obj;
    }

    private IEnumerator DeactivatePrefab(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    public void OnNotify()
    {
        gameObject.SetActive(false);
    }
}
