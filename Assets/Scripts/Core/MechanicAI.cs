using UnityEngine;
using System.Collections;

public class MechanicAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float repairTime = 3f;

    private enum State { Idle, MoveToCar, Repair }
    private State currentState = State.Idle;

    private Car targetCar;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Idle:
                    FindCar();
                    break;
                case State.MoveToCar:
                    MoveToTarget();
                    break;
                case State.Repair:
                    yield return StartCoroutine(RepairCar());
                    break;
            }
            yield return null;
        }
    }

    void FindCar()
    {
        Car[] cars = FindObjectsOfType<Car>();
        float minDist = float.MaxValue;
        Car nearest = null;
        foreach (var car in cars)
        {
            if (!car.isRepaired)
            {
                float dist = Vector3.Distance(transform.position, car.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = car;
                }
            }
        }
        if (nearest != null)
        {
            targetCar = nearest;
            currentState = State.MoveToCar;
        }
    }

    void MoveToTarget()
    {
        if (targetCar == null || targetCar.isRepaired)
        {
            currentState = State.Idle;
            return;
        }
        Vector3 dir = (targetCar.transform.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetCar.transform.position) < 1.2f)
        {
            currentState = State.Repair;
        }
    }

    IEnumerator RepairCar()
    {
        if (targetCar == null)
        {
            currentState = State.Idle;
            yield break;
        }

        // Витрати ресурсів залежно від типу поломки
        string neededResource = "Metal";
        int resourceCost = 2;
        int reward = 10;
        switch (targetCar.damageType)
        {
            case Car.DamageType.Engine:
                neededResource = "Metal";
                resourceCost = 3;
                reward = 20;
                break;
            case Car.DamageType.Wheels:
                neededResource = "Plastic";
                resourceCost = 2;
                reward = 12;
                break;
            case Car.DamageType.Body:
                neededResource = "Metal";
                resourceCost = 2;
                reward = 15;
                break;
            case Car.DamageType.Electronics:
                neededResource = "Plastic";
                resourceCost = 3;
                reward = 18;
                break;
        }

        if (!InventoryManager.Instance.HasResource(neededResource, resourceCost))
        {
            // Недостатньо ресурсів — шукаємо іншу машину
            targetCar = null;
            currentState = State.Idle;
            yield break;
        }

        InventoryManager.Instance.RemoveResource(neededResource, resourceCost);
        targetCar.StartRepair();
        yield return new WaitForSeconds(repairTime);
        targetCar.CompleteRepair();

        // Додаємо гроші
        InventoryManager.Instance.AddResource("Money", reward);

        // Оновлюємо UI
        UIManager.Instance.UpdateMoney(InventoryManager.Instance.GetResourceAmount("Money"));
        UIManager.Instance.UpdateMetal(InventoryManager.Instance.GetResourceAmount("Metal"));
        UIManager.Instance.UpdatePlastic(InventoryManager.Instance.GetResourceAmount("Plastic"));

        targetCar = null;
        currentState = State.Idle;
    }
} 