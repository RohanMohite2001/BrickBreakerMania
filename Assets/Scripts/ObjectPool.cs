using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public List<GameObject> pooledObjectList;
    public GameObject[] objectToPool;
    public int amountToPool;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pooledObjectList = new List<GameObject>();
        GameObject temp;

        for (int i = 0; i < amountToPool; i++)
        {
            for (int j = 0; j < objectToPool.Length; j++)
            {
                temp = Instantiate(objectToPool[j]);
                temp.SetActive(false);
                pooledObjectList.Add(temp);
            }
        }
    }

    public GameObject GetPooledObject(int index)
    {
        if (!pooledObjectList[index].activeInHierarchy)
        {
            return pooledObjectList[index];
        }
        return null;
    }
}
