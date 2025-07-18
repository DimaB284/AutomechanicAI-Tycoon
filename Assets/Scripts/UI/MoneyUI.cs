using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText;

    void Update()
    {
        if (GameManager.Instance != null)
            moneyText.text = "$" + GameManager.Instance.Money.ToString();
    }
} 