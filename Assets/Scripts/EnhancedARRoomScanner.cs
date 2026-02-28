using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnhancedARRoomScanner : MonoBehaviour
{
    public static EnhancedARRoomScanner Instance { get; private set; }
    
    [Header("Scanning")]
    public bool autoScanOnStart = false;
    public float scanDuration = 30f;
    public float scanCooldown = 2f;
    
    [Header("Detection")]
    public float wallDetectionInterval = 0.5f;
    public float minWallHeight = 1.2f;
    public float maxWallHeight = 4f;
    public float wallThickness = 0.1f;
    
    [Header("Room Bounds")]
    public Vector3 roomCenter;
    public Vector3 roomSize;
    public bool hasValidRoom = false;
    
    [Header("Visualization")]
    public bool showRoomBounds = true;
    public Material roomBoundsMaterial;
    public GameObject roomPreviewPrefab;
    
    [Header("Events")]
    public UnityEngine.Events.UnityEvent OnScanComplete;
    public UnityEngine.Events.UnityEvent<Vector3, Vector3> OnRoomDetected;
    
    private List<Vector3> wallPoints = new List<Vector3>();
    private bool isScanning = false;
    private Coroutine scanCoroutine;
    private GameObject previewInstance;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        LoadSavedRoom();
        
        if (autoScanOnStart && !hasValidRoom)
        {
            StartScan();
        }
    }
    
    public void StartScan()
    {
        if (isScanning) return;
        
        isScanning = true;
        wallPoints.Clear();
        
        Debug.Log("Starting room scan... Walk around your space.");
        
        scanCoroutine = StartCoroutine(ScanRoutine());
    }
    
    public void StopScan()
    {
        if (!isScanning) return;
        
        isScanning = false;
        
        if (scanCoroutine != null)
            StopCoroutine(scanCoroutine);
        
        FinalizeScan();
    }
    
    IEnumerator ScanRoutine()
    {
        float elapsed = 0f;
        
        while (elapsed < scanDuration && isScanning)
        {
            elapsed += Time.deltaTime;
            
            // Simulated wall detection
            DetectWalls();
            
            yield return new WaitForSeconds(wallDetectionInterval);
        }
        
        FinalizeScan();
    }
    
    void DetectWalls()
    {
        // In real AR, this would use depth cameras
        // For now, simulate detection from player position
        
        var camera = Camera.main;
        if (camera == null) return;
        
        // Cast rays in front of camera
        for (int i = -30; i <= 30; i += 15)
        {
            for (int j = -20; j <= 20; j += 20)
            {
                Vector3 direction = Quaternion.Euler(j, i, 0) * camera.transform.forward;
                
                if (Physics.Raycast(camera.transform.position, direction, out RaycastHit hit, 10f))
                {
                    if (!wallPoints.Contains(hit.point))
                    {
                        wallPoints.Add(hit.point);
                    }
                }
            }
        }
    }
    
    void FinalizeScan()
    {
        isScanning = false;
        
        if (wallPoints.Count < 4)
        {
            Debug.Log("Not enough data. Using defaults.");
            UseDefaultRoom();
            return;
        }
        
        CalculateRoomBounds();
        SaveRoomData();
        CreateRoomPreview();
        
        hasValidRoom = true;
        
        Debug.Log($"Room detected: {roomSize.x:F1}m x {roomSize.z:F1}m x {roomSize.y:F1}m");
        
        OnScanComplete?.Invoke();
        OnRoomDetected?.Invoke(roomCenter, roomSize);
    }
    
    void CalculateRoomBounds()
    {
        Vector3 min = wallPoints[0];
        Vector3 max = wallPoints[0];
        
        foreach (var point in wallPoints)
        {
            min = Vector3.Min(min, point);
            max = Vector3.Max(max, point);
        }
        
        roomCenter = (min + max) / 2f;
        roomSize = max - min;
        
        // Clamp to reasonable values
        roomSize.x = Mathf.Clamp(roomSize.x, 2f, 20f);
        roomSize.y = Mathf.Clamp(roomSize.y, 2f, 5f);
        roomSize.z = Mathf.Clamp(roomSize.z, 2f, 20f);
    }
    
    void UseDefaultRoom()
    {
        roomCenter = Vector3.zero;
        roomSize = new Vector3(4f, 2.5f, 5f);
        hasValidRoom = false;
        
        OnScanComplete?.Invoke();
    }
    
    void SaveRoomData()
    {
        PlayerPrefs.SetFloat("Room_CenterX", roomCenter.x);
        PlayerPrefs.SetFloat("Room_CenterY", roomCenter.y);
        PlayerPrefs.SetFloat("Room_CenterZ", roomCenter.z);
        PlayerPrefs.SetFloat("Room_SizeX", roomSize.x);
        PlayerPrefs.SetFloat("Room_SizeY", roomSize.y);
        PlayerPrefs.SetFloat("Room_SizeZ", roomSize.z);
        PlayerPrefs.SetInt("Room_Valid", hasValidRoom ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    void LoadSavedRoom()
    {
        if (PlayerPrefs.HasKey("Room_SizeX"))
        {
            roomCenter = new Vector3(
                PlayerPrefs.GetFloat("Room_CenterX"),
                PlayerPrefs.GetFloat("Room_CenterY"),
                PlayerPrefs.GetFloat("Room_CenterZ")
            );
            
            roomSize = new Vector3(
                PlayerPrefs.GetFloat("Room_SizeX"),
                PlayerPrefs.GetFloat("Room_SizeY"),
                PlayerPrefs.GetFloat("Room_SizeZ")
            );
            
            hasValidRoom = PlayerPrefs.GetInt("Room_Valid", 0) == 1;
            
            if (hasValidRoom)
            {
                CreateRoomPreview();
                Debug.Log("Loaded saved room data");
            }
        }
    }
    
    void CreateRoomPreview()
    {
        if (roomPreviewPrefab != null)
        {
            if (previewInstance != null)
                Destroy(previewInstance);
            
            previewInstance = Instantiate(roomPreviewPrefab, roomCenter, Quaternion.identity);
            previewInstance.transform.localScale = roomSize;
        }
    }
    
    public void ClearRoomData()
    {
        PlayerPrefs.DeleteKey("Room_CenterX");
        PlayerPrefs.DeleteKey("Room_CenterY");
        PlayerPrefs.DeleteKey("Room_CenterZ");
        PlayerPrefs.DeleteKey("Room_SizeX");
        PlayerPrefs.DeleteKey("Room_SizeY");
        PlayerPrefs.DeleteKey("Room_SizeZ");
        PlayerPrefs.DeleteKey("Room_Valid");
        PlayerPrefs.Save();
        
        hasValidRoom = false;
        roomCenter = Vector3.zero;
        roomSize = Vector3.zero;
        
        if (previewInstance != null)
            Destroy(previewInstance);
        
        Debug.Log("Room data cleared");
    }
    
    public Vector3 GetRoomPosition(string roomId)
    {
        // Position rooms relative to scanned room
        // This is simplified - real implementation would use room graph
        var rooms = FindObjectOfType<HomeRoomSystem>()?.allRooms;
        if (rooms == null) return roomCenter;
        
        var room = rooms.Find(r => r.roomId == roomId);
        if (room == null) return roomCenter;
        
        return roomCenter + room.position;
    }
    
    void OnDrawGizmos()
    {
        if (!showRoomBounds) return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(roomCenter, roomSize);
        
        Gizmos.color = Color.yellow;
        foreach (var point in wallPoints)
        {
            Gizmos.DrawSphere(point, 0.05f);
        }
    }
}
