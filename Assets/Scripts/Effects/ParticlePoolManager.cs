using System.Collections.Generic;
using UnityEngine;

namespace TreePlanQAQ.Effects
{
    public class ParticlePoolManager : MonoBehaviour
    {
        public static ParticlePoolManager Instance { get; private set; }
        
        [Header("Pool Settings")]
        public int poolSize = 20;
        public bool autoExpand = true;
        
        private Dictionary<string, Queue<GameObject>> particlePools = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, GameObject> poolPrefabs = new Dictionary<string, GameObject>();
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        
        public void RegisterPool(string poolId, GameObject prefab)
        {
            if (particlePools.ContainsKey(poolId))
            {
                Debug.LogWarning($"Pool {poolId} already exists!");
                return;
            }
            
            poolPrefabs[poolId] = prefab;
            particlePools[poolId] = new Queue<GameObject>();
            
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                particlePools[poolId].Enqueue(obj);
            }
        }
        
        public GameObject GetFromPool(string poolId)
        {
            if (!particlePools.ContainsKey(poolId))
            {
                Debug.LogError($"Pool {poolId} not found!");
                return null;
            }
            
            if (particlePools[poolId].Count > 0)
            {
                GameObject obj = particlePools[poolId].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            
            if (autoExpand && poolPrefabs.ContainsKey(poolId))
            {
                GameObject newObj = Instantiate(poolPrefabs[poolId], transform);
                return newObj;
            }
            
            return null;
        }
        
        public void ReturnToPool(string poolId, GameObject obj)
        {
            if (!particlePools.ContainsKey(poolId))
            {
                Debug.LogError($"Pool {poolId} not found!");
                return;
            }
            
            obj.SetActive(false);
            particlePools[poolId].Enqueue(obj);
        }
        
        public void ClearPool(string poolId)
        {
            if (!particlePools.ContainsKey(poolId)) return;
            
            while (particlePools[poolId].Count > 0)
            {
                GameObject obj = particlePools[poolId].Dequeue();
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
        }
    }
}
