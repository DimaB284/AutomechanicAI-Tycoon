using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("Prefabs and positions")]
    public GameObject boxPrefab;
    public Transform[] boxSpawnPoints;

    private int currentBoxIndex = 0;

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

    public bool BuildBox(int metalCost, int plasticCost)
    {
        if (currentBoxIndex >= boxSpawnPoints.Length)
            return false;
        if (!InventoryManager.Instance.HasResource("Metal", metalCost) ||
            !InventoryManager.Instance.HasResource("Plastic", plasticCost))
            return false;

        InventoryManager.Instance.RemoveResource("Metal", metalCost);
        InventoryManager.Instance.RemoveResource("Plastic", plasticCost);

        Instantiate(boxPrefab, boxSpawnPoints[currentBoxIndex].position, Quaternion.identity);
        currentBoxIndex++;
        return true;
    }
} 