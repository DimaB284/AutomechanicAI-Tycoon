using UnityEngine;
using System.Linq;

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

        // Перемішуємо точки спавну для випадковості
        var shuffledPoints = spawnPoints.OrderBy(x => Random.value).ToArray();

        foreach (var spawnPoint in shuffledPoints)
        {
            // Перевіряємо, чи точка вільна (немає інших машин поруч)
            Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 2f);
            bool hasCar = colliders.Any(c => c.GetComponent<Car>() != null);

            if (!hasCar)
            {
                GameObject carObj = Instantiate(carPrefab, spawnPoint.position, Quaternion.identity);
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