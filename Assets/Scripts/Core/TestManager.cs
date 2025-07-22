using UnityEngine;

public class TestManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== MANAGERS TEST ===");
        
        if (GameManager.Instance != null)
            Debug.Log("GameManager OK");
        else
            Debug.Log("GameManager NOT FOUND");
            
        if (InventoryManager.Instance != null)
            Debug.Log("InventoryManager OK");
        else
            Debug.Log("InventoryManager NOT FOUND");
            
        if (UIManager.Instance != null)
            Debug.Log("UIManager OK");
        else
            Debug.Log("UIManager NOT FOUND");
            
        if (SaveManager.Instance != null)
            Debug.Log("SaveManager OK");
        else
            Debug.Log("SaveManager NOT FOUND");

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddResource("Money", 100);
            InventoryManager.Instance.AddResource("Metal", 10);
            InventoryManager.Instance.AddResource("Plastic", 10);
            InventoryManager.Instance.AddResource("Electronics", 10);
            InventoryManager.Instance.AddResource("Tires", 10);
            InventoryManager.Instance.AddResource("Paint", 10);
            
            Debug.Log($"Resourses added: Money={InventoryManager.Instance.GetResourceAmount("Money")}, " +
                     $"Metal={InventoryManager.Instance.GetResourceAmount("Metal")}, " +
                     $"Plastic={InventoryManager.Instance.GetResourceAmount("Plastic")}");
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMoney(InventoryManager.Instance.GetResourceAmount("Money"));
            UIManager.Instance.UpdateMetal(InventoryManager.Instance.GetResourceAmount("Metal"));
            UIManager.Instance.UpdatePlastic(InventoryManager.Instance.GetResourceAmount("Plastic"));
            UIManager.Instance.UpdateElectronics(InventoryManager.Instance.GetResourceAmount("Electronics"));
            UIManager.Instance.UpdateTires(InventoryManager.Instance.GetResourceAmount("Tires"));
            UIManager.Instance.UpdatePaint(InventoryManager.Instance.GetResourceAmount("Paint"));
            Debug.Log("UI has been updated");
        }
        
        Debug.Log("=== TEST FINISHED ===");
    }
} 