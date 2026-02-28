using UnityEngine;
using System.IO;
using System;

[Serializable]
public class PlayerData
{
    public string playerName;
    public double lastLatitude;
    public double lastLongitude;
    public double lastAltitude;
    public string currentScenario;
    public float flightTime;
    public int flightsCompleted;
    public string[] unlockedLocations;
    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;
    public float mouseSensitivity;
    public bool useDesktopInput;
    public Vector3 wheelCalibration;
    public Vector3 leverCalibration;
}

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    
    private string SavePath => Path.Combine(Application.persistentDataPath, "playerdata.json");
    
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
        LoadGame();
    }
    
    public void SaveGame()
    {
        try
        {
            PlayerData data = GatherPlayerData();
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Game saved to {SavePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }
    
    public void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("No save file found. Creating new profile.");
            CreateNewProfile();
            return;
        }
        
        try
        {
            string json = File.ReadAllText(SavePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            ApplyPlayerData(data);
            Debug.Log("Game loaded successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            CreateNewProfile();
        }
    }
    
    PlayerData GatherPlayerData()
    {
        var flightController = FindObjectOfType<FlightController>();
        var calibration = FindObjectOfType<CalibrationManager>();
        
        return new PlayerData
        {
            playerName = Environment.UserName,
            lastLatitude = flightController?.latitude ?? 40.7128,
            lastLongitude = flightController?.longitude ?? -74.006,
            lastAltitude = flightController?.altitude ?? 500,
            flightTime = Time.realtimeSinceStartup,
            masterVolume = 1f,
            sfxVolume = 1f,
            musicVolume = 0.7f,
            mouseSensitivity = FindObjectOfType<DesktopInputManager>()?.mouseSensitivity ?? 2f,
            useDesktopInput = FindObjectOfType<DesktopInputManager>()?.useDesktopInput ?? true
        };
    }
    
    void ApplyPlayerData(PlayerData data)
    {
        // Apply settings
        var desktop = FindObjectOfType<DesktopInputManager>();
        if (desktop != null)
        {
            desktop.mouseSensitivity = data.mouseSensitivity;
            desktop.useDesktopInput = data.useDesktopInput;
        }
        
        // Restore flight position
        var flightController = FindObjectOfType<FlightController>();
        if (flightController != null && data.currentScenario != null)
        {
            flightController.ResetPosition(data.lastLongitude, data.lastLatitude, data.lastAltitude);
        }
    }
    
    void CreateNewProfile()
    {
        PlayerData data = new PlayerData
        {
            playerName = "Pilot",
            lastLatitude = 40.7128,
            lastLongitude = -74.006,
            lastAltitude = 500,
            mouseSensitivity = 2f,
            useDesktopInput = true
        };
        
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }
    
    public void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save data deleted.");
        }
    }
    
    void OnApplicationQuit()
    {
        SaveGame();
    }
    
    void OnApplicationPause(bool pause)
    {
        if (pause) SaveGame();
    }
}
