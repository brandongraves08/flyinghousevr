using UnityEngine;
using TMPro;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }
    
    [Header("HUD Elements")]
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI subtitleText;
    public TextMeshProUGUI coordinatesText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI altitudeText;
    public TextMeshProUGUI weatherText;
    
    [Header("Settings")]
    public float fadeSpeed = 2f;
    public float messageDuration = 3f;
    
    private FlightController flightController;
    private WeatherManager weatherManager;
    private Coroutine currentMessage;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        flightController = FindObjectOfType<FlightController>();
        weatherManager = FindObjectOfType<WeatherManager>();
    }
    
    void Update()
    {
        UpdateFlightData();
    }
    
    void UpdateFlightData()
    {
        if (flightController != null)
        {
            if (coordinatesText != null)
            {
                coordinatesText.text = $"{flightController.latitude:F4}°N {flightController.longitude:F4}°W";
            }
            
            if (altitudeText != null)
            {
                altitudeText.text = $"{(int)flightController.altitude}m";
            }
        }
        
        if (weatherManager != null && weatherText != null)
        {
            weatherText.text = weatherManager.currentWeather;
        }
    }
    
    public void ShowMessage(string message, string subtitle = "", float duration = 0)
    {
        float dur = duration > 0 ? duration : messageDuration;
        
        if (currentMessage != null)
            StopCoroutine(currentMessage);
        
        currentMessage = StartCoroutine(DisplayMessage(message, subtitle, dur));
    }
    
    IEnumerator DisplayMessage(string message, string subtitle, float duration)
    {
        if (mainText != null)
        {
            mainText.text = message;
            mainText.alpha = 1f;
        }
        
        if (subtitleText != null)
        {
            subtitleText.text = subtitle;
            subtitleText.alpha = 1f;
        }
        
        yield return new WaitForSeconds(duration);
        
        // Fade out
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            if (mainText != null) mainText.alpha = 1f - t;
            if (subtitleText != null) subtitleText.alpha = 1f - t;
            yield return null;
        }
        
        if (mainText != null) mainText.text = "";
        if (subtitleText != null) subtitleText.text = "";
    }
    
    public void ClearHUD()
    {
        if (currentMessage != null)
            StopCoroutine(currentMessage);
        
        if (mainText != null) mainText.text = "";
        if (subtitleText != null) subtitleText.text = "";
    }
}
