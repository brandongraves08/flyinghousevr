using UnityEngine;

public class FlightRewards : MonoBehaviour
{
    public static FlightRewards Instance { get; private set; }
    
    [Header("Rewards")]
    public int baseFlightReward = 50;
    public int waypointBonus = 25;
    public int scenarioCompleteBonus = 100;
    public int altitudeMilestoneBonus = 30;
    public float timeMultiplier = 1f;
    
    private HouseUpgradeSystem upgradeSystem;
    private FlightController flightController;
    private float flightStartTime;
    private bool flightActive = false;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        upgradeSystem = FindObjectOfType<HouseUpgradeSystem>();
        flightController = FindObjectOfType<FlightController>();
        StartFlightTracking();
    }
    
    public void StartFlightTracking()
    {
        flightStartTime = Time.time;
        flightActive = true;
    }
    
    public void EndFlightAndReward()
    {
        if (!flightActive) return;
        flightActive = false;
        
        float duration = Time.time - flightStartTime;
        int reward = CalculateReward(duration);
        
        if (upgradeSystem != null)
        {
            upgradeSystem.AddCredits(reward);
        }
        
        // Show reward
        HUDManager.Instance?.ShowMessage($"Flight Complete!", $"+", 3f);
    }
    
    int CalculateReward(float duration)
    {
        int reward = baseFlightReward;
        
        // Altitude bonus
        if (flightController != null)
        {
            if (flightController.altitude > 1000) reward += altitudeMilestoneBonus;
            if (flightController.altitude > 2000) reward += altitudeMilestoneBonus * 2;
        }
        
        // Time multiplier (capped at 2x for 10 min flight)
        float timeBonus = Mathf.Min(duration / 600f, 1f) * timeMultiplier;
        reward += Mathf.RoundToInt(reward * timeBonus);
        
        return reward;
    }
    
    public void RewardWaypointReached()
    {
        if (upgradeSystem != null)
        {
            upgradeSystem.AddCredits(waypointBonus);
            HUDManager.Instance?.ShowMessage("Waypoint Reached!", $"+", 2f);
        }
    }
    
    public void RewardScenarioComplete()
    {
        if (upgradeSystem != null)
        {
            upgradeSystem.AddCredits(scenarioCompleteBonus);
            HUDManager.Instance?.ShowMessage("Scenario Complete!", $"+", 3f);
        }
    }
}
