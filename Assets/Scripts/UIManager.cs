using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("Main Panels")]
    public GameObject mainMenuPanel;
    public GameObject pausePanel;
    public GameObject hudPanel;
    public GameObject mapPanel;
    public GameObject upgradePanel;
    public GameObject scenarioPanel;
    
    [Header("HUD Elements")]
    public TextMeshProUGUI altitudeText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI coordinatesText;
    public TextMeshProUGUI creditsText;
    public TextMeshProUGUI missionText;
    public Slider throttleSlider;
    public Slider steeringSlider;
    public Image weatherIcon;
    
    [Header("Selection")]
    public Button[] menuButtons;
    public int selectedButton = 0;
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;
    
    [Header("VR Support")]
    public bool useLaserPointer = true;
    public LineRenderer laserLine;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        ShowMainMenu();
    }
    
    void Update()
    {
        HandleInput();
        UpdateHUD();
    }
    
    void HandleInput()
    {
        // Toggle panels
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgrades();
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleScenarios();
        }
    }
    
    void UpdateHUD()
    {
        var flight = FindObjectOfType<FlightController>();
        var weather = FindObjectOfType<WeatherManager>();
        var upgrades = FindObjectOfType<HouseUpgradeSystem>();
        
        if (flight != null)
        {
            if (altitudeText != null)
                altitudeText.text = $"{(int)flight.altitude}m";
            
            if (coordinatesText != null)
                coordinatesText.text = $"{flight.latitude:F2}, {flight.longitude:F2}";
            
            if (throttleSlider != null)
            {
                float altitudePercent = Mathf.InverseLerp((float)flight.minAltitude, (float)flight.maxAltitude, (float)flight.altitude);
                throttleSlider.value = altitudePercent;
            }
        }
        
        if (weather != null && missionText != null)
        {
            missionText.text = weather.currentWeather;
        }
        
        if (upgrades != null && creditsText != null)
        {
            creditsText.text = $"${upgrades.playerCredits}";
        }
    }
    
    public void ShowMainMenu()
    {
        HideAllPanels();
        mainMenuPanel?.SetActive(true);
    }
    
    void HideAllPanels()
    {
        mainMenuPanel?.SetActive(false);
        pausePanel?.SetActive(false);
        hudPanel?.SetActive(true);
        mapPanel?.SetActive(false);
        upgradePanel?.SetActive(false);
        scenarioPanel?.SetActive(false);
    }
    
    public void TogglePause()
    {
        bool isPaused = pausePanel?.activeSelf ?? false;
        
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            HideAllPanels();
            pausePanel?.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    
    public void ResumeGame()
    {
        pausePanel?.SetActive(false);
        hudPanel?.SetActive(true);
        Time.timeScale = 1f;
    }
    
    public void ToggleMap()
    {
        bool isMapOpen = mapPanel?.activeSelf ?? false;
        
        if (isMapOpen)
        {
            mapPanel?.SetActive(false);
            hudPanel?.SetActive(true);
        }
        else
        {
            HideAllPanels();
            mapPanel?.SetActive(true);
        }
    }
    
    public void ToggleUpgrades()
    {
        bool isOpen = upgradePanel?.activeSelf ?? false;
        
        if (isOpen)
        {
            upgradePanel?.SetActive(false);
            hudPanel?.SetActive(true);
        }
        else
        {
            HideAllPanels();
            upgradePanel?.SetActive(true);
        }
    }
    
    public void ToggleScenarios()
    {
        bool isOpen = scenarioPanel?.activeSelf ?? false;
        
        if (isOpen)
        {
            scenarioPanel?.SetActive(false);
            hudPanel?.SetActive(true);
        }
        else
        {
            HideAllPanels();
            scenarioPanel?.SetActive(true);
        }
    }
    
    public void StartGame()
    {
        HideAllPanels();
        hudPanel?.SetActive(true);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void ShowMessage(string message, float duration = 3f)
    {
        HUDManager.Instance?.ShowMessage(message, "", duration);
    }
}
