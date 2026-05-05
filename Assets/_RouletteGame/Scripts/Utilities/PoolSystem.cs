using System.Collections.Generic;
using UnityEngine;

namespace _RouletteGame.Utilities
{
     public class PoolSystem : MonoBehaviour
    {
        public static PoolSystem Instance { get; private set; }
        
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
            SpawnPool();
        }

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        void SpawnPool()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    obj.AddComponent<PoolableObject>().PoolTag = pool.tag;
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnGameObject(string poolTag)
        {
            if (!poolDictionary.ContainsKey(poolTag))
            {
                Debug.LogWarning("Pool with tag " + poolTag + " doesn't exist.");
                return null;
            }
            
            if (poolDictionary[poolTag].Count == 0)
            {
                Debug.LogWarning("Pool is empty, adding a new object to pool.");

                Pool pool = pools.Find(p => p.tag == poolTag);
                if (pool != null)
                {
                    GameObject newObj = Instantiate(pool.prefab);
                    newObj.SetActive(false);
                    newObj.AddComponent<PoolableObject>().PoolTag = pool.tag;
                    poolDictionary[poolTag].Enqueue(newObj);
                }
                else
                {
                    Debug.LogError("Pool with tag " + poolTag + " not found in pool list.");
                    return null;
                }
            }

            GameObject objectToSpawn = poolDictionary[poolTag].Dequeue();
            objectToSpawn.SetActive(true);
            return objectToSpawn;
        }
    
        public void ReturnToPool(string poolTag, GameObject obj) => poolDictionary[poolTag].Enqueue(obj);
    }
}