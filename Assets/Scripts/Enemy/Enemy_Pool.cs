using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pool : MonoBehaviour, IObserver
{
    [SerializeField] Subject playerSubject;
    [Serializable]
    public struct Pool
    {
        public int id;
        public string name;
        public GameObject targetPrefab;
    }
    public Pool[] object_Pool;
    public List<List<GameObject>> object_List = new();

    private void OnEnable()
    {
        playerSubject.AddObsrver(this);
        for(int i = 0; i < object_Pool.Length; i++)
        {
            object_List.Add(new List<GameObject>());
        }
    }

    public GameObject GetObject(Vector2 position, string request_name)
    {
        GameObject obj = null;
        int target_id = 0;

        foreach (var pool in object_Pool)
        {
            if (pool.name == request_name)
            {
                target_id = pool.id;
                break;
            }
        }

        foreach (GameObject item in object_List[target_id])
        {
            if (!item.activeSelf)
            {
                obj = item;
                obj.SetActive(true);
                break;
            }
        }

        if (!obj)
        {
            obj = Instantiate(object_Pool[target_id].targetPrefab, transform);
            object_List[target_id].Add(obj);
        }

        obj.transform.position = position;

        return obj;
    }

    private IEnumerator DeactivatePrefab(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    public void OnNotify()
    {
        foreach(var list in object_List)
        {
            foreach (var item in list)
            {
                if(item.activeSelf)
                    item.SetActive(false);
            }
        }
    }
}
