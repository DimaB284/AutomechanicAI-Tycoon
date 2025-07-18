using UnityEngine;

public class BuildButton : MonoBehaviour
{
    public GameObject buildingPrefab;

    public void OnClick()
    {
        Debug.Log("Button clicked!");
        BuildManager.Instance.SelectBuilding(buildingPrefab);
    }
}
