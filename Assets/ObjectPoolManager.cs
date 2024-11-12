using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    public GameObject effectPrefab; // ����Ʈ ������
    public GameObject fireballPrefab; // �÷��̾� ���̾ ������
    public int initialPoolSize = 10; // �ʱ� Ǯ ������

    private Queue<GameObject> effectPool = new Queue<GameObject>();
    private Queue<GameObject> fireballPool = new Queue<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // �ʱ� Ǯ�� ����
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject effectObj = Instantiate(effectPrefab);
            GameObject fireballObj = Instantiate(fireballPrefab);

            effectObj.transform.SetParent(this.transform);
            fireballObj.transform.SetParent(this.transform);

            effectObj.SetActive(false);
            fireballObj.SetActive(false);

            effectPool.Enqueue(effectObj);
            fireballPool.Enqueue(fireballObj);
        }
    }

    // ������Ʈ�� Ǯ���� ��������
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
}
