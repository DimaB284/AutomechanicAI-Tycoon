using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<string, int> resources = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddResource(string resourceType, int amount)
    {
        if (!resources.ContainsKey(resourceType))
            resources[resourceType] = 0;
        resources[resourceType] += amount;
    }

    public bool RemoveResource(string resourceType, int amount)
    {
        if (!resources.ContainsKey(resourceType) || resources[resourceType] < amount)
            return false;
        resources[resourceType] -= amount;
        return true;
    }

    public bool HasResource(string resourceType, int amount)
    {
        return resources.ContainsKey(resourceType) && resources[resourceType] >= amount;
    }

    public int GetResourceAmount(string resourceType)
    {
        return resources.ContainsKey(resourceType) ? resources[resourceType] : 0;
    }
} 