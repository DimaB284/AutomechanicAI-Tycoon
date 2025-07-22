using UnityEngine;

public class Car : MonoBehaviour
{
    public enum CarType { Sedan, SUV, Truck, Minivan, Bus }
    public enum DamageType { Engine, Wheels, Body, Electronics, Paint }

    public CarType carType;
    public DamageType damageType;
    public bool isRepaired = false;
    public bool isPendingDestroy = false;
    public int spawnPointIndex = -1;

    public void Init(CarType type, DamageType damage)
    {
        carType = type;
        damageType = damage;
        isRepaired = false;
    }

    public void StartRepair()
    {
        // Тут можна додати анімацію або ефекти ремонту
    }

    public void CompleteRepair()
    {
        isRepaired = true;
        isPendingDestroy = true;
        Destroy(gameObject, 1.5f);
    }


    private void OnDestroy()
    {
        if (CarSpawner.Instance != null && spawnPointIndex >= 0)
            CarSpawner.Instance.FreeSpawnPoint(spawnPointIndex);
    }
} 
