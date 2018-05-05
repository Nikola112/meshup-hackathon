using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private int _initialSize = 10;

    private Queue<PoolObject> pooledObjects;
    private object _padLock = new object();

    private void Awake()
    {
        pooledObjects = new Queue<PoolObject>(_initialSize);

        if (_parent == null) _parent = transform;

        PopulatePool();
    }

    private void PopulatePool()
    {
        for(int i = 0; i < _initialSize; i++)
        {
            GameObject obj = Instantiate(_prefab);
            obj.SetActive(false);
            obj.transform.parent = _parent;

            var pooledObj = obj.AddComponent<PooledObject>();
            pooledObj.parent = this;

            var pool = new PoolObject { gameObject = obj, pooledObject = pooledObj };

            pooledObj.pool = pool;

            pooledObjects.Enqueue(pool);
        }
    }

    public GameObject Get(bool active = true)
    {
        if(pooledObjects.Count > 0)
        {
            PoolObject poolObj;

            lock(_padLock)
            {
                poolObj = pooledObjects.Dequeue();
            }

            poolObj.gameObject.SetActive(active);

            return poolObj.gameObject;
        }

        GameObject obj = Instantiate(_prefab);
        obj.transform.parent = _parent;

        var pooledObj = obj.AddComponent<PooledObject>();
        pooledObj.parent = this;

        var pool = new PoolObject { gameObject = obj, pooledObject = pooledObj };

        pooledObj.pool = pool;

        obj.SetActive(active);

        return obj;
    }

    public GameObject Get(Vector3 position, Quaternion rotation, bool active = true)
    {
        if (pooledObjects.Count > 0)
        {
            PoolObject poolObj;

            lock (_padLock)
            {
                poolObj = pooledObjects.Dequeue();
            }

            poolObj.gameObject.transform.position = position;
            poolObj.gameObject.transform.rotation = rotation;

            poolObj.gameObject.SetActive(active);

            return poolObj.gameObject;
        }

        GameObject obj = Instantiate(_prefab, position, rotation);
        obj.transform.parent = _parent;

        var pooledObj = obj.AddComponent<PooledObject>();
        pooledObj.parent = this;

        var pool = new PoolObject { gameObject = obj, pooledObject = pooledObj };

        pooledObj.pool = pool;

        obj.SetActive(active);

        return obj;
    }

    public void Return(PoolObject toReturn)
    {
        toReturn.gameObject.SetActive(false);
        toReturn.gameObject.transform.position = Vector3.zero;
        toReturn.gameObject.transform.rotation = Quaternion.identity;

        lock(_padLock)
        {
            pooledObjects.Enqueue(toReturn);
        }
    }

    public void Return(PooledObject toReturn)
    {
        toReturn.gameObject.SetActive(false);
        toReturn.gameObject.transform.position = Vector3.zero;
        toReturn.gameObject.transform.rotation = Quaternion.identity;

        lock (_padLock)
        {
            pooledObjects.Enqueue(toReturn.pool);
        }
    }

    public void Return(GameObject toReturn)
    {
        toReturn.SetActive(false);
        toReturn.transform.position = Vector3.zero;
        toReturn.transform.rotation = Quaternion.identity;

        lock (_padLock)
        {
            pooledObjects.Enqueue(toReturn.GetComponent<PooledObject>().pool);
        }
    }
}
