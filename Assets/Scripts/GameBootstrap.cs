using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrap : MonoBehaviour
{
    [Header("Setup")]
    public bool autoInitializeManagers = true;
    public bool skipMenusInEditor = true;
    
    [Header("Fallback Camera")]
    public Camera playModeCamera;
    
    void Awake()
    {
        if (autoInitializeManagers)
        {
            InitializeCoreSystems();
        }
        
        SetupCamera();
        VerifyDependencies();
    }
    
    void InitializeCoreSystems()
    {
        // Ensure managers exist
        EnsureManagerExists<FlyingHouseManager>("FlyingHouseManager");
        EnsureManagerExists<SaveSystem>("SaveSystem");
        EnsureManagerExists<HUDManager>("HUDManager");
        EnsureManagerExists<ErrorHandler>("ErrorHandler");
        
        #if UNITY_EDITOR
        if (skipMenusInEditor && SceneManager.GetActiveScene().name != "FlyingHouse")
        {
            // We're in menu, that's fine
        }
        #endif
    }
    
    void EnsureManagerExists<T>(string name) where T : MonoBehaviour
    {
        T existing = FindObjectOfType<T>();
        if (existing == null)
        {
            GameObject go = new GameObject(name);
            go.AddComponent<T>();
            Debug.Log($"Created missing manager: {name}");
        }
    }
    
    void SetupCamera()
    {
        if (Camera.main == null && playModeCamera != null)
        {
            playModeCamera.gameObject.SetActive(true);
            Debug.Log("Activated fallback camera");
        }
    }
    
    void VerifyDependencies()
    {
        // Check for critical components
        var flightController = FindObjectOfType<FlightController>();
        if (flightController == null)
        {
            Debug.LogError("No FlightController found! Game cannot run.");
        }
        
        var desktopInput = FindObjectOfType<DesktopInputManager>();
        if (desktopInput == null && Application.platform != RuntimePlatform.Android)
        {
            Debug.LogWarning("No DesktopInputManager - VR only mode");
        }
    }
}
