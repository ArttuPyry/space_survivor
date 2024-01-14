using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler instance;

    private List<GameObject> pooledObject = new List<GameObject>();
    private int amountToBool = 20;

    [SerializeField] private GameObject prefab;

    public bool willExpand = true;
    public int maxAmount = 50;

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
            }
        }
        if (willExpand)
        {
            if (pooledObject.Count < maxAmount)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pooledObject.Add(obj);
            }
        }
        return null;
    }
}
