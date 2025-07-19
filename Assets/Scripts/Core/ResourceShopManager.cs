using UnityEngine;

public class ResourceShopManager : MonoBehaviour
{
    public static ResourceShopManager Instance { get; private set; }

    [Header("Ціни на ресурси")] 
    public int metalPrice = 5;
    public int plasticPrice = 4;
    public int electronicsPrice = 8;
    public int tiresPrice = 6;
    public int paintPrice = 2;

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

    public void BuyMetal(int amount)
    {
        BuyResource("Metal", amount, metalPrice);
    }
    public void BuyPlastic(int amount)
    {
        BuyResource("Plastic", amount, plasticPrice);
    }
    public void BuyElectronics(int amount)
    {
        BuyResource("Electronics", amount, electronicsPrice);
    }
    public void BuyTires(int amount)
    {
        BuyResource("Tires", amount, tiresPrice);
    }
    public void BuyPaint(int amount)
    {
        BuyResource("Paint", amount, paintPrice);
    }

    private void BuyResource(string resource, int amount, int price)
    {
        int totalCost = price * amount;
        if (InventoryManager.Instance.GetResourceAmount("Money") < totalCost)
        {
            Debug.Log($"Недостатньо грошей для покупки {resource}!");
            return;
        }
        InventoryManager.Instance.RemoveResource("Money", totalCost);
        InventoryManager.Instance.AddResource(resource, amount);
        Debug.Log($"Куплено {amount} {resource} за {totalCost} грошей");
        // Оновлюємо HUD
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMoney(InventoryManager.Instance.GetResourceAmount("Money"));
            switch (resource)
            {
                case "Metal": UIManager.Instance.UpdateMetal(InventoryManager.Instance.GetResourceAmount("Metal")); break;
                case "Plastic": UIManager.Instance.UpdatePlastic(InventoryManager.Instance.GetResourceAmount("Plastic")); break;
                case "Electronics": UIManager.Instance.UpdateElectronics(InventoryManager.Instance.GetResourceAmount("Electronics")); break;
                case "Tires": UIManager.Instance.UpdateTires(InventoryManager.Instance.GetResourceAmount("Tires")); break;
                case "Paint": UIManager.Instance.UpdatePaint(InventoryManager.Instance.GetResourceAmount("Paint")); break;
            }
        }
    }
} 