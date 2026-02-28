#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Builds a usable placeholder scene and wires core managers so the project is ready to open.
public static class SceneBuilder
{
    [MenuItem("Tools/FlyingHouse/Build Full Placeholder Scene")]
    public static void BuildFullScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Create GameManager root
        var gm = new GameObject("GameManager");
        var fm = gm.AddComponent<GameObject>();

        // FlyingHouseManager
        var fh = new GameObject("FlyingHouseManager");
        var fhComp = fh.AddComponent<FlyingHouseManager>();
        fh.transform.SetParent(null);

        // FlightController
        var fcGO = new GameObject("FlightController");
        var fc = fcGO.AddComponent<FlightController>();
        fcGO.transform.SetParent(fh.transform);
        fhComp.flightController = fc;

        // CalibrationManager
        var calibGO = new GameObject("CalibrationManager");
        var calib = calibGO.AddComponent<CalibrationManager>();
        calibGO.transform.SetParent(fh.transform);
        fhComp.calibrationManager = calib;

        // WaypointManager
        var wpgo = new GameObject("WaypointManager");
        wpgo.AddComponent<WaypointManager>();

        // WeatherManager
        var wm = new GameObject("WeatherManager");
        wm.AddComponent<WeatherManager>();

        // ScenarioManager
        var sm = new GameObject("ScenarioManager");
        var smComp = sm.AddComponent<ScenarioManager>();
        smComp.flightController = fc;

        // HUD Manager
        var hudGO = new GameObject("HUDManager");
        var hud = hudGO.AddComponent<HUDManager>();

        // Desktop Input Manager
        var diGO = new GameObject("DesktopInputManager");
        var dim = diGO.AddComponent<DesktopInputManager>();
        dim.flightController = fc;
        dim.steeringWheel = null;
        dim.altitudeLever = null;

        // Add managers to root
        fhComp.windAudio = null;
        fhComp.ambienceAudio = null;

        // Create simple room
        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.localScale = new Vector3(4f,1f,4f);

        var backWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backWall.name = "BackWall";
        backWall.transform.position = new Vector3(0f,1.25f,-6f);
        backWall.transform.localScale = new Vector3(12f,2.5f,0.2f);

        // Steering wheel placeholder
        var wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        wheel.name = "SteeringWheel";
        wheel.transform.position = new Vector3(0.8f,1.1f,0.5f);
        wheel.transform.localScale = new Vector3(0.5f,0.05f,0.5f);
        var sw = wheel.AddComponent<SteeringWheel>();
        fhComp.flightController = fc;

        // Altitude lever placeholder
        var lever = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lever.name = "AltitudeLever";
        lever.transform.position = new Vector3(0.6f,1.0f,0.6f);
        lever.transform.localScale = new Vector3(0.1f,0.4f,0.1f);
        var al = lever.AddComponent<AltitudeLever>();

        // Window placeholder
        var window = GameObject.CreatePrimitive(PrimitiveType.Quad);
        window.name = "WindowFrame";
        window.transform.position = new Vector3(0f,1.2f,-1.5f);
        window.transform.localScale = new Vector3(1.5f,1.0f,1f);
        var wp = window.AddComponent<WindowPortal>();

        // Balcony placeholder
        var balcony = new GameObject("BalconyManager");
        balcony.AddComponent<BalconyManager>();

        // AudioManager
        var audio = new GameObject("AudioManager");
        audio.AddComponent<AudioManager>();

        // SaveSystem
        var save = new GameObject("SaveSystem");
        save.AddComponent<SaveSystem>();

        // UI Manager
        var ui = new GameObject("UIManager");
        ui.AddComponent<UIManager>();

        // Link some references
        dim.playerCamera = Camera.main != null ? Camera.main.transform : null;
        fhComp.flightController = fc;
        fhComp.calibrationManager = calib;

        // Save the scene
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/Scenes/FlyingHouse.unity");

        Debug.Log("Built full placeholder scene: Assets/Scenes/FlyingHouse.unity");
    }
}
#endif
