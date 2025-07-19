using UnityEngine;

public class TestManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== ТЕСТ МЕНЕДЖЕРІВ ===");
        
        // Перевіряємо чи всі менеджери існують
        if (GameManager.Instance != null)
            Debug.Log("✓ GameManager OK");
        else
            Debug.Log("✗ GameManager НЕ ЗНАЙДЕНО");
            
        if (InventoryManager.Instance != null)
            Debug.Log("✓ InventoryManager OK");
        else
            Debug.Log("✗ InventoryManager НЕ ЗНАЙДЕНО");
            
        if (UIManager.Instance != null)
            Debug.Log("✓ UIManager OK");
        else
            Debug.Log("✗ UIManager НЕ ЗНАЙДЕНО");
            
        if (SaveManager.Instance != null)
            Debug.Log("✓ SaveManager OK");
        else
            Debug.Log("✗ SaveManager НЕ ЗНАЙДЕНО");

        // Тестуємо додавання ресурсів
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddResource("Money", 100);
            InventoryManager.Instance.AddResource("Metal", 10);
            InventoryManager.Instance.AddResource("Plastic", 10);
            InventoryManager.Instance.AddResource("Electronics", 10);
            InventoryManager.Instance.AddResource("Tires", 10);
            InventoryManager.Instance.AddResource("Paint", 10);
            
            Debug.Log($"Ресурси додано: Money={InventoryManager.Instance.GetResourceAmount("Money")}, " +
                     $"Metal={InventoryManager.Instance.GetResourceAmount("Metal")}, " +
                     $"Plastic={InventoryManager.Instance.GetResourceAmount("Plastic")}");
        }

        // Тестуємо оновлення UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMoney(InventoryManager.Instance.GetResourceAmount("Money"));
            UIManager.Instance.UpdateMetal(InventoryManager.Instance.GetResourceAmount("Metal"));
            UIManager.Instance.UpdatePlastic(InventoryManager.Instance.GetResourceAmount("Plastic"));
            UIManager.Instance.UpdateElectronics(InventoryManager.Instance.GetResourceAmount("Electronics"));
            UIManager.Instance.UpdateTires(InventoryManager.Instance.GetResourceAmount("Tires"));
            UIManager.Instance.UpdatePaint(InventoryManager.Instance.GetResourceAmount("Paint"));
            Debug.Log("UI оновлено");
        }
        
        Debug.Log("=== ТЕСТ ЗАВЕРШЕНО ===");
    }
} 