using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;

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

    void SpawnCar()
    {
        if (spawnPoints.Length == 0 || carPrefab == null)
            return;
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject carObj = Instantiate(carPrefab, spawnPoint.position, Quaternion.identity);
        Car car = carObj.GetComponent<Car>();
        if (car != null)
        {
            Car.CarType type = (Car.CarType)Random.Range(0, System.Enum.GetValues(typeof(Car.CarType)).Length);
            Car.DamageType damage = (Car.DamageType)Random.Range(0, System.Enum.GetValues(typeof(Car.DamageType)).Length);
            car.Init(type, damage);
        }
    }
} 