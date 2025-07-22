using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public float GameTime { get; private set; } 
    public event Action OnSecondPassed;

    private float timer = 0f;

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

    public void StartGameTime()
    {
        GameTime = 0f;
        timer = 0f;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGamePaused)
            return;

        timer += Time.deltaTime;
        GameTime += Time.deltaTime;

        if (timer >= 1f)
        {
            timer -= 1f;
            OnSecondPassed?.Invoke();
        }
    }
}