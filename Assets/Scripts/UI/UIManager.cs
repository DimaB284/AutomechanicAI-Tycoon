using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    public Text moneyText;
    public Text metalText;
    public Text plasticText;
    public Text electronicsText;
    public Text tiresText;
    public Text paintText;

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

    public void UpdateElectronics(int amount)
    {
        if (electronicsText != null)
            electronicsText.text = $"Electronics: {amount}";
    }

    public void UpdateTires(int amount)
    {
        if (tiresText != null)
            tiresText.text = $"Tires: {amount}";
    }

    public void UpdatePaint(int amount)
    {
        if (paintText != null)
            paintText.text = $"Paint: {amount}";
    }

    public void ShowPauseMenu(bool show)
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(show);
    }
} 