using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")] 
    public Image moneyIcon;
    public Text moneyText;
    public Image metalIcon;
    public Text metalText;
    public Image plasticIcon;
    public Text plasticText;
    public Image electronicsIcon;
    public Text electronicsText;
    public Image tiresIcon;
    public Text tiresText;
    public Image paintIcon;
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
            moneyText.text = amount.ToString();
    }
    public void UpdateMetal(int amount)
    {
        if (metalText != null)
            metalText.text = amount.ToString();
    }
    public void UpdatePlastic(int amount)
    {
        if (plasticText != null)
            plasticText.text = amount.ToString();
    }
    public void UpdateElectronics(int amount)
    {
        if (electronicsText != null)
            electronicsText.text = amount.ToString();
    }
    public void UpdateTires(int amount)
    {
        if (tiresText != null)
            tiresText.text = amount.ToString();
    }
    public void UpdatePaint(int amount)
    {
        if (paintText != null)
            paintText.text = amount.ToString();
    }

    public void ShowPauseMenu(bool show)
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(show);
    }
} 