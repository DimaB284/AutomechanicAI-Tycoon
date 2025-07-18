using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingName;
    public int cost;
    public Sprite icon;
    public int incomePerSecond = 1;
    public float incomeInterval = 1f;

    private void Start()
    {
        StartCoroutine(GenerateIncome());
    }

    private System.Collections.IEnumerator GenerateIncome()
    {
        while (true)
        {
            yield return new WaitForSeconds(incomeInterval);
            GameManager.Instance.AddMoney(incomePerSecond);
        }
    }
}
