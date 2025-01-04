using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Dictionary<string, Queue<T>> poolDictionary = new Dictionary<string, Queue<T>>();
    private readonly Dictionary<string, T> prefabDictionary = new Dictionary<string, T>();
    private readonly Transform parent;

    public ObjectPool(Dictionary<string, T> prefabs, int initialSize, Transform parent = null)
    {
        this.parent = parent;

        foreach (var prefabEntry in prefabs)
        {
            string type = prefabEntry.Key;
            T prefab = prefabEntry.Value;

            prefabDictionary[type] = prefab;
            poolDictionary[type] = new Queue<T>();

            for (int i = 0; i < initialSize; i++)
            {
                T obj = Object.Instantiate(prefab, parent);
                obj.gameObject.SetActive(false);
                poolDictionary[type].Enqueue(obj);
            }
        }
    }

    public T Get(string type)
    {
        if (poolDictionary.TryGetValue(type, out var queue) && queue.Count > 0)
        {
            T obj = queue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        if (prefabDictionary.TryGetValue(type, out var prefab))
        {
            T obj = Object.Instantiate(prefab, parent);
            return obj;
        }

        Debug.LogError($"Prefab of type '{type}' not found in ObjectPool.");
        return null;
    }

    public void Return(T obj, string type)
    {
        obj.gameObject.SetActive(false);

        if (poolDictionary.TryGetValue(type, out var queue))
        {
            queue.Enqueue(obj);
        }
        else
        {
            Debug.LogWarning($"Pool for type '{type}' not found. Destroying the object.");
            Object.Destroy(obj.gameObject);
        }
    }
}
