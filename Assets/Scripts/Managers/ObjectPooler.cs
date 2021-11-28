using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(500)]
public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pooler
    {
        public string Key;
        public GameObject poolObject;
        public int amount = 1;
        public int amountToExpandIfOutOfPool = 1;
        public List<GameObject> pooledObjects;
    }
    public Transform PoolParent;
    public static ObjectPooler instance;
    public List<Pooler> PoolObjects;

    
    void Awake()
    {
        if (!instance)
            instance = this;
    }

    private void Start()
    {

        foreach (Pooler pooler in PoolObjects)
        {
            for (int i=0;i<pooler.amount;i++)
            {
                GameObject obj = (GameObject)Instantiate(pooler.poolObject, PoolParent);
                obj.SetActive(false);
                pooler.pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string Key,Vector3 pos,Quaternion rot)
    {
        Pooler pooler = PoolObjects.Find(x => x.Key == Key);
        GameObject returnObject = null;
        if (pooler != null)
        {
            returnObject = pooler.pooledObjects.Find(x => !x.activeInHierarchy);
            if (!returnObject)
            {
                expandPool(pooler);
                returnObject = pooler.pooledObjects.Find(x => !x.activeInHierarchy);
            }            
        }
        else
        {
            Debug.Log("There is no pool with Key : " + Key);
            return null;
        }

        if (returnObject)
        {
            returnObject.transform.position = pos;
            returnObject.transform.rotation = rot;
            returnObject.SetActive(true);
        }
        return returnObject;
    }


    void expandPool(Pooler pooler)
    {
        for (int i = 0; i < pooler.amountToExpandIfOutOfPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooler.poolObject, PoolParent);
            obj.SetActive(false);
            pooler.pooledObjects.Add(obj);
        }
    }
}
