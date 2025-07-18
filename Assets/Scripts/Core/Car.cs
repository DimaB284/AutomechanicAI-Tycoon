using UnityEngine;

public class Car : MonoBehaviour
{
    public enum CarType { Sedan, SUV, Truck }
    public enum DamageType { Engine, Wheels, Body, Electronics }

    public CarType carType;
    public DamageType damageType;
    public bool isRepaired = false;

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
        // Тут можна додати логіку після ремонту (наприклад, забрати машину зі сцени)
        Destroy(gameObject, 1.5f);
    }
} 