using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FlightScenario
{
    public string scenarioName;
    [TextArea(3, 10)]
    public string description;
    public double startLongitude;
    public double startLatitude;
    public double startAltitude = 500;
    public string targetWeather = "Clear";
    public List<string> waypoints = new List<string>();
    public float timeLimit = 0; // 0 = no limit
    public string completionCondition = "explore"; // explore, navigate, storm_survival
}

public class ScenarioManager : MonoBehaviour
{
    [Header("Scenarios")]
    public List<FlightScenario> scenarios = new List<FlightScenario>();
    
    [Header("Current Scenario")]
    public string activeScenarioName = "";
    public bool scenarioActive = false;
    public float scenarioTimer = 0f;

    [Header("References")]
    public FlightController flightController;
    public WeatherManager weatherManager;
    public FlyingHouseManager flyingHouseManager;
    public TMPro.TextMeshProUGUI hudText;
    public AudioSource missionAudio;

    private FlightScenario currentScenario;
    private Dictionary<string, Vector3d> knownLocations = new Dictionary<string, Vector3d>();

    void Start()
    {
        InitializeScenarios();
        InitializeKnownLocations();
    }

    void InitializeKnownLocations()
    {
        knownLocations["NYC"] = new Vector3d(-74.006, 40.7128, 500);
        knownLocations["SF"] = new Vector3d(-122.4194, 37.7749, 500);
        knownLocations["PARIS"] = new Vector3d(2.3522, 48.8566, 500);
        knownLocations["TOKYO"] = new Vector3d(139.6917, 35.6895, 500);
        knownLocations["LONDON"] = new Vector3d(-0.1276, 51.5074, 500);
        knownLocations["SYDNEY"] = new Vector3d(151.2093, -33.8688, 500);
        knownLocations["DUBAI"] = new Vector3d(55.2708, 25.2048, 500);
        knownLocations["RIO"] = new Vector3d(-43.1729, -22.9068, 500);
    }

    void InitializeScenarios()
    {
        scenarios.Add(new FlightScenario 
        { 
            scenarioName = "Calm Skies Over NYC",
            description = "Start in New York and explore the city. Get familiar with the controls.",
            startLongitude = -74.006,
            startLatitude = 40.7128,
            targetWeather = "Clear",
            waypoints = new List<string> { "Statue of Liberty", "Empire State Building", "Central Park" }
        });

        scenarios.Add(new FlightScenario 
        { 
            scenarioName = "Storm Through the Rockies",
            description = "Navigate through turbulent weather over the Rocky Mountains. Keep steady!",
            startLongitude = -106.0,
            startLatitude = 39.5,
            startAltitude = 1200,
            targetWeather = "Storm",
            waypoints = new List<string> { "Aspen", "Vail", "Denver" },
            completionCondition = "storm_survival",
            timeLimit = 300
        });

        scenarios.Add(new FlightScenario 
        { 
            scenarioName = "Night Flight to Paris",
            description = "A serene evening journey across the Atlantic to the City of Light.",
            startLongitude = -74.006,
            startLatitude = 40.7128,
            targetWeather = "Cloudy",
            waypoints = new List<string> { "Atlantic Ocean", "English Channel", "Paris" },
            timeLimit = 600
        });

        scenarios.Add(new FlightScenario 
        { 
            scenarioName = "Tokyo Express",
            description = "High speed flight through Japanese mountains and cities. Test your reflexes!",
            startLongitude = 139.0,
            startLatitude = 35.5,
            startAltitude = 300,
            targetWeather = "Rain",
            waypoints = new List<string> { "Mt Fuji", "Tokyo Tower", "Shibuya" },
            completionCondition = "navigate",
            timeLimit = 180
        });
    }

    public void StartScenario(string scenarioName)
    {
        currentScenario = scenarios.Find(s => s.scenarioName == scenarioName);
        if (currentScenario == null)
        {
            Debug.LogError("Scenario not found: " + scenarioName);
            return;
        }

        activeScenarioName = scenarioName;
        scenarioActive = true;
        scenarioTimer = 0f;

        // Set up flight
        if (flightController != null)
        {
            flightController.ResetPosition(
                currentScenario.startLongitude,
                currentScenario.startLatitude,
                currentScenario.startAltitude
            );
        }

        // Set weather
        if (weatherManager != null)
        {
            weatherManager.SetWeather(currentScenario.targetWeather, true);
        }

        // Display briefing
        ShowBriefing(currentScenario);
        
        Debug.Log("Scenario started: " + scenarioName);
    }

    void ShowBriefing(FlightScenario scenario)
    {
        if (hudText != null)
        {
            hudText.text = $"{scenario.scenarioName}\n{scenario.description}\n\n" +
                          $"Starting: {scenario.startLatitude:F2}, {scenario.startLongitude:F2}\n" +
                          $"Weather: {scenario.targetWeather}";
        }

        if (missionAudio != null)
        {
            missionAudio.Play();
        }

        StartCoroutine(ClearBriefingAfter(5f));
    }

    IEnumerator ClearBriefingAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (hudText != null) hudText.text = "";
    }

    void Update()
    {
        if (!scenarioActive || currentScenario == null) return;

        scenarioTimer += Time.deltaTime;

        // Check time limit
        if (currentScenario.timeLimit > 0 && scenarioTimer > currentScenario.timeLimit)
        {
            ScenarioFailed("Time limit reached");
        }

        // Check completion conditions
        CheckCompletionCondition();
    }

    void CheckCompletionCondition()
    {
        switch (currentScenario.completionCondition.ToLower())
        {
            case "explore":
                // Auto-complete after enough time
                if (scenarioTimer > 60f)
                {
                    ScenarioCompleted();
                }
                break;
            case "storm_survival":
                // Completed when time runs out and player is still flying
                break;
            case "navigate":
                // Would check waypoint proximity
                break;
        }
    }

    public void ScenarioCompleted()
    {
        scenarioActive = false;
        if (hudText != null) hudText.text = "MISSION COMPLETED!";
        Debug.Log("Scenario completed: " + activeScenarioName);
    }

    public void ScenarioFailed(string reason)
    {
        scenarioActive = false;
        if (hudText != null) hudText.text = $"MISSION FAILED: {reason}";
        Debug.Log("Scenario failed: " + reason);
    }

    public void EndScenario()
    {
        scenarioActive = false;
        currentScenario = null;
        activeScenarioName = "";
        if (hudText != null) hudText.text = "";
    }

    public void RestartCurrentScenario()
    {
        if (!string.IsNullOrEmpty(activeScenarioName))
        {
            StartScenario(activeScenarioName);
        }
    }
}

// Helper struct for Vector3 with doubles
public struct Vector3d
{
    public double x, y, z;
    
    public Vector3d(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
