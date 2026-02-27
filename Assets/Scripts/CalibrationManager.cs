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

    public void SaveCalibration()
    {
        if (steeringWheel == null || altitudeLever == null)
        {
            Debug.LogWarning("Cannot save calibration: references missing.");
            return;
        }

        currentData = new CalibrationData()
        {
            wheelPosition = steeringWheel.position,
            wheelRotation = steeringWheel.rotation,
            leverPosition = altitudeLever.position,
            leverRotation = altitudeLever.rotation
        };

        try
        {
            string json = JsonUtility.ToJson(currentData, true);
            File.WriteAllText(savePath, json);
            Debug.Log("Calibration saved to " + savePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save calibration: " + e.Message);
        }
    }

    public void ResetCalibration()
    {
        if (File.Exists(savePath))
        {
            try
            {
                File.Delete(savePath);
                Debug.Log("Calibration file deleted.");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to delete calibration: " + e.Message);
            }
        }

        // Optionally reset transforms to origin/defaults
        if (steeringWheel != null)
        {
            steeringWheel.localPosition = Vector3.zero;
            steeringWheel.localRotation = Quaternion.identity;
        }
        if (altitudeLever != null)
        {
            altitudeLever.localPosition = Vector3.zero;
            altitudeLever.localRotation = Quaternion.identity;
        }

        currentData = null;
    }
}
