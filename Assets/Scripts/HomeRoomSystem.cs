using UnityEngine;
using System.Collections.Generic;

public class HomeRoomSystem : MonoBehaviour
{
    [System.Serializable]
    public class RoomData
    {
        public string roomId;
        public string roomName;
        public RoomType roomType;
        public Vector3 bounds;
        public Vector3 position;
        public float ceilingHeight;
        public List<GameObject> windows;
        public List<GameObject> doors;
        public bool isLocked;
        public int cost;
        public List<string> requiredUpgrades;
    }
    
    public enum RoomType
    {
        Cockpit,
        Navigation,
        Observatory,
        EngineBay,
        CrewQuarters,
        CargoHold,
        CaptainStudy,
        Pantry,
        MedicalBay
    }
    
    [Header("Room Library")]
    public List<RoomData> allRooms = new List<RoomData>();
    public List<RoomData> unlockedRooms = new List<RoomData>();
    
    [Header("Room Templates")]
    public GameObject cockpitPrefab;
    public GameObject navigationPrefab;
    public GameObject observatoryPrefab;
    public GameObject engineBayPrefab;
    public GameObject crewQuartersPrefab;
    
    [Header("AR Detection")]
    public bool useARRoomDetection = true;
    public float minRoomSize = 2.0f;
    
    private RoomData currentRoom;
    private HouseUpgradeSystem upgradeSystem;
    
    void Start()
    {
        InitializeDefaultRooms();
        upgradeSystem = FindObjectOfType<HouseUpgradeSystem>();
    }
    
    void InitializeDefaultRooms()
    {
        // Room 1: Cockpit (always unlocked)
        allRooms.Add(new RoomData {
            roomId = "cockpit_01",
            roomName = "Main Cockpit",
            roomType = RoomType.Cockpit,
            position = Vector3.zero,
            cost = 0,
            isLocked = false
        });
        
        // Room 2: Navigation
        allRooms.Add(new RoomData {
            roomId = "nav_01",
            roomName = "Navigation",
            roomType = RoomType.Navigation,
            position = new Vector3(-8, 0, 0),
            cost = 1000,
            isLocked = true
        });
        
        currentRoom = allRooms[0];
        unlockedRooms.Add(currentRoom);
    }
    
    public void UnlockRoom(string roomId)
    {
        var room = allRooms.Find(r => r.roomId == roomId);
        if (room == null || !room.isLocked) return;
        
        room.isLocked = false;
        unlockedRooms.Add(room);
        Debug.Log("Unlocked: " + room.roomName);
    }
    
    public void SwitchRoom(string roomId)
    {
        var room = unlockedRooms.Find(r => r.roomId == roomId);
        if (room != null)
        {
            currentRoom = room;
        }
    }
    
    public RoomData GetCurrentRoom() => currentRoom;
    public List<RoomData> GetUnlockedRooms() => unlockedRooms;
}
