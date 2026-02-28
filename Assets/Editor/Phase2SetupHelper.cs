#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Phase2SetupHelper
{
    static Phase2SetupHelper()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            SetupRoomPrefabs();
        }
    }

    [MenuItem("Tools/FlyingHouse/Auto-Assign Room Prefabs")]
    static void SetupRoomPrefabs()
    {
        var homeSystem = GameObject.FindObjectOfType<HomeRoomSystem>();
        if (homeSystem == null) return;

        // Try to load prefabs from Resources/Prefabs/Rooms or Assets
        string[] roomNames = {
            "NavigationRoom", "ObservatoryRoom", "EngineBayRoom",
            "CaptainStudy", "CargoHold", "Pantry", "MedicalBay"
        };

        for (int i = 0; i < homeSystem.allRooms.Count; i++)
        {
            var room = homeSystem.allRooms[i];
            if (room.roomPrefab == null && i > 0) // Skip cockpit
            {
                string prefabName = roomNames[System.Math.Min(i-1, roomNames.Length-1)];
                room.roomPrefab = Resources.Load<GameObject>($"Prefabs/Rooms/{prefabName}");
                
                if (room.roomPrefab == null)
                {
                    // Try direct asset path
                    room.roomPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Prefabs/Rooms/{prefabName}.prefab");
                }
            }
        }

        Debug.Log("Room prefabs auto-assigned. Save scene to persist.");
    }
}
#endif
