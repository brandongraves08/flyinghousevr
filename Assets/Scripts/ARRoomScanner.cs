using UnityEngine;
using System.Collections.Generic;

#if UNITY_ANDROID
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#endif

public class ARRoomScanner : MonoBehaviour
{
    [Header("AR Components")]
    public bool enableScanning = true;
    public bool scanOnStart = false;
    
    [Header("Scan Settings")]
    public float scanDuration = 30f;
    public float minWallHeight = 1.5f;
    public float maxWallHeight = 4f;
    public float minRoomArea = 4f;
    
    [Header("Visualization")]
    public Material wallPreviewMaterial;
    public Material floorPreviewMaterial;
    public bool showDebugVisualization = true;
    
    private List<Vector3> detectedWalls = new List<Vector3>();
    private Vector3 roomCenter;
    private float roomWidth;
    private float roomDepth;
    private float roomHeight;
    private bool isScanning = false;
    
    public static ARRoomScanner Instance { get; private set; }
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        if (scanOnStart)
        {
            StartScan();
        }
    }
    
    public void StartScan()
    {
        if (isScanning) return;
        
        Debug.Log("Starting room scan...");
        isScanning = true;
        
        #if UNITY_ANDROID
        StartCoroutine(ScanRoutine());
        #else
        // Desktop fallback - simulate scan
        SimulateScan();
        #endif
    }
    
    System.Collections.IEnumerator ScanRoutine()
    {
        float elapsed = 0;
        
        while (elapsed < scanDuration && isScanning)
        {
            elapsed += Time.deltaTime;
            
            // This would integrate with AR Foundation
            // For Meta Quest: Depth API
            // For ARKit/Android: ARPlaneManager
            
            yield return null;
        }
        
        FinalizeScan();
    }
    
    void FinalizeScan()
    {
        isScanning = false;
        
        // Calculate room dimensions
        if (detectedWalls.Count >= 3)
        {
            CalculateRoomBounds();
            SaveRoomData();
        }
        
        Debug.Log($"Scan complete! Room: {roomWidth:F1}m x {roomDepth:F1}m x {roomHeight:F1}m");
    }
    
    void CalculateRoomBounds()
    {
        // Bounding box from wall points
        Vector3 min = detectedWalls[0];
        Vector3 max = detectedWalls[0];
        
        foreach (var wall in detectedWalls)
        {
            min = Vector3.Min(min, wall);
            max = Vector3.Max(max, wall);
        }
        
        roomCenter = (min + max) / 2f;
        roomWidth = max.x - min.x;
        roomDepth = max.z - min.z;
        roomHeight = max.y - min.y;
    }
    
    void SaveRoomData()
    {
        // Save to persistent storage
        PlayerPrefs.SetFloat("Room_Width", roomWidth);
        PlayerPrefs.SetFloat("Room_Depth", roomDepth);
        PlayerPrefs.SetFloat("Room_Height", roomHeight);
        PlayerPrefs.SetFloat("Room_CenterX", roomCenter.x);
        PlayerPrefs.SetFloat("Room_CenterY", roomCenter.y);
        PlayerPrefs.SetFloat("Room_CenterZ", roomCenter.z);
        PlayerPrefs.Save();
    }
    
    void SimulateScan()
    {
        // Fallback for desktop testing
        roomWidth = 4f;
        roomDepth = 5f;
        roomHeight = 2.5f;
        roomCenter = Vector3.zero;
        
        Debug.Log("Simulated room scan (desktop mode)");
    }
    
    public Vector3 GetRoomCenter() => roomCenter;
    public Vector3 GetRoomSize() => new Vector3(roomWidth, roomHeight, roomDepth);
    public bool HasScannedRoom() => PlayerPrefs.HasKey("Room_Width");
    public bool IsScanning() => isScanning;
    
    void OnDrawGizmos()
    {
        if (!showDebugVisualization || isScanning) return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(roomCenter, new Vector3(roomWidth, roomHeight, roomDepth));
        
        Gizmos.color = Color.yellow;
        foreach (var wall in detectedWalls)
        {
            Gizmos.DrawSphere(wall, 0.1f);
        }
    }
}
