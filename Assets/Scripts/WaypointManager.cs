using UnityEngine;
using System.Collections.Generic;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance { get; private set; }
    
    [Header("Waypoint Settings")]
    public float waypointRadiusMeters = 1000f; // Default 1km
    public bool showDebugGizmos = true;
    
    [Header("Events")]
    public UnityEngine.Events.UnityEvent<string> OnWaypointReached;
    
    private FlightController flightController;
    private List<Destination> activeWaypoints = new List<Destination>();
    private HashSet<string> completedWaypoints = new HashSet<string>();
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        flightController = FindObjectOfType<FlightController>();
    }
    
    void Update()
    {
        CheckWaypointProximity();
    }
    
    void CheckWaypointProximity()
    {
        if (flightController == null) return;
        
        foreach (var waypoint in activeWaypoints)
        {
            if (completedWaypoints.Contains(waypoint.name)) continue;
            
            float distance = CalculateHaversineDistance(
                flightController.latitude, flightController.longitude,
                waypoint.latitude, waypoint.longitude
            );
            
            if (distance <= waypointRadiusMeters)
            {
                WaypointReached(waypoint);
            }
        }
    }
    
    float CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000; // Earth radius in meters
        
        double dLat = (lat2 - lat1) * Mathf.Deg2Rad;
        double dLon = (lon2 - lon1) * Mathf.Deg2Rad;
        
        double a = Mathf.Sin((float)dLat * 0.5f) * Mathf.Sin((float)dLat * 0.5f) +
                   Mathf.Cos((float)lat1 * Mathf.Deg2Rad) * Mathf.Cos((float)lat2 * Mathf.Deg2Rad) *
                   Mathf.Sin((float)dLon * 0.5f) * Mathf.Sin((float)dLon * 0.5f);
        
        double c = 2 * Mathf.Atan2(Mathf.Sqrt((float)a), Mathf.Sqrt((float)(1 - a)));
        
        return (float)(R * c);
    }
    
    void WaypointReached(Destination waypoint)
    {
        completedWaypoints.Add(waypoint.name);
        OnWaypointReached?.Invoke(waypoint.name);
        Debug.Log($"Waypoint reached: {waypoint.name}");
    }
    
    public void AddWaypoint(string name, double lat, double lon)
    {
        activeWaypoints.Add(new Destination { name = name, latitude = lat, longitude = lon });
    }
    
    public void ClearWaypoints()
    {
        activeWaypoints.Clear();
        completedWaypoints.Clear();
    }
    
    void OnDrawGizmos()
    {
        if (!showDebugGizmos || Application.isPlaying) return;
        
        Gizmos.color = Color.cyan;
        foreach (var wp in activeWaypoints)
        {
            // Visualize at world origin for editor
            Gizmos.DrawWireSphere(Vector3.zero, 10f);
        }
    }
    
    [System.Serializable]
    public class Destination
    {
        public string name;
        public double latitude;
        public double longitude;
    }
}
