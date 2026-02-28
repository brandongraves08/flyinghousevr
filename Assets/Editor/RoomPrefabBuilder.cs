#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class RoomPrefabBuilder : MonoBehaviour
{
    [MenuItem("Tools/FlyingHouse/Build Room Prefabs")]
    public static void BuildRoomPrefabs()
    {
        BuildNavigationRoom();
        BuildObservatoryRoom();
        BuildEngineBay();
        BuildCaptainStudy();
        BuildCargoHold();
        BuildPantry();
        BuildMedicalBay();
        AssetDatabase.SaveAssets();
        Debug.Log("All room prefabs built!");
    }

    static void BuildNavigationRoom()
    {
        GameObject room = new GameObject("NavigationRoom");
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor"; floor.transform.SetParent(room.transform);
        floor.transform.localScale = new Vector3(8, 0.2f, 8); floor.transform.position = new Vector3(0, -0.1f, 0);
        GameObject walls = new GameObject("Walls"); walls.transform.SetParent(room.transform);
        GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backWall.name = "BackWall"; backWall.transform.SetParent(walls.transform);
        backWall.transform.localScale = new Vector3(8, 3, 0.2f); backWall.transform.position = new Vector3(0, 1.5f, -4);
        GameObject window = GameObject.CreatePrimitive(PrimitiveType.Quad);
        window.name = "Window"; window.transform.SetParent(walls.transform);
        window.transform.localScale = new Vector3(3, 1.5f, 1); window.transform.position = new Vector3(0, 1.5f, -3.9f);
        window.AddComponent<WindowPortal>();
        GameObject console = GameObject.CreatePrimitive(PrimitiveType.Cube);
        console.name = "NavConsole"; console.transform.SetParent(room.transform);
        console.transform.localScale = new Vector3(2, 1, 1); console.transform.position = new Vector3(0, 0.5f, -2);
        GameObject table = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        table.name = "MapTable"; table.transform.SetParent(room.transform);
        table.transform.localScale = new Vector3(1.5f, 0.8f, 1.5f); table.transform.position = new Vector3(2, 0.4f, 0);
        GameObject connector = new GameObject("Connector_Cockpit"); connector.transform.SetParent(room.transform);
        connector.transform.position = new Vector3(4, 0, 0);
        var rc = connector.AddComponent<RoomConnector>(); rc.connectedRoomId = "room_cockpit"; rc.connectedRoomName = "Captain's Cockpit";
        string path = "Assets/Prefabs/Rooms/NavigationRoom.prefab"; System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
        PrefabUtility.SaveAsPrefabAsset(room, path); DestroyImmediate(room);
    }

    static void BuildObservatoryRoom()
    {
        GameObject room = new GameObject("ObservatoryRoom");
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        floor.name = "Floor"; floor.transform.SetParent(room.transform);
        floor.transform.localScale = new Vector3(5, 0.2f, 5); floor.transform.position = new Vector3(0, -0.1f, 0);
        GameObject dome = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        dome.name = "Dome"; dome.transform.SetParent(room.transform);
        dome.transform.localScale = new Vector3(6, 3, 6); dome.transform.position = new Vector3(0, 1.5f, 0);
        var renderer = dome.GetComponent<Renderer>(); if (renderer != null) { renderer.material = new Material(Shader.Find("Transparent/Specular")); renderer.material.color = new Color(0.8f, 0.9f, 1f, 0.3f); }
        GameObject telescope = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        telescope.name = "Telescope"; telescope.transform.SetParent(room.transform);
        telescope.transform.localScale = new Vector3(0.3f, 1.5f, 0.3f); telescope.transform.position = new Vector3(0, 0.75f, 0); telescope.transform.rotation = Quaternion.Euler(-30, 0, 0);
        GameObject connector = new GameObject("Connector_Cockpit"); connector.transform.SetParent(room.transform);
        connector.transform.position = new Vector3(0, 0, 3); var rc = connector.AddComponent<RoomConnector>(); rc.connectedRoomId = "room_cockpit"; rc.connectedRoomName = "Captain's Cockpit";
        string path = "Assets/Prefabs/Rooms/ObservatoryRoom.prefab"; System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path)); PrefabUtility.SaveAsPrefabAsset(room, path); DestroyImmediate(room);
    }

    static void BuildEngineBay()
    {
        GameObject room = new GameObject("EngineBayRoom");
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor"; floor.transform.SetParent(room.transform);
        floor.transform.localScale = new Vector3(10, 0.2f, 6); floor.transform.position = new Vector3(0, -0.1f, 0);
        for (int i = 0; i < 3; i++) { GameObject engine = GameObject.CreatePrimitive(PrimitiveType.Cylinder); engine.name = $"Engine_{i}"; engine.transform.SetParent(room.transform); engine.transform.localScale = new Vector3(1, 2, 1); engine.transform.position = new Vector3((i - 1) * 3, 1, -2); var rend = engine.GetComponent<Renderer>(); if (rend != null) { rend.material = new Material(Shader.Find("Standard")); rend.material.EnableKeyword("_EMISSION"); rend.material.SetColor("_EmissionColor", Color.cyan * 0.5f); } }
        GameObject panel = GameObject.CreatePrimitive(PrimitiveType.Cube); panel.name = "ControlPanel"; panel.transform.SetParent(room.transform); panel.transform.localScale = new Vector3(3, 1.2f, 0.5f); panel.transform.position = new Vector3(0, 0.6f, 2.5f);
        GameObject connector = new GameObject("Connector_Cockpit"); connector.transform.SetParent(room.transform); connector.transform.position = new Vector3(6, 0, 0); var rc = connector.AddComponent<RoomConnector>(); rc.connectedRoomId = "room_cockpit"; rc.connectedRoomName = "Captain's Cockpit";
        string path = "Assets/Prefabs/Rooms/EngineBayRoom.prefab"; System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path)); PrefabUtility.SaveAsPrefabAsset(room, path); DestroyImmediate(room);
    }

    static void BuildCaptainStudy()
    {
        GameObject room = new GameObject("CaptainStudyRoom");
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube); floor.name = "Floor"; floor.transform.SetParent(room.transform); floor.transform.localScale = new Vector3(6, 0.2f, 6); floor.transform.position = new Vector3(0, -0.1f, 0);
        GameObject desk = GameObject.CreatePrimitive(PrimitiveType.Cube); desk.name = "CaptainDesk"; desk.transform.SetParent(room.transform); desk.transform.localScale = new Vector3(2, 0.8f, 1); desk.transform.position = new Vector3(0, 0.4f, -2);
        GameObject chair = GameObject.CreatePrimitive(PrimitiveType.Cube); chair.name = "CaptainChair"; chair.transform.SetParent(room.transform); chair.transform.localScale = new Vector3(0.6f, 0.5f, 0.6f); chair.transform.position = new Vector3(0, 0.4f, -1.2f);
        GameObject bookshelf = GameObject.CreatePrimitive(PrimitiveType.Cube); bookshelf.name = "Bookshelf"; bookshelf.transform.SetParent(room.transform); bookshelf.transform.localScale = new Vector3(1.2f, 2f, 0.4f); bookshelf.transform.position = new Vector3(-2f, 1f, -2f);
        GameObject globe = GameObject.CreatePrimitive(PrimitiveType.Sphere); globe.name = "Globe"; globe.transform.SetParent(room.transform); globe.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f); globe.transform.position = new Vector3(1.5f, 0.8f, -1.5f);
        GameObject connector = new GameObject("Connector_Cockpit"); connector.transform.SetParent(room.transform); connector.transform.position = new Vector3(3, 0, 0); var rc = connector.AddComponent<RoomConnector>(); rc.connectedRoomId = "room_cockpit"; rc.connectedRoomName = "Captain's Cockpit";
        string path = "Assets/Prefabs/Rooms/CaptainStudy.prefab"; System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path)); PrefabUtility.SaveAsPrefabAsset(room, path); DestroyImmediate(room);
    }

    static void BuildCargoHold()
    {
        GameObject room = new GameObject("CargoHoldRoom");
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube); floor.name = "Floor"; floor.transform.SetParent(room.transform); floor.transform.localScale = new Vector3(10, 0.2f, 10); floor.transform.position = new Vector3(0, -0.1f, 0);
        for (int i = 0; i < 6; i++) { GameObject crate = GameObject.CreatePrimitive(PrimitiveType.Cube); crate.name = $"Crate_{i}"; crate.transform.SetParent(room.transform); crate.transform.localScale = new Vector3(1f, 1f, 1f); crate.transform.position = new Vector3((i%3)-1.0f, 0.5f, (i/3)-1.0f); }
        GameObject ramp = GameObject.CreatePrimitive(PrimitiveType.Cube); ramp.name = "Ramp"; ramp.transform.SetParent(room.transform); ramp.transform.localScale = new Vector3(3, 0.2f, 1.5f); ramp.transform.position = new Vector3(0, 0.1f, 4f);
        GameObject connector = new GameObject("Connector_Cockpit"); connector.transform.SetParent(room.transform); connector.transform.position = new Vector3(-5, 0, 0); var rc = connector.AddComponent<RoomConnector>(); rc.connectedRoomId = "room_cockpit"; rc.connectedRoomName = "Captain's Cockpit";
        string path = "Assets/Prefabs/Rooms/CargoHold.prefab"; System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path)); PrefabUtility.SaveAsPrefabAsset(room, path); DestroyImmediate(room);
    }

    static void BuildPantry()
    {
        GameObject room = new GameObject("PantryRoom");
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube); floor.name = "Floor"; floor.transform.SetParent(room.transform); floor.transform.localScale = new Vector3(4, 0.2f, 4); floor.transform.position = new Vector3(0, -0.1f, 0);
        GameObject shelf = GameObject.CreatePrimitive(PrimitiveType.Cube); shelf.name = "Shelf"; shelf.transform.SetParent(room.transform); shelf.transform.localScale = new Vector3(2, 2, 0.5f); shelf.transform.position = new Vector3(-1f, 1f, 0);
        GameObject fridge = GameObject.CreatePrimitive(PrimitiveType.Cube); fridge.name = "Fridge"; fridge.transform.SetParent(room.transform); fridge.transform.localScale = new Vector3(0.8f, 1.6f, 0.8f); fridge.transform.position = new Vector3(1f, 0.8f, 0.5f);
        GameObject table = GameObject.CreatePrimitive(PrimitiveType.Cylinder); table.name = "PrepTable"; table.transform.SetParent(room.transform); table.transform.localScale = new Vector3(1f, 0.6f, 1f); table.transform.position = new Vector3(0, 0.4f, -1f);
        GameObject connector = new GameObject("Connector_Cockpit"); connector.transform.SetParent(room.transform); connector.transform.position = new Vector3(2.2f, 0, 0); var rc = connector.AddComponent<RoomConnector>(); rc.connectedRoomId = "room_cockpit"; rc.connectedRoomName = "Captain's Cockpit";
        string path = "Assets/Prefabs/Rooms/Pantry.prefab"; System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path)); PrefabUtility.SaveAsPrefabAsset(room, path); DestroyImmediate(room);
    }

    static void BuildMedicalBay()
    {
        GameObject room = new GameObject("MedicalBayRoom");
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube); floor.name = "Floor"; floor.transform.SetParent(room.transform); floor.transform.localScale = new Vector3(5, 0.2f, 6); floor.transform.position = new Vector3(0, -0.1f, 0);
        GameObject bed = GameObject.CreatePrimitive(PrimitiveType.Cube); bed.name = "MedBed"; bed.transform.SetParent(room.transform); bed.transform.localScale = new Vector3(1.2f, 0.4f, 2f); bed.transform.position = new Vector3(0, 0.4f, -1f);
        GameObject medCab = GameObject.CreatePrimitive(PrimitiveType.Cube); medCab.name = "MedCabinet"; medCab.transform.SetParent(room.transform); medCab.transform.localScale = new Vector3(0.8f, 1.6f, 0.6f); medCab.transform.position = new Vector3(-1.5f, 0.8f, 1f);
        GameObject monitor = GameObject.CreatePrimitive(PrimitiveType.Cube); monitor.name = "VitalsMonitor"; monitor.transform.SetParent(room.transform); monitor.transform.localScale = new Vector3(0.4f, 0.4f, 0.05f); monitor.transform.position = new Vector3(0.6f, 1f, -0.6f);
        GameObject connector = new GameObject("Connector_Cockpit"); connector.transform.SetParent(room.transform); connector.transform.position = new Vector3(-3f, 0, 0); var rc = connector.AddComponent<RoomConnector>(); rc.connectedRoomId = "room_cockpit"; rc.connectedRoomName = "Captain's Cockpit";
        string path = "Assets/Prefabs/Rooms/MedicalBay.prefab"; System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path)); PrefabUtility.SaveAsPrefabAsset(room, path); DestroyImmediate(room);
    }
}
#endif
