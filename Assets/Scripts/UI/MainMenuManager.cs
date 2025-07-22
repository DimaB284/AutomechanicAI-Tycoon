using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject hudPanel; // твій HUD

    private void Start()
    {
        ShowMenu(true);
    }

    public void ShowMenu(bool show)
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(show);
        if (hudPanel != null)
            hudPanel.SetActive(!show);
        //Time.timeScale = 0f;
        // Можна поставити Time.timeScale = 0f, якщо треба пауза
    }

    public void OnStartGame()
    {
        ShowMenu(false);
        //Time.timeScale = 1f;
    }

    public void OnQuit()
    {
        Application.Quit();
    }
} 