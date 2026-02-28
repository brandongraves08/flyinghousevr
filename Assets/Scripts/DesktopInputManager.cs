using UnityEngine;

public class DesktopInputManager : MonoBehaviour
{
    [Header("Input Mode")]
    public bool useDesktopInput = true;
    public bool forceDesktopInEditor = true;
    
    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;
    public Transform playerCamera;
    
    [Header("Flight Controls")]
    public float keyboardSteeringSpeed = 1f;
    public float keyboardThrottleSpeed = 0.5f;
    public KeyCode brakeKey = KeyCode.Space;
    public KeyCode pauseKey = KeyCode.Escape;
    
    [Header("References")]
    public FlightController flightController;
    public SteeringWheel steeringWheel;
    public AltitudeLever altitudeLever;
    public FlyingHouseManager houseManager;
    
    private float rotationX = 0f;
    private float rotationY = 0f;
    private float currentSteering = 0f;
    private float currentThrottle = 0f;
    private bool cursorLocked = false;
    
    void Start()
    {
        // Auto-detect VR
        if (!forceDesktopInEditor && UnityEngine.XR.XRSettings.enabled)
        {
            useDesktopInput = false;
            enabled = false;
            return;
        }
        
        // Find references
        if (flightController == null) flightController = FindObjectOfType<FlightController>();
        if (houseManager == null) houseManager = FindObjectOfType<FlyingHouseManager>();
        
        if (playerCamera == null)
        {
            playerCamera = Camera.main?.transform;
        }
        
        // Start in desktop mode
        if (useDesktopInput)
        {
            EnableDesktopMode();
        }
    }
    
    void Update()
    {
        if (!useDesktopInput) return;
        
        HandleMouseLook();
        HandleFlightInput();
        HandleUIInput();
    }
    
    void HandleMouseLook()
    {
        // Lock/unlock cursor
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.L))
        {
            ToggleCursorLock();
        }
        
        if (!cursorLocked) return;
        
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxLookAngle, maxLookAngle);
        
        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }
    
    void HandleFlightInput()
    {
        // Steering (A/D or Left/Right arrows)
        float steerInput = Input.GetAxis("Horizontal");
        
        // Smooth steering
        currentSteering = Mathf.MoveTowards(currentSteering, steerInput, keyboardSteeringSpeed * Time.deltaTime);
        
        // Apply to flight controller or steering wheel
        if (flightController != null)
        {
            flightController.SetManualSteering(currentSteering);
        }
        
        if (steeringWheel != null)
        {
            steeringWheel.SetWheelRotation(currentSteering * 90f);
        }
        
        // Throttle (W/S or Up/Down arrows)
        float throttleInput = Input.GetAxis("Vertical");
        
        // Smooth throttle
        currentThrottle = Mathf.MoveTowards(currentThrottle, throttleInput, keyboardThrottleSpeed * Time.deltaTime);
        
        // Apply to altitude lever
        if (altitudeLever != null)
        {
            altitudeLever.SetLeverPosition(currentThrottle);
        }
        
        // Brake
        if (Input.GetKey(brakeKey))
        {
            if (flightController != null)
            {
                flightController.SetFlying(false);
            }
        }
        else if (Input.GetKeyUp(brakeKey))
        {
            if (flightController != null)
            {
                flightController.SetFlying(true);
            }
        }
    }
    
    void HandleUIInput()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (houseManager != null)
            {
                houseManager.PauseFlight();
            }
        }
        
        // Quick location jumps (number keys)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (houseManager != null) houseManager.GoToLocation("nyc");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (houseManager != null) houseManager.GoToLocation("paris");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (houseManager != null) houseManager.GoToLocation("tokyo");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (houseManager != null) houseManager.GoToLocation("sf");
        }
        
        // Weather toggles
        if (Input.GetKeyDown(KeyCode.F1))
        {
            WeatherManager wm = FindObjectOfType<WeatherManager>();
            if (wm != null) wm.SetWeather("Clear");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            WeatherManager wm = FindObjectOfType<WeatherManager>();
            if (wm != null) wm.SetWeather("Cloudy");
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            WeatherManager wm = FindObjectOfType<WeatherManager>();
            if (wm != null) wm.SetWeather("Rain");
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            WeatherManager wm = FindObjectOfType<WeatherManager>();
            if (wm != null) wm.SetWeather("Storm");
        }
    }
    
    void ToggleCursorLock()
    {
        cursorLocked = !cursorLocked;
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !cursorLocked;
    }
    
    void EnableDesktopMode()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Set up a desktop camera if needed
        if (playerCamera == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                playerCamera = cam.transform;
            }
        }
    }
    
    public void EnableVRMode()
    {
        useDesktopInput = false;
        enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void OnGUI()
    {
        if (!useDesktopInput) return;
        
        // Show control hints
        GUILayout.BeginArea(new Rect(10, 10, 250, 200));
        GUILayout.BeginVertical("box");
        
        GUILayout.Label("DESKTOP CONTROLS", GUILayout.Height(20));
        GUILayout.Label("WASD / Arrows - Steer & Throttle");
        GUILayout.Label("Space - Brake (hover)");
        GUILayout.Label("Right Click / L - Lock mouse");
        GUILayout.Label("ESC - Pause");
        GUILayout.Label("1-4 - Jump to cities");
        GUILayout.Label("F1-F4 - Change weather");
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
