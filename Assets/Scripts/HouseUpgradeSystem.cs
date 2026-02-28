using UnityEngine;
using System.Collections.Generic;

public class HouseUpgradeSystem : MonoBehaviour
{
    [System.Serializable]
    public class RoomUpgrade
    {
        public string roomName;
        public string description;
        public int cost;
        public bool unlocked;
        public GameObject roomPrefab;
        public Transform spawnPoint;
        public List<string> requiredUpgrades;
    }
    
    [System.Serializable]
    public class ShipUpgrade
    {
        public string upgradeName;
        public string description;
        public int cost;
        public UpgradeType type;
        public float value;
        public bool purchased;
    }
    
    public enum UpgradeType
    {
        Speed,
        Maneuverability,
        AltitudeRange,
        Visibility,
        WeatherResistance,
        FuelEfficiency
    }
    
    [Header("Rooms")]
    public List<RoomUpgrade> availableRooms = new List<RoomUpgrade>();
    
    [Header("Ship Systems")]
    public List<ShipUpgrade> shipUpgrades = new List<ShipUpgrade>();
    
    [Header("Resources")]
    public int playerCredits = 0;
    public int flightsCompleted = 0;
    public int totalDistance = 0;
    
    [Header("Events")]
    public UnityEngine.Events.UnityEvent<string> OnUpgradePurchased;
    public UnityEngine.Events.UnityEvent<string> OnRoomUnlocked;
    
    private FlightController flightController;
    private WeatherManager weatherManager;
    
    void Start()
    {
        InitializeDefaultUpgrades();
        flightController = FindObjectOfType<FlightController>();
        weatherManager = FindObjectOfType<WeatherManager>();
    }
    
    void InitializeDefaultUpgrades()
    {
        // Default ship upgrades
        shipUpgrades.Add(new ShipUpgrade
        {
            upgradeName = "Nano Hull Plating",
            description = "Reduces weather turbulence by 15%",
            cost = 500,
            type = UpgradeType.WeatherResistance,
            value = 0.15f
        });
        
        shipUpgrades.Add(new ShipUpgrade
        {
            upgradeName = "Advanced Gyro",
            description = "Increases turn speed by 20%",
            cost = 750,
            type = UpgradeType.Maneuverability,
            value = 1.2f
        });
        
        shipUpgrades.Add(new ShipUpgrade
            

        {
            upgradeName = "Turbo Propellers",
            description = "Increases max speed by 25%",
            cost = 1000,
            type = UpgradeType.Speed,
            value = 1.25f
        });
        
        shipUpgrades.Add(new ShipUpgrade
        {
            upgradeName = "High Altitude Wings",
            description = "Unlocks altitude up to 5000m",
            cost = 1500,
            type = UpgradeType.AltitudeRange,
            value = 5000f
        });
        
        // Default rooms
        availableRooms.Add(new RoomUpgrade
        {
            roomName = "Cockpit (Phase 1)",
            description = "Your starting flight deck",
            cost = 0,
            unlocked = true
        });
        
        availableRooms.Add(new RoomUpgrade
        {
            roomName = "Navigation Room",
            description = "Advanced maps and route planning",
            cost = 1000,
            unlocked = false,
            requiredUpgrades = new List<string>()
        });
        
        availableRooms.Add(new RoomUpgrade
        {
            roomName = "Engine Bay",
            description = "Access engine systems and upgrades",
            cost = 1500,
            unlocked = false,
            requiredUpgrades = new List<string> { "Turbo Propellers" }
        });
        
        availableRooms.Add(new RoomUpgrade
        {
            roomName = "Observatory",
            description = "360° viewing deck with telescope",
            cost = 2500,
            unlocked = false,
            requiredUpgrades = new List<string> { "High Altitude Wings" }
        });
    }
    
    public bool PurchaseShipUpgrade(string upgradeName)
    {
        var upgrade = shipUpgrades.Find(u => u.upgradeName == upgradeName);
        if (upgrade == null || upgrade.purchased) return false;
        
        if (playerCredits >= upgrade.cost)
        {
            playerCredits -= upgrade.cost;
            upgrade.purchased = true;
            ApplyUpgrade(upgrade);
            OnUpgradePurchased?.Invoke(upgradeName);
            SaveUpgrades();
            return true;
        }
        
        return false;
    }
    
    void ApplyUpgrade(ShipUpgrade upgrade)
    {
        if (flightController == null) return;
        
        switch (upgrade.type)
        {
            case UpgradeType.Speed:
                flightController.baseSpeed *= upgrade.value;
                break;
            case UpgradeType.Maneuverability:
                flightController.turnSpeed *= upgrade.value;
                break;
            case UpgradeType.AltitudeRange:
                flightController.maxAltitude = upgrade.value;
                break;
        }
    }
    
    public bool UnlockRoom(string roomName)
    {
        var room = availableRooms.Find(r => r.roomName == roomName);
        if (room == null || room.unlocked) return false;
        
        // Check requirements
        foreach (var req in room.requiredUpgrades)
        {
            var upg = shipUpgrades.Find(u => u.upgradeName == req);
            if (upg == null || !upg.purchased)
                return false;
        }
        
        if (playerCredits >= room.cost)
        {
            playerCredits -= room.cost;
            room.unlocked = true;
            SpawnRoom(room);
            OnRoomUnlocked?.Invoke(roomName);
            SaveUpgrades();
            return true;
        }
        
        return false;
    }
    
    void SpawnRoom(RoomUpgrade room)
    {
        if (room.roomPrefab != null && room.spawnPoint != null)
        {
            Instantiate(room.roomPrefab, room.spawnPoint.position, room.spawnPoint.rotation);
        }
    }
    
    public void AddCredits(int amount)
    {
        playerCredits += amount;
    }
    
    void SaveUpgrades()
    {
        // Serialize to JSON
        string json = JsonUtility.ToJson(this, true);
        System.IO.File.WriteAllText(
            System.IO.Path.Combine(Application.persistentDataPath, "upgrades.json"),
            json
        );
    }
    
    public int GetCreditDisplay() => playerCredits;
}
