using UnityEngine;
using CesiumForUnity;

public class FlyingHouseManager : MonoBehaviour
{
    [Header("Core Systems")]
    public FlightController flightController;
    public CalibrationManager calibrationManager;
    public WindowPortal[] windows;
    
    [Header("Cesium")]
    public Cesium3DTileset terrainTileset;
    public CesiumGeoreference georeference;
    
    [Header("Starting Location")]
    public double startLongitude = -74.006f;  // NYC
    public double startLatitude = 40.7128f;
    public double startAltitude = 500f;
    
    [Header("Audio")]
    public AudioSource windAudio;
    public AudioSource ambienceAudio;
    
    void Start()
    {
        InitializeCesium();
        InitializeFlight();
        InitializeAudio();
        
        Debug.Log("Flying House VR initialized. Grab the wheel and take off!");
    }
    
    void InitializeCesium()
    {
        if (georeference == null)
        {
            georeference = FindObjectOfType<CesiumGeoreference>();
            if (georeference == null)
            {
                GameObject geoObj = new GameObject("CesiumGeoreference");
                georeference = geoObj.AddComponent<CesiumGeoreference>();
            }
        }
        
        if (terrainTileset == null)
        {
            terrainTileset = FindObjectOfType<Cesium3DTileset>();
        }
        
        // Position at start
        georeference.longitude = startLongitude;
        georeference.latitude = startLatitude;
        georeference.height = startAltitude;
    }
    
    void InitializeFlight()
    {
        if (flightController != null)
        {
            flightController.ResetPosition(startLongitude, startLatitude, startAltitude);
        }
    }
    
    void InitializeAudio()
    {
        if (windAudio != null)
        {
            windAudio.loop = true;
            windAudio.Play();
        }
        
        if (ambienceAudio != null)
        {
            ambienceAudio.loop = true;
            ambienceAudio.Play();
        }
    }
    
    void Update()
    {
        // Update wind intensity based on speed
        UpdateWindAudio();
    }
    
    void UpdateWindAudio()
    {
        if (windAudio != null && flightController != null)
        {
            // Map wind volume/pitch to flight speed
            float throttle = flightController.altitudeLever != null ? 
                Mathf.Abs(flightController.altitudeLever.GetThrottleInput()) : 0.5f;
            
            windAudio.volume = Mathf.Lerp(0.1f, 0.8f, throttle);
            windAudio.pitch = Mathf.Lerp(0.8f, 1.2f, throttle);
        }
    }
    
    // Called by UI or voice
    public void GoToLocation(string locationName)
    {
        // Could integrate with geocoding API
        switch (locationName.ToLower())
        {
            case "nyc":
            case "new york":
                flightController.ResetPosition(-74.006, 40.7128, 500);
                break;
            case "sf":
            case "san francisco":
                flightController.ResetPosition(-122.4194, 37.7749, 500);
                break;
            case "paris":
                flightController.ResetPosition(2.3522, 48.8566, 500);
                break;
            case "tokyo":
                flightController.ResetPosition(139.6917, 35.6895, 500);
                break;
            default:
                Debug.Log("Unknown location: " + locationName);
                break;
        }
    }
    
    public void PauseFlight()
    {
        if