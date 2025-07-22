using UnityEngine;
using System.Collections;

public class MechanicAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float repairTime = 3f;
    public float approachDistance = 2f;

    private enum State { Idle, MoveToCar, Repair }
    private State currentState = State.Idle;

    private Car targetCar;
    private Vector3 startPosition;
    private float stuckTimer = 0f;
    private Vector3 lastPosition;
    private Animator animator;
    private Rigidbody rb;

    private void Start()
    {
        startPosition = transform.position;
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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
            Debug.Log($"Robot has found a car in distance {minDist}");
        }
    }

    void MoveToTarget()
    {
        if (targetCar == null || targetCar.isRepaired)
        {
            if (animator != null)
                animator.SetBool("isWalking", false);
            currentState = State.Idle;
            return;
        }

        Vector3 targetPos = targetCar.transform.position;
        targetPos.y = transform.position.y;

        float approachDist = GetApproachDistance(targetCar.carType);
        float distanceToCar = Vector3.Distance(transform.position, targetPos);

        if (Vector3.Distance(transform.position, lastPosition) < 0.01f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > 2f)
            {
                if (animator != null)
                    animator.SetBool("isWalking", false);
                Debug.Log("Robot is stuck, searching for car");
                targetCar = null;
                currentState = State.Idle;
                return;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        if (distanceToCar <= approachDist)
        {
            if (animator != null)
                animator.SetBool("isWalking", false);
            Debug.Log($"Robot has reached the car, distance: {distanceToCar}");
            currentState = State.Repair;
            return;
        }

        Vector3 dir = (targetPos - transform.position).normalized;
        Vector3 nextPos = transform.position + dir * moveSpeed * Time.deltaTime;

        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f);
        }

        if (animator != null)
            animator.SetBool("isWalking", true);

        if (rb != null && !rb.isKinematic)
            rb.MovePosition(nextPos);
        else
            transform.position = nextPos;

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

        Vector3 dirToCar = (targetCar.transform.position - transform.position);
        dirToCar.y = 0;
        if (dirToCar != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dirToCar, Vector3.up);
            transform.rotation = lookRotation;
        }

        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("Repair");
        }

        Debug.Log("Robot starts repair");

        string neededResource = "Metal";
        int baseResourceCost = 2;
        int baseReward = 10;
        switch (targetCar.damageType)
        {
            case Car.DamageType.Engine:
                neededResource = "Metal";
                baseResourceCost = 3;
                baseReward = 20;
                break;
            case Car.DamageType.Wheels:
                neededResource = "Plastic";
                baseResourceCost = 2;
                baseReward = 12;
                break;
            case Car.DamageType.Body:
                neededResource = "Metal";
                baseResourceCost = 2;
                baseReward = 15;
                break;
            case Car.DamageType.Electronics:
                neededResource = "Electronics";
                baseResourceCost = 2;
                baseReward = 18;
                break;
            case Car.DamageType.Paint:
                neededResource = "Paint";
                baseResourceCost = 1;
                baseReward = 8;
                break;
        }

        float multiplier = GetCarTypeMultiplier(targetCar.carType);
        int resourceCost = Mathf.CeilToInt(baseResourceCost * multiplier);
        int reward = Mathf.CeilToInt(baseReward * multiplier);
        float repairTime = this.repairTime * multiplier;

        if (InventoryManager.Instance == null || !InventoryManager.Instance.HasResource(neededResource, resourceCost))
        {
            Debug.Log($"Not enough resourses: {neededResource}");
            targetCar = null;
            currentState = State.Idle;
            yield break;
        }

        InventoryManager.Instance.RemoveResource(neededResource, resourceCost);
        targetCar.StartRepair();
        
        Debug.Log($"Repair longs for {repairTime} seconds");
        yield return new WaitForSeconds(repairTime);
        
        if (targetCar != null)
        {
            targetCar.CompleteRepair();
            Debug.Log("Repair isfinished");
        }

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddResource("Money", reward);

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

    float GetCarTypeMultiplier(Car.CarType type)
    {
        switch (type)
        {
            case Car.CarType.Sedan: return 1.0f;
            case Car.CarType.Truck: return 2.0f;
            case Car.CarType.Minivan: return 1.5f;
            case Car.CarType.Bus: return 3.0f;
            default: return 1.0f;
        }
    }

    float GetApproachDistance(Car.CarType type)
    {
        switch (type)
        {
            case Car.CarType.Sedan: return 1.0f;
            case Car.CarType.Truck: return 1.0f;
            case Car.CarType.Minivan: return 1.5f;
            case Car.CarType.Bus: return 1.0f;
            default: return 1.0f;
        }
    }
} 