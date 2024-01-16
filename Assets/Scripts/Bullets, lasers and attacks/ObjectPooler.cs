using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{

    [SerializeField] private static ObjectPooler instance;

    private List<GameObject> pooledObject = new List<GameObject>();
    [SerializeField] private int amountToBool = 20;

    [SerializeField] private GameObject prefab;

    [SerializeField] private bool willExpand = true;
    [SerializeField] private int maxAmount = 50;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < amountToBool; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pooledObject.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObject.Count; i++)
        {
            if (!pooledObject[i].activeInHierarchy)
            {
                return pooledObject[i];
            } else if (willExpand)
            {
                if (pooledObject.Count < maxAmount)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.SetActive(false);
                    pooledObject.Add(obj);
                }
            } else
            {
                return null;
            }
        }
        return null;
    }
}
