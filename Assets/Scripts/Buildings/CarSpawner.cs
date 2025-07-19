using UnityEngine;
using System.Linq;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; // Масив різних префабів машин
    [Tooltip("Ймовірності для кожного типу машини (сума = 1.0)")]
    public float[] carChances; // Ймовірності для кожного типу машини
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public float carSpawnRadius = 2f;
    public float mechanicSafeRadius = 2.5f;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCar();
            timer = 0f;
        }
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

    void SpawnCar()
    {
        if (spawnPoints.Length == 0 || carPrefabs == null || carPrefabs.Length == 0)
            return;

        // Перемішуємо точки спавну для випадковості
        var shuffledPoints = spawnPoints.OrderBy(x => Random.value).ToArray();

        foreach (var spawnPoint in shuffledPoints)
        {
            Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, carSpawnRadius);
            bool hasCar = colliders.Any(c => c.GetComponent<Car>() != null);
            bool hasMechanic = colliders.Any(c => c.GetComponent<MechanicAI>() != null);
            bool mechanicTooClose = Physics.OverlapSphere(spawnPoint.position, mechanicSafeRadius)
                .Any(c => c.GetComponent<MechanicAI>() != null);

            if (!hasCar && !hasMechanic && !mechanicTooClose)
            {
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
                    Car.CarType type = (Car.CarType)Random.Range(0, System.Enum.GetValues(typeof(Car.CarType)).Length);
                    Car.DamageType damage = (Car.DamageType)Random.Range(0, System.Enum.GetValues(typeof(Car.DamageType)).Length);
                    car.Init(type, damage);
                }
                return; // Спавнимо тільки одну машину за раз
            }
        }
        // Якщо всі точки зайняті — не спавнити машину
    }
} 