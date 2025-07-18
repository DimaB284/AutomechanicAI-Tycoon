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
        // Завантажуємо прогрес
        SaveManager.Instance.LoadGame();

        // Запускаємо ігровий час
        TimeManager.Instance.StartGameTime();

        // Якщо гра запускається вперше, додаємо стартові ресурси
        if (InventoryManager.Instance.GetResourceAmount("Money") == 0 &&
            InventoryManager.Instance.GetResourceAmount("Metal") == 0 &&
            InventoryManager.Instance.GetResourceAmount("Plastic") == 0)
        {
            InventoryManager.Instance.AddResource("Money", 100);
            InventoryManager.Instance.AddResource("Metal", 10);
            InventoryManager.Instance.AddResource("Plastic", 8);
        }

        // Оновлюємо HUD
        UIManager.Instance.UpdateMoney(InventoryManager.Instance.GetResourceAmount("Money"));
        UIManager.Instance.UpdateMetal(InventoryManager.Instance.GetResourceAmount("Metal"));
        UIManager.Instance.UpdatePlastic(InventoryManager.Instance.GetResourceAmount("Plastic"));
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

    // ������ ��� ����������/������������, �������� �� ������� ����
}