#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// This Editor utility creates a minimal placeholder scene for FlyingHouseVR.
// It intentionally avoids adding project-specific runtime components so the scene
// can be generated even before importing Cesium or other packages.
public static class SceneBuilder
{
    [MenuItem("Tools/FlyingHouse/Build Placeholder Scene")]
    public static void BuildPlaceholderScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "FlyingHouse";

        // Create XR Origin placeholder
        var xrOrigin = new GameObject("XROrigin");
        var camGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        camGO.name = "PlayerCameraPlaceholder";
        camGO.transform.SetParent(xrOrigin.transform);
        camGO.transform.localPosition = new Vector3(0, 1.6f, 0);
        camGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        // Create Steering Wheel placeholder
        var wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        wheel.name = "SteeringWheel_Placeholder";
        wheel.transform.position = new Vector3(0.8f, 1.1f, 0.5f);
        wheel.transform.localScale = new Vector3(0.5f, 0.05f, 0.5f);

        // Create Lever placeholder
        var lever = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lever.name = "AltitudeLever_Placeholder";
        lever.transform.position = new Vector3(0.6f, 1.0f, 0.6f);
        lever.transform.localScale = new Vector3(0.1f, 0.4f, 0.1f);

        // Create Window Frame placeholder
        var window = GameObject.CreatePrimitive(PrimitiveType.Quad);
        window.name = "WindowFrame_Placeholder";
        window.transform.position = new Vector3(0f, 1.2f, -1.5f);
        window.transform.localScale = new Vector3(1.5f, 1.0f, 1f);

        // Create Cesium placeholder object
        var cesium = new GameObject("Cesium_Georeference_Placeholder");
        cesium.transform.position = Vector3.zero;

        // Create a simple room
        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.localScale = new Vector3(2f, 1f, 2f);

        var walls = new GameObject("Walls");
        var wall1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall1.name = "Wall_Back";
        wall1.transform.SetParent(walls.transform);
        wall1.transform.position = new Vector3(0f, 1.25f, -3f);
        wall1.transform.localScale = new Vector3(6f, 2.5f, 0.2f);

        // Save scene
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/FlyingHouse.unity");
        Debug.Log("Placeholder scene created: Assets/Scenes/FlyingHouse.unity");
    }
}
#endif
