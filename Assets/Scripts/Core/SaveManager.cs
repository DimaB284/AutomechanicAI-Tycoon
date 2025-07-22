using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int money;
    public int metal;
    public int plastic;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string savePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.money = InventoryManager.Instance.GetResourceAmount("Money");
        data.metal = InventoryManager.Instance.GetResourceAmount("Metal");
        data.plastic = InventoryManager.Instance.GetResourceAmount("Plastic");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
            return;
        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        InventoryManager.Instance.AddResource("Money", data.money - InventoryManager.Instance.GetResourceAmount("Money"));
        InventoryManager.Instance.AddResource("Metal", data.metal - InventoryManager.Instance.GetResourceAmount("Metal"));
        InventoryManager.Instance.AddResource("Plastic", data.plastic - InventoryManager.Instance.GetResourceAmount("Plastic"));
        UIManager.Instance.UpdateMoney(data.money);
        UIManager.Instance.UpdateMetal(data.metal);
        UIManager.Instance.UpdatePlastic(data.plastic);
    }
} 