using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    
    [Header("Buttons")]
    public Button playButton;
    public Button vrButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button quitButton;
    
    [Header("Settings")]
    public Slider volumeSlider;
    public Toggle vrToggle;
    public Dropdown qualityDropdown;
    
    [Header("Display")]
    public Text versionText;
    public Text lastFlightText;
    
    void Start()
    {
        // Wire buttons
        playButton?.onClick.AddListener(StartGame);
        settingsButton?.onClick.AddListener(ShowSettings);
        creditsButton?.onClick.AddListener(ShowCredits);
        quitButton?.onClick.AddListener(QuitGame);
        
        // Load settings
        LoadSettings();
        
        // Show version
        if (versionText != null)
            versionText.text = "v1.0 - Phase 2";
        
        // Show last flight
        if (PlayerPrefs.HasKey("LastCity") && lastFlightText != null)
        {
            string city = PlayerPrefs.GetString("LastCity");
            lastFlightText.text = $"Last flight: {city}";
        }
        
        ShowMain();
        
        // Fade in
        StartCoroutine(FadeIn());
    }
    
    System.Collections.IEnumerator FadeIn()
    {
        float alpha = 0;
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null) yield break;
        
        cg.alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * 2;
            cg.alpha = alpha;
            yield return null;
        }
        cg.alpha = 1;
    }
    
    void ShowMain()
    {
        mainPanel?.SetActive(true);
        settingsPanel?.SetActive(false);
        creditsPanel?.SetActive(false);
    }
    
    void StartGame()
    {
        SceneManager.LoadScene("FlyingHouse");
    }
    
    void ShowSettings()
    {
        mainPanel?.SetActive(false);
        settingsPanel?.SetActive(true);
    }
    
    void ShowCredits()
    {
        mainPanel?.SetActive(false);
        creditsPanel?.SetActive(true);
    }
    
    void QuitGame()
    {
        Application.Quit();
    }
    
    public void SaveSettings()
    {
        if (volumeSlider != null)
            PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        
        if (vrToggle != null)
            PlayerPrefs.SetInt("VRMode", vrToggle.isOn ? 1 : 0);
        
        if (qualityDropdown != null)
            PlayerPrefs.SetInt("Quality", qualityDropdown.value);
        
        PlayerPrefs.Save();
        
        // Apply
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 2));
    }
    
    void LoadSettings()
    {
        if (volumeSlider != null)
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        
        if (vrToggle != null)
            vrToggle.isOn = PlayerPrefs.GetInt("VRMode", 0) == 1;
        
        if (qualityDropdown != null)
            qualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);
        
        // Apply loaded settings
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);
    }
    
    public void BackToMain()
    {
        SaveSettings();
        ShowMain();
    }
}
