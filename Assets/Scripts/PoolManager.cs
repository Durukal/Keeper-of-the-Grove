using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<Transform> activeList;
    [System.Serializable]
    public class ObjectPool
    {
        // Gameobject to pool
        public GameObject prefab;

        // Maximum instances of the gameobject
        public int maximumInstances;

        // Name of the pool
        public string poolName;

        // List to hold all instances of the object
        private List<GameObject> objectList;


        /// <summary>
        /// Initialize the pool with creating instances of the gameobject and a container
        /// for the hieararchy
        /// </summary>
        public void InitializePool()
        {
            // Create the list and container for the objects
            objectList = new List<GameObject>();
            GameObject pool = new GameObject("[" + poolName + "]");

            // Reference to the created instance
            GameObject clone;

            for (int i = 0; i < maximumInstances; i++)
            {
                // Create the gameobject
                clone = Instantiate(prefab);

                // Deactivate and add to the container and list
                clone.SetActive(false);
                clone.transform.parent = pool.transform;

                objectList.Add(clone);
            }
        }
        /// <summary>
        /// Get the next gameobject that can be spawned from the pool
        /// </summary>
        /// <returns>Next gameobject to spawn</returns>
        public GameObject GetNextObject()
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                if (!objectList[i].activeInHierarchy)
                {
                    return objectList[i];
                }
            }

            return null;
        }

        public void removeFromPool(GameObject poolObject)
        {
            if (objectList.Contains(poolObject))
                objectList.Remove(poolObject);
        }

        public void returnToPool(GameObject poolObject)
        {
            if (!objectList.Contains(poolObject))
                objectList.Add(poolObject);
            poolObject.transform.parent = objectList[0].transform.parent;
        }
        public int MaximumInstances { get { return maximumInstances; } }
        public string PoolName { get { return poolName; } set { poolName = value; } }
    }

    // List to hold all the pools for the game
    public List<ObjectPool> pools;

    public static PoolManager Instance;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize all the pools
        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].InitializePool();
        }
    }

    /// <summary>
    /// Spawn the next gameobject at its current place 
    /// </summary>
    /// <param name="poolName">Name of the pool to get the gameobject</param>
    /// <returns>Spawned gameobject</returns>
    public GameObject Spawn(string poolName)
    {
        // Get the pool with the given pool name
        ObjectPool pool = GetObjectPool(poolName);

        if (pool == null)
        {
            //Debug.LogErrorFormat("Cannot find the object pool with name %s", poolName);

            return null;
        }

        // Get the next object from the pool
        GameObject clone = pool.GetNextObject();

        if (clone == null)
        {
            //Debug.LogError("Scene contains maximum number of instances.");

            return null;
        }

        // Spawn the gameobject
        clone.SetActive(true);
        if (poolName == "Pig")
        {
            activeList.Add(clone.transform);
            clone.transform.position = new Vector3(Random.Range(GameManager.Instance.min_x_position + 7f, GameManager.Instance.max_x_position - 7f),
                Random.Range(GameManager.Instance.min_y_position + 7f, GameManager.Instance.max_y_position - 7f), 0);
                
        }
        if (poolName == "Chopper")
        {
            activeList.Add(clone.transform);
            clone.transform.position = new Vector3(Random.Range(GameManager.Instance.min_x_position + 7f, GameManager.Instance.max_x_position - 7f),
                Random.Range(GameManager.Instance.min_y_position + 7f, GameManager.Instance.max_y_position - 7f), 0);
            
        }
        if (poolName == "Hunter")
        {
            activeList.Add(clone.transform);
            clone.transform.position = new Vector3(Random.Range(GameManager.Instance.min_x_position + 7f, GameManager.Instance.max_x_position - 7f),
                Random.Range(GameManager.Instance.min_y_position + 7f, GameManager.Instance.max_y_position - 7f), 0);
            
        }
        if (poolName == "Tree")
        {
            activeList.Add(clone.transform);
            clone.transform.position = new Vector3(Random.Range(GameManager.Instance.min_x_position + 7f, GameManager.Instance.max_x_position - 7f),
                Random.Range(GameManager.Instance.min_y_position + 7f, GameManager.Instance.max_y_position - 7f), 0);
            
        }
        return clone;
    }
    public void returnObjectToPool(string poolName, GameObject returninObject)
    {
        // Get the pool with the given pool name
        ObjectPool pool = GetObjectPool(poolName);

        pool.returnToPool(returninObject);

        Despawn(returninObject);
    }
    public GameObject Spawn(string poolName, Vector3 position, Quaternion rotation)
    {
        // Spawn the gameobject
        GameObject clone = Spawn(poolName);

        // Set its position and rotation
        if (clone != null)
        {
            clone.transform.position = position;
            clone.transform.rotation = rotation;

            return clone;
        }

        return null;
    }
    public GameObject Spawn(string poolName, Vector3 minVector, Vector3 maxVector, Quaternion rotation)
    {
        // Determine the random position
        float x = Random.Range(minVector.x, maxVector.x);
        float y = Random.Range(minVector.y, maxVector.y);
        float z = Random.Range(minVector.z, maxVector.z);
        Vector3 newPosition = new Vector3(x, y, z);

        // Spawn the next gameobject
        return Spawn(poolName, newPosition, rotation);
    }
    public void Despawn(GameObject obj)
    {
        if (obj.CompareTag("Animal"))
        {
            activeList.Remove(obj.transform);
            obj.SetActive(false);
        }
        else if (obj.CompareTag("Hunter"))
        {
            activeList.Remove(obj.transform);
            obj.SetActive(false);
        }
        else if (obj.CompareTag("Chopper"))
        {
            activeList.Remove(obj.transform);
            obj.SetActive(false);
        }
        else if (obj.CompareTag("Tree"))
        {
            activeList.Remove(obj.transform);
            obj.SetActive(false);
        }
    }
    public void Despawn(GameObject obj, float delay)
    {
        Invoke("Despawn", delay);
    }
    private ObjectPool GetObjectPool(string poolName)
    {
        // Find the pool with the given name
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].PoolName.Equals(poolName))
            {
                return pools[i];
            }
        }

        return null;
    }
}
