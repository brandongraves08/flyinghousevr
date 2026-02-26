using UnityEngine;
using System.IO;

[System.Serializable]
public class CalibrationData
{
    public Vector3 wheelPosition;
    public Quaternion wheelRotation;
    public Vector3 leverPosition;
    public Quaternion leverRotation;
}

public class CalibrationManager : MonoBehaviour
{
    [Header("Objects to Calibrate")]
    public Transform steeringWheel;
    public Transform altitudeLever;
    
    private string savePath;
    private CalibrationData currentData;
    
    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "calibration.json");
        LoadCalibration();
    }
    
    void LoadCalibration()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No calibration file found. Using default positions.");
            return;
        }
        
        try
        {
            string json = File.ReadAllText(savePath);
            currentData = JsonUtility.FromJson<CalibrationData>(json);
            
            ApplyCalibration();
            Debug.Log("Calibration loaded successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load calibration: " + e.Message);
        }
    }
    
    void ApplyCalibration()
    {
        if (steeringWheel != null && currentData != null)
        {
            steeringWheel.position = currentData.wheelPosition;
            steeringWheel.rotation = currentData.wheelRotation;
        }
        
        if (altitudeLever != null && currentData != null)
        {
            altitudeLever.position = currentData.leverPosition;
            altitudeLever.rotation = currentData.leverRotation;
        }
    }
    