using UnityEngine;
using System.Collections;

public class MechanicAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float repairTime = 3f;
    public float approachDistance = 2f; // Відстань, на якій робот починає ремонт

    private enum State { Idle, MoveToCar, Repair }
    private State currentState = State.Idle;

    private Car targetCar;
    private Vector3 startPosition;
    private float stuckTimer = 0f;
    private Vector3 lastPosition;
    private Animator animator;

    private void Start()
    {
        startPosition = transform.position;
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
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
            stuckTimer = 0f;
            Debug.Log($"Робот знайшов машину на відстані {minDist}");
        }
    }

    void MoveToTarget()
    {
        if (targetCar == null || targetCar.isRepaired)
        {
            if (animator != null)
                animator.SetBool("IsWalking", false);
            currentState = State.Idle;
            return;
        }

        // Рухаємося до машини тільки по XZ
        Vector3 targetPos = targetCar.transform.position;
        targetPos.y = transform.position.y; // залишаємо поточний Y

        float distanceToCar = Vector3.Distance(transform.position, targetPos);

        // Перевіряємо чи робот не застряг
        if (Vector3.Distance(transform.position, lastPosition) < 0.01f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > 2f)
            {
                if (animator != null)
                    animator.SetBool("IsWalking", false);
                Debug.Log("Робот застряг, шукаємо іншу машину");
                targetCar = null;
                currentState = State.Idle;
                return;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        // Якщо дійшли до машини
        if (distanceToCar <= approachDistance)
        {
            if (animator != null)
                animator.SetBool("IsWalking", false);
            Debug.Log($"Робот дійшов до машини, відстань: {distanceToCar}");
            currentState = State.Repair;
            return;
        }

        // Рухаємося до машини
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        // Розвертаємо робота у напрямку руху (тільки по Y)
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f);
        }

        if (animator != null)
            animator.SetBool("IsWalking", true);

        lastPosition = transform.position;
    }

    IEnumerator RepairCar()
    {
        if (targetCar == null)
        {
            if (animator != null)
                animator.SetBool("IsWalking", false);
            currentState = State.Idle;
            yield break;
        }

        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("Repair");
        }

        Debug.Log("Робот починає ремонт");

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
                neededResource = "Tires";
                resourceCost = 2;
                reward = 14;
                break;
            case Car.DamageType.Body:
                neededResource = "Plastic";
                resourceCost = 2;
                reward = 15;
                break;
            case Car.DamageType.Electronics:
                neededResource = "Electronics";
                resourceCost = 2;
                reward = 18;
                break;
            case Car.DamageType.Paint:
                neededResource = "Paint";
                resourceCost = 1;
                reward = 8;
                break;
        }

        if (InventoryManager.Instance == null || !InventoryManager.Instance.HasResource(neededResource, resourceCost))
        {
            Debug.Log($"Недостатньо ресурсів: {neededResource}");
            targetCar = null;
            currentState = State.Idle;
            yield break;
        }

        InventoryManager.Instance.RemoveResource(neededResource, resourceCost);
        targetCar.StartRepair();
        
        Debug.Log($"Ремонт триває {repairTime} секунд");
        yield return new WaitForSeconds(repairTime);
        
        if (targetCar != null)
        {
            targetCar.CompleteRepair();
            Debug.Log("Ремонт завершено");
        }

        // Додаємо гроші
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddResource("Money", reward);

        // Оновлюємо UI
        if (UIManager.Instance != null && InventoryManager.Instance != null)
        {
            UIManager.Instance.UpdateMoney(InventoryManager.Instance.GetResourceAmount("Money"));
            UIManager.Instance.UpdateMetal(InventoryManager.Instance.GetResourceAmount("Metal"));
            UIManager.Instance.UpdatePlastic(InventoryManager.Instance.GetResourceAmount("Plastic"));
            UIManager.Instance.UpdateElectronics(InventoryManager.Instance.GetResourceAmount("Electronics"));
            UIManager.Instance.UpdateTires(InventoryManager.Instance.GetResourceAmount("Tires"));
            UIManager.Instance.UpdatePaint(InventoryManager.Instance.GetResourceAmount("Paint"));
        }

        targetCar = null;
        currentState = State.Idle;
    }
} 