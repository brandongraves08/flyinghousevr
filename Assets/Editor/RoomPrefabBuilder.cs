#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class RoomPrefabBuilder : MonoBehaviour
{
    [MenuItem("Tools/FlyingHouse/Build Room Prefabs")]
    public static void BuildRoomPrefabs()
    {
        // Create Navigation Room
        BuildNavigationRoom();
        
        // Create Observatory Room
        BuildObservatoryRoom();
        
        // Create Engine Bay
        BuildEngineBay();
        
        AssetDatabase.SaveAssets();
        Debug.Log("Room prefabs built!");
    }
    
    static void BuildNavigationRoom()
    {
        GameObject room = new GameObject("NavigationRoom");
        
        // Floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";
        floor.transform.SetParent(room.transform);
        floor.transform.localScale = new Vector3(8, 0.2f, 8);
        floor.transform.position = new Vector3(0, -0.1f, 0);
        
        // Walls
        GameObject walls = new GameObject("Walls");
        walls.transform.SetParent(room.transform);
        
        // Back wall with window
        GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backWall.name = "BackWall";
        backWall.transform.SetParent(walls.transform);
        backWall.transform.localScale = new Vector3(8, 3, 0.2f);
        backWall.transform.position = new Vector3(0, 1.5f, -4);
        
        // Window
        GameObject window = GameObject.CreatePrimitive(PrimitiveType.Quad);
        window.name = "Window";
        window.transform.SetParent(walls.transform);
        window.transform.localScale = new Vector3(3, 1.5f, 1);
        window.transform.position = new Vector3(0, 1.5f, -3.9f);
        var wp = window.AddComponent<WindowPortal>();
        
        // Console
        GameObject console = GameObject.CreatePrimitive(PrimitiveType.Cube);
        console.name = "NavConsole";
        console.transform.SetParent(room.transform);
        console.transform.localScale = new Vector3(2, 1, 1);
        console.transform.position = new Vector3(0, 0.5f, -2);
        
        // Map table
        GameObject table = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        table.name = "MapTable";
        table.transform.SetParent(room.transform);
        table.transform.localScale = new Vector3(1.5f, 0.8f, 1.5f);
        table.transform.position = new Vector3(2, 0.4f, 0);
        
        // Connector to Cockpit
        GameObject connector = new GameObject("Connector_Cockpit");
        connector.transform.SetParent(room.transform);
        connector.transform.position = new Vector3(4, 0, 0);
        var rc = connector.AddComponent<RoomConnector>();
        rc.connectedRoomId = "room_cockpit";
        rc.connectedRoomName = "Captain's Cockpit";
        
        // Save prefab
        string path = "Assets/Prefabs/Rooms/NavigationRoom.prefab";
        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
        PrefabUtility.SaveAsPrefabAsset(room, path);
        DestroyImmediate(room);
    }
    
    static void BuildObservatoryRoom()
    {
        GameObject room = new GameObject("ObservatoryRoom");
        
        // Circular floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        floor.name = "Floor";
        floor.transform.SetParent(room.transform);
        floor.transform.localScale = new Vector3(5, 0.2f, 5);
        floor.transform.position = new Vector3(0, -0.1f, 0);
        
        // Glass dome structure (simplified)
        GameObject dome = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        dome.name = "Dome";
        dome.transform.SetParent(room.transform);
        dome.transform.localScale = new Vector3(6, 3, 6);
        dome.transform.position = new Vector3(0, 1.5f, 0);
        
        var renderer = dome.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Transparent/Specular"));
            renderer.material.color = new Color(0.8f, 0.9f, 1f, 0.3f);
        }
        
        // Telescope
        GameObject telescope = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        telescope.name = "Telescope";
        telescope.transform.SetParent(room.transform);
        telescope.transform.localScale = new Vector3(0.3f, 1.5f, 0.3f);
        telescope.transform.position = new Vector3(0, 0.75f, 0);
        telescope.transform.rotation = Quaternion.Euler(-30, 0, 0);
        
        // Connector
        GameObject connector = new GameObject("Connector_Cockpit");
        connector.transform.SetParent(room.transform);
        connector.transform.position = new Vector3(0, 0, 3);
        var rc = connector.AddComponent<RoomConnector>();
        rc.connectedRoomId = "room_cockpit";
        rc.connectedRoomName = "Captain's Cockpit";
        
        string path = "Assets/Prefabs/Rooms/ObservatoryRoom.prefab";
        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
        PrefabUtility.SaveAsPrefabAsset(room, path);
        DestroyImmediate(room);
    }
    
    static void BuildEngineBay()
    {
        GameObject room = new GameObject("EngineBayRoom");
        
        // Industrial floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";
        floor.transform.SetParent(room.transform);
        floor.transform.localScale = new Vector3(10, 0.2f, 6);
        floor.transform.position = new Vector3(0, -0.1f, 0);
        
        // Engine cores
        for (int i = 0; i < 3; i++)
        {
            GameObject engine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            engine.name = $"Engine_{i}";
            engine.transform.SetParent(room.transform);
            engine.transform.localScale = new Vector3(1, 2, 1);
            engine.transform.position = new Vector3((i - 1) * 3, 1, -2);
            
            // Add glow effect (emission)
            var rend = engine.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = new Material(Shader.Find("Standard"));
                rend.material.EnableKeyword("_EMISSION");
                rend.material.SetColor("_EmissionColor", Color.cyan * 0.5f);
            }
        }
        
        // Control panel
        GameObject panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        panel.name = "ControlPanel";
        panel.transform.SetParent(room.transform);
        panel.transform.localScale = new Vector3(3, 1.2f, 0.5f);
        panel.transform.position = new Vector3(0, 0.6f, 2.5f);
        
        // Connector
        GameObject connector = new GameObject("Connector_Cockpit");
        connector.transform.SetParent(room.transform);
        connector.transform.position = new Vector3(6, 0, 0);
        var rc = connector.AddComponent<RoomConnector>();
        rc.connectedRoomId = "room_cockpit";
        rc.connectedRoomName = "Captain's Cockpit";
        
        string path = "Assets/Prefabs/Rooms/EngineBayRoom.prefab";
        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
        PrefabUtility.SaveAsPrefabAsset(room, path);
        DestroyImmediate(room);
    }
}
#endif
