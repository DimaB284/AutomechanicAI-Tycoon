using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGamePaused { get; private set; }

    private float autosaveInterval = 30f;
    private float autosaveTimer = 0f;

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

    private void Start()
    {
        SaveManager.Instance.LoadGame();

        TimeManager.Instance.StartGameTime();

        if (InventoryManager.Instance.GetResourceAmount("Money") == 0 &&
            InventoryManager.Instance.GetResourceAmount("Metal") == 0 &&
            InventoryManager.Instance.GetResourceAmount("Plastic") == 0 &&
            InventoryManager.Instance.GetResourceAmount("Electronics") == 0 &&
            InventoryManager.Instance.GetResourceAmount("Tires") == 0 &&
            InventoryManager.Instance.GetResourceAmount("Paint") == 0)
        {
            InventoryManager.Instance.AddResource("Money", 200);
            InventoryManager.Instance.AddResource("Metal", 20);
            InventoryManager.Instance.AddResource("Plastic", 15);
            InventoryManager.Instance.AddResource("Electronics", 20);
            InventoryManager.Instance.AddResource("Tires", 30);
            InventoryManager.Instance.AddResource("Paint", 100);
        }

        UIManager.Instance.UpdateMoney(InventoryManager.Instance.GetResourceAmount("Money"));
        UIManager.Instance.UpdateMetal(InventoryManager.Instance.GetResourceAmount("Metal"));
        UIManager.Instance.UpdatePlastic(InventoryManager.Instance.GetResourceAmount("Plastic"));
        UIManager.Instance.UpdateElectronics(InventoryManager.Instance.GetResourceAmount("Electronics"));
        UIManager.Instance.UpdateTires(InventoryManager.Instance.GetResourceAmount("Tires"));
        UIManager.Instance.UpdatePaint(InventoryManager.Instance.GetResourceAmount("Paint"));
    }

    private void Update()
    {
        autosaveTimer += Time.deltaTime;
        if (autosaveTimer >= autosaveInterval)
        {
            SaveManager.Instance.SaveGame();
            autosaveTimer = 0f;
        }
    }

    public void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1f;
    }
}