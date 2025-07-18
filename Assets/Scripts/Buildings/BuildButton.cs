using UnityEngine;

public class BuildButton : MonoBehaviour
{
    public GameObject buildingPrefab;

    public void OnClick()
    {
        BuildManager.Instance.SelectBuilding(buildingPrefab);
    }
}
