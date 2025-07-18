using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public GameObject buildingToPlace;
    private GameObject previewInstance;
    private Material previewMaterial;
    public ErrorUI errorUI;
    private Color previewColorValid = new Color(1, 1, 1, 0.5f);
    private Color previewColorInvalid = new Color(1, 0.3f, 0.3f, 0.5f);

    private void Awake()
    {
        Instance = this;
    }

    public void SelectBuilding(GameObject buildingPrefab)
    {
        buildingToPlace = buildingPrefab;
        CreatePreview();
    }

    private void Update()
    {
        if (buildingToPlace == null)
        {
            DestroyPreview();
            return;
        }

        UpdatePreviewPosition();
        bool canBuild = CanBuildHere(out Vector3 buildPosition, true);
        SetPreviewColor(canBuild);

        if (Input.GetMouseButtonDown(0))
        {
            if (canBuild)
            {
                Building buildingData = buildingToPlace.GetComponent<Building>();
                if (GameManager.Instance.SpendMoney(buildingData.cost))
                {
                    Vector3 spawnPos = buildPosition + Vector3.up * 3.0f;
                    GameObject newBuilding = Instantiate(buildingToPlace, spawnPos, Quaternion.identity);
                    GameManager.Instance.builtBuildings.Add(buildingData.buildingName);
                    Debug.Log("Building placed at: " + spawnPos);
                }
                else
                {
                    if (errorUI != null)
                        errorUI.ShowError("Недостатньо грошей!");
                    else
                        Debug.Log("Not enough money!");
                }
                buildingToPlace = null;
                DestroyPreview();
            }
            else
            {
                if (errorUI != null)
                    errorUI.ShowError("Не можна будувати тут!");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            buildingToPlace = null;
            DestroyPreview();
        }
    }

    private void CreatePreview()
    {
        DestroyPreview();
        if (buildingToPlace == null) return;
        previewInstance = Instantiate(buildingToPlace);
        foreach (var renderer in previewInstance.GetComponentsInChildren<Renderer>())
        {
            previewMaterial = new Material(renderer.sharedMaterial);
            previewMaterial.color = new Color(1, 1, 1, 0.5f);
            renderer.material = previewMaterial;
        }
        // Вимкнути колайдери для preview
        foreach (var col in previewInstance.GetComponentsInChildren<Collider>())
            col.enabled = false;
    }

    private void UpdatePreviewPosition()
    {
        if (previewInstance == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            previewInstance.transform.position = hit.point;
        }
    }

    private void DestroyPreview()
    {
        if (previewInstance != null)
            Destroy(previewInstance);
    }

    private void SetPreviewColor(bool canBuild)
    {
        if (previewInstance == null) return;
        foreach (var renderer in previewInstance.GetComponentsInChildren<Renderer>())
        {
            if (canBuild)
                renderer.material.color = previewColorValid;
            else
                renderer.material.color = previewColorInvalid;
        }
    }

    private bool CanBuildHere(out Vector3 buildPosition, bool checkOccupied = false)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            buildPosition = hit.point;
            if (checkOccupied)
            {
                float checkRadius = 1.0f;
                Collider[] colliders = Physics.OverlapSphere(buildPosition, checkRadius);
                foreach (var col in colliders)
                {
                    if (col.gameObject.GetComponent<Building>() != null)
                        return false;
                }
            }
            return true;
        }
        buildPosition = Vector3.zero;
        return false;
    }
}
