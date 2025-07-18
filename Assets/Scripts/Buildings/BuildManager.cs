using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public GameObject buildingToPlace;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectBuilding(GameObject buildingPrefab)
    {
        buildingToPlace = buildingPrefab;
    }

    private void Update()
    {
        if (buildingToPlace == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Instantiate(buildingToPlace, hit.point, Quaternion.identity);
                buildingToPlace = null;
            }
        }

        // Для зручності: Escape відміняє будівництво
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            buildingToPlace = null;
        }
    }
}
