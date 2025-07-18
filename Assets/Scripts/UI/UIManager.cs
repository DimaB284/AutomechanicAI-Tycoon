using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    public Text moneyText;
    public Text metalText;
    public Text plasticText;

    [Header("Меню паузи")]
    public GameObject pauseMenuPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateMoney(int amount)
    {
        if (moneyText != null)
            moneyText.text = $"Money: {amount}";
    }

    public void UpdateMetal(int amount)
    {
        if (metalText != null)
            metalText.text = $"Metal: {amount}";
    }

    public void UpdatePlastic(int amount)
    {
        if (plasticText != null)
            plasticText.text = $"Plastic: {amount}";
    }

    public void ShowPauseMenu(bool show)
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(show);
    }
} 