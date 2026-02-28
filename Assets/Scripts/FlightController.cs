using UnityEngine;
using CesiumForUnity;

public class FlightController : MonoBehaviour
{
    [Header("Flight Settings")]
    public float baseSpeed = 50f;
    public float minAltitude = 100f;
    public float maxAltitude = 2000f;
    public float turnSpeed = 30f;
    public float altitudeChangeRate = 100f;
    public float pitchSpeed = 20f;
    
    [Header("References")]
    public SteeringWheel steeringWheel;
    public AltitudeLever altitudeLever;
    public CesiumGlobeAnchor globeAnchor;
    public CesiumGeoreference georeference;
    
    [Header("Current State")]
    public double longitude = -74.006f;  // NYC default
    public double latitude = 40.7128f;
    public double altitude = 500f;
    public float heading = 0f;
    public float pitch = 0f;
    
    private Vector3 velocity;
    private bool isFlying = true;
    
    void Start()
    {
        if (globeAnchor == null)
            globeAnchor = GetComponent<CesiumGlobeAnchor>();
            
        if (georeference == null)
            georeference = FindObjectOfType<CesiumGeoreference>();
            
        UpdateGlobePosition();
    }
    
    void Update()
    {
        if (!isFlying) return;
        
        float steering = steeringWheel != null ? steeringWheel.GetSteeringInput() : manualSteeringInput;
        float throttle = altitudeLever != null ? altitudeLever.GetThrottleInput() : 0f;
        
        // Update heading based on steering
        heading += steering * turnSpeed * Time.deltaTime;
        
        // Update altitude based on throttle
        float targetAltitude = Mathf.Clamp(
            (float)altitude + (throttle * altitudeChangeRate * Time.deltaTime),
            minAltitude,
            maxAltitude
        );
        altitude = targetAltitude;
        
        // Calculate speed based on altitude (higher = faster)
        float altitudeFactor = Mathf.InverseLerp(minAltitude, maxAltitude, (float)altitude);
        float currentSpeed = baseSpeed * (0.5f + altitudeFactor * 2f);
        
        // Move in heading direction
        float headingRad = heading * Mathf.Deg2Rad;
        velocity.x = Mathf.Sin(headingRad) * currentSpeed * Time.deltaTime;
        velocity.z = Mathf.Cos(headingRad) * currentSpeed * Time.deltaTime;
        
        // Update geodetic position
        UpdatePositionFromVelocity();
        
        // Apply banking visual
        float bankAngle = -steering * 30f;
        transform.rotation = Quaternion.Euler(pitch, heading, bankAngle);
    }
    
    void UpdatePositionFromVelocity()
    {
        // Convert velocity to lat/lon changes
        float metersPerDegree = 111320f * Mathf.Cos((float)latitude * Mathf.Deg2Rad);
        
        longitude += velocity.x / metersPerDegree;
        latitude += velocity.z / 111320f;
        
        UpdateGlobePosition();
    }
    
    void UpdateGlobePosition()
    {
        if (globeAnchor != null)
        {
            globeAnchor.longitudeLongitudeLatitudeHeight = new double4(longitude, latitude, altitude, 0);
        }
    }
    
    public void SetFlying(bool flying)
    {
        isFlying = flying;
    }
    
    public void ResetPosition(double lon, double lat, double alt)
    {
        longitude = lon;
        latitude = lat;
        altitude = alt;
        heading = 0f;
        UpdateGlobePosition();
    }

    // Desktop input support
    private float manualSteeringInput = 0f;
    
    public void SetManualSteering(float steering)
    {
        manualSteeringInput = steering;
        
        // Apply directly to heading if no steering wheel
        if (steeringWheel == null)
        {
            heading += steering * turnSpeed * Time.deltaTime * 0.5f;
        }
    }
}