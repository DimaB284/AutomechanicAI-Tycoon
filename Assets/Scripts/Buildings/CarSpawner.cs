using UnityEngine;
using System.Linq;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; // Масив різних префабів машин
    [Tooltip("Ймовірності для кожного типу машини (сума = 1.0)")]
    public float[] carChances; // Ймовірності для кожного типу машини
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public float carSpawnRadius = 1.0f; // Зменшено для точнішої перевірки
    public float mechanicSafeRadius = 2.5f;
    public GameObject problemIndicatorPrefab;

    [HideInInspector]
    public bool[] spawnOccupied;

    private float timer = 0f;
    public static CarSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        spawnOccupied = new bool[spawnPoints.Length];
    }

    public void FreeSpawnPoint(int index)
    {
        if (index >= 0 && index < spawnOccupied.Length)
            spawnOccupied[index] = false;
    }

    GameObject GetRandomCarPrefab()
    {
        if (carPrefabs == null || carPrefabs.Length == 0 || carChances == null || carChances.Length != carPrefabs.Length)
            return null;
        float rand = Random.value;
        float cumulative = 0f;
        for (int i = 0; i < carPrefabs.Length; i++)
        {
            cumulative += carChances[i];
            if (rand < cumulative)
                return carPrefabs[i];
        }
        return carPrefabs[carPrefabs.Length - 1]; // fallback
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            TrySpawnCar();
            timer = 0f;
        }
    }

    void TrySpawnCar()
    {
        if (spawnPoints.Length == 0 || carPrefabs == null || carPrefabs.Length == 0)
            return;

        // Збираємо всі вільні spawnPoints
        var freeIndices = Enumerable.Range(0, spawnPoints.Length)
            .Where(i => !spawnOccupied[i])
            .ToList();

        if (freeIndices.Count == 0)
            return;

        int chosen = freeIndices[Random.Range(0, freeIndices.Count)];
        var spawnPoint = spawnPoints[chosen];

        GameObject prefab = GetRandomCarPrefab();
        if (prefab == null) return;
        GameObject carObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        // Притискаємо машину до землі через Raycast
        RaycastHit hit;
        if (Physics.Raycast(carObj.transform.position + Vector3.up * 2f, Vector3.down, out hit, 10f))
            carObj.transform.position = hit.point;

        Car car = carObj.GetComponent<Car>();
        if (car != null)
        {
            car.spawnPointIndex = chosen;
            Car.CarType type = (Car.CarType)Random.Range(0, System.Enum.GetValues(typeof(Car.CarType)).Length);
            Car.DamageType damage = (Car.DamageType)Random.Range(0, System.Enum.GetValues(typeof(Car.DamageType)).Length);
            car.Init(type, damage);
            if (problemIndicatorPrefab != null)
            {
                var indicator = Instantiate(problemIndicatorPrefab, carObj.transform);
                // Динамічно визначаємо верхню точку машини
                Renderer rend = carObj.GetComponentInChildren<Renderer>();
                float yOffset = 2f;
                if (rend != null)
                    yOffset = rend.bounds.max.y - carObj.transform.position.y + 0.5f;
                indicator.transform.localPosition = new Vector3(0, yOffset, 0);
                var pi = indicator.GetComponent<ProblemIndicator>();
                if (pi != null)
                    pi.SetProblem(damage);
            }
        }
        spawnOccupied[chosen] = true;
    }
}