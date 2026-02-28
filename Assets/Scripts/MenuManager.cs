using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject scenarioSelectPanel;
    public GameObject loadingScreen;
    
    [Header("Settings")]
    public UnityEngine.UI.Slider volumeSlider;
    public UnityEngine.UI.Toggle vrToggle;
    public TMPro.TMP_Dropdown qualityDropdown;
    
    [Header("Loading")]
    public TMPro.TextMeshProUGUI loadingText;
    public UnityEngine.UI.Slider loadingBar;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        ShowMainMenu();
        LoadSettings();
    }
    
    public void ShowMainMenu()
    {
        HideAllPanels();
        mainMenuPanel?.SetActive(true);
    }
    
    public void ShowSettings()
    {
        HideAllPanels();
        settingsPanel?.SetActive(true);
    }
    
    public void ShowScenarioSelect()
    {
        HideAllPanels();
        scenarioSelectPanel?.SetActive(true);
    }
    
    void HideAllPanels()
    {
        mainMenuPanel?.SetActive(false);
        settingsPanel?.SetActive(false);
        scenarioSelectPanel?.SetActive(false);
        loadingScreen?.SetActive(false);
    }
    
    public void StartGame(string scenarioName)
    {
        ShowLoadingScreen();
        SceneManager.LoadSceneAsync("FlyingHouse").completed += (asyncOp) => {
            // Scene loaded, initialize scenario
            var scenarioManager = FindObjectOfType<ScenarioManager>();
            if (scenarioManager != null && !string.IsNullOrEmpty(scenarioName))
            {
                scenarioManager.StartScenario(scenarioName);
            }
        };
    }
    
    public void QuickStart()
    {
        StartGame("");
    }
    
    void ShowLoadingScreen()
    {
        HideAllPanels();
        loadingScreen?.SetActive(true);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
    
    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }
    
    void LoadSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        if (volumeSlider != null) volumeSlider.value = savedVolume;
        
        int savedQuality = PlayerPrefs.GetInt("Quality", 2);
        if (qualityDropdown != null) qualityDropdown.value = savedQuality;
    }
    
    public void SaveSettings()
    {
        if (volumeSlider != null)
            PlayerPrefs.SetFloat("MasterVolume", volumeSlider.value);
        
        if (qualityDropdown != null)
            PlayerPrefs.SetInt("Quality", qualityDropdown.value);
        
        PlayerPrefs.Save();
    }
}
