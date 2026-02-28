using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class ErrorHandler : MonoBehaviour
{
    public static ErrorHandler Instance { get; private set; }
    
    [Header("UI")]
    public GameObject errorPanel;
    public TextMeshProUGUI errorTitle;
    public TextMeshProUGUI errorMessage;
    public Button dismissButton;
    public Button restartButton;
    public Button quitButton;
    
    [Header("Icons")]
    public Sprite infoIcon;
    public Sprite warningIcon;
    public Sprite errorIcon;
    public Image iconDisplay;
    
    private bool isShowing = false;
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        // Subscribe to Unity errors
        Application.logMessageReceived += HandleLog;
    }
    
    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }
    
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Auto-show critical errors
        if (type == LogType.Exception || type == LogType.Error)
        {
            if (ShouldAutoShowError(logString))
            {
                ShowError("Critical Error", logString, true);
            }
        }
    }
    
    bool ShouldAutoShowError(string message)
    {
        // Only show user-relevant errors
        string[] userErrors = {
            "Cesium",
            "XR",
            "VR",
            "SteamVR",
            "Oculus",
            "OpenXR",
            "Network",
            "Connection"
        };
        
        foreach (var term in userErrors)
        {
            if (message.Contains(term)) return true;
        }
        
        return false;
    }
    
    public void ShowInfo(string title, string message)
    {
        ShowDialog(title, message, DialogType.Info, false);
    }
    
    public void ShowWarning(string title, string message)
    {
        ShowDialog(title, message, DialogType.Warning, false);
    }
    
    public void ShowError(string title, string message, bool allowRestart = true)
    {
        ShowDialog(title, message, DialogType.Error, allowRestart);
    }
    
    void ShowDialog(string title, string message, DialogType type, bool allowRestart)
    {
        if (errorPanel == null) return;
        
        errorTitle.text = title;
        errorMessage.text = message;
        
        // Set icon
        switch (type)
        {
            case DialogType.Info:
                if (iconDisplay != null) iconDisplay.sprite = infoIcon;
                break;
            case DialogType.Warning:
                if (iconDisplay != null) iconDisplay.sprite = warningIcon;
                break;
            case DialogType.Error:
                if (iconDisplay != null) iconDisplay.sprite = errorIcon;
                break;
        }
        
        // Show buttons
        if (restartButton != null) restartButton.gameObject.SetActive(allowRestart);
        if (quitButton != null) quitButton.gameObject.SetActive(Application.isEditor == false);
        
        errorPanel.SetActive(true);
        isShowing = true;
        
        // Pause game
        Time.timeScale = 0f;
    }
    
    public void Dismiss()
    {
        errorPanel?.SetActive(false);
        isShowing = false;
        Time.timeScale = 1f;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    
    public void QuitApplication()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
    
    public static void LogErrorSilently(string context, Exception e)
    {
        Debug.LogError($"[{context}] {e.Message}");
        Debug.LogError($"Stack trace: {e.StackTrace}");
    }
    
    enum DialogType { Info, Warning, Error }
}
