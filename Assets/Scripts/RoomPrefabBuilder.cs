using UnityEngine;
using System.Collections;

public class RoomPrefabBuilder : MonoBehaviour
{
    [Header("Room Types")]
    public bool buildCockpit = true;
    public bool buildNavigation = true;
    public bool buildObservatory = true;
    public bool buildEngineBay = true;
    public bool buildCrewQuarters = true;
    
    [Header("Materials")]
    public Material floorMaterial;
    public Material wallMaterial;
    public Material ceilingMaterial;
    public Material glassMaterial;
    
    [Header("Sizes")]
    public float defaultWidth = 4f;
    public float defaultDepth = 5f;
    public float defaultHeight = 2.5f;
    public float wallThickness = 0.1f;
    
    void Start()
    {
        if (buildCockpit) BuildCockpit();
    }
    
    public GameObject BuildCockpit(Vector3? position = null)
    {
        Vector3 pos = position ?? Vector3.zero;
        
        GameObject cockpit = new GameObject("Room_Cockpit");
        cockpit.transform.position = pos;
        
        // Floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";
        floor.transform.SetParent(cockpit.transform);
        floor.transform.localScale = new Vector3(defaultWidth, wallThickness, defaultDepth);
        floor.transform.position = pos + Vector3.down * (wallThickness/2);
        
        // Walls (back, left, right)
        // Back wall
        GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backWall.name = "BackWall";
        backWall.transform.SetParent(cockpit.transform);
        backWall.transform.localScale = new Vector3(defaultWidth, defaultHeight, wallThickness);
        backWall.transform.position = pos + new Vector3(0, defaultHeight/2, -defaultDepth/2);
        
        // Left wall (partial for window)
        GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftWall.name = "LeftWall";
        leftWall.transform.SetParent(cockpit.transform);
        leftWall.transform.localScale = new Vector3(wallThickness, defaultHeight, defaultDepth * 0.7f);
        leftWall.transform.position = pos + new Vector3(-defaultWidth/2, defaultHeight/2, defaultDepth * 0.15f);
        
        // Right wall
        GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightWall.name = "RightWall";
        rightWall.transform.SetParent(cockpit.transform);
        rightWall.transform.localScale = new Vector3(wallThickness, defaultHeight, defaultDepth);
        rightWall.transform.position = pos + new Vector3(defaultWidth/2, defaultHeight/2, 0);
        
        // Ceiling
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.name = "Ceiling";
        ceiling.transform.SetParent(cockpit.transform);
        ceiling.transform.localScale = new Vector3(defaultWidth, wallThickness, defaultDepth);
        ceiling.transform.position = pos + new Vector3(0, defaultHeight + wallThickness/2, 0);
        
        // Window frame
        GameObject window = GameObject.CreatePrimitive(PrimitiveType.Cube);
        window.name = "WindowFrame";
        window.transform.SetParent(cockpit.transform);
        window.transform.localScale = new Vector3(wallThickness, 1.5f, 2f);
        window.transform.position = pos + new Vector3(-defaultWidth/2, defaultHeight/2, -defaultDepth * 0.25f);
        
        // Steering wheel pedestal
        GameObject pedestal = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pedestal.name = "WheelPedestal";
        pedestal.transform.SetParent(cockpit.transform);
        pedestal.transform.localScale = new Vector3(0.1f, 1f, 0.1f);
        pedestal.transform.position = pos + new Vector3(0.5f, 0.5f, 0.3f);
        
        // Steering wheel
        GameObject wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        wheel.name = "SteeringWheel";
        wheel.transform.SetParent(cockpit.transform);
        wheel.transform.localScale = new Vector3(0.5f, 0.05f, 0.5f);
        wheel.transform.position = pos + new Vector3(0.5f, 1.1f, 0.3f);
        wheel.transform.rotation = Quaternion.Euler(90, 0, 0);
        
        // Apply materials
        ApplyMaterials(cockpit);
        
        return cockpit;
    }
    
    void ApplyMaterials(GameObject room)
    {
        foreach (Transform child in room.transform)
        {
            Renderer rend = child.GetComponent<Renderer>();
            if (rend == null) continue;
            
            switch (child.name)
            {
                case "Floor":
                    if (floorMaterial != null) rend.material = floorMaterial;
                    break;
                case "Ceiling":
                    if (ceilingMaterial != null) rend.material = ceilingMaterial;
                    break;
                case "WindowFrame":
                case "Window":
                    if (glassMaterial != null) rend.material = glassMaterial;
                    break;
                default:
                    if (wallMaterial != null) rend.material = wallMaterial;
                    break;
            }
        }
    }
    
    // Additional room builders would be added here for other room types
}
