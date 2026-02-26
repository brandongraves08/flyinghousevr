using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AltitudeLever : MonoBehaviour
{
    [Header("Lever Settings")]
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float returnSpeed = 60f;
    public bool autoReturn = true;
    
    [Header("Output")]
    public float throttleOutput = 0f;  // -1 to 1
    
    [Header("Visual")]
    public Transform leverMesh;
    public Transform handle;
    
    [Header("Haptics")]
    public float hapticAmplitude = 0.3f;
    public float hapticInterval = 0.1f;
    
    private XRGrabInteractable grabInteractable;
    private float currentAngle = 0f;
    private bool isGrabbed = false;
    private Transform grabbingController;
    private float lastHapticTime;
    
    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
        }
        
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        
        if (handle != null)
            grabInteractable.attachTransform = handle;
    }
    
    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }
    
    void Update()
    {
        if (isGrabbed && grabbingController != null)
        {
            // Calculate angle from controller position
            currentAngle = GetAngleFromController();
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
            
            // Haptics at interval
            if (Time.time - lastHapticTime > hapticInterval)
            {
                SendHaptics();
                lastHapticTime = Time.time;
            }
        }
        else if (autoReturn)
        {
            // Return to neutral
            currentAngle = Mathf.MoveTowards(currentAngle, 0f, returnSpeed * Time.deltaTime);
        }
        
        // Update visual
        if (leverMesh != null)
        {
            leverMesh.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);
        }
        
        // Calculate throttle output (-1 = dive/slow, 0 = hover, 1 = climb/speed)
        float range = Mathf.Max(maxAngle - minAngle, 0.1f);
        float normalized = (currentAngle - minAngle) / range;
        throttleOutput = (normalized * 2f) - 1f;  // Convert to -1 to 1
    }
    
    float GetAngleFromController()
    {
        if (grabbingController == null) return 0f;
        
        // Get local position of controller relative to lever pivot
        Vector3 localPos = transform.InverseTransformPoint(grabbingController.position);
        
        // Calculate angle (pitching forward/back)
        return Mathf.Atan2(localPos.z, localPos.y) * Mathf.Rad2Deg;
    }
    
    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        grabbingController = args.interactorObject.transform;
        lastHapticTime = Time.time;
    }
    
    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        grabbingController = null;
    }
    
    void SendHaptics()
    {
        if (grabbingController == null) return;
        
        var controller = grabbingController.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor>();
        controller?.SendHapticImpulse(hapticAmplitude, 0.05f);
    }
    
    public float GetThrottleInput()
    {
        return throttleOutput;
    }
    
    public void SetLeverPosition(float position)
    {
        // position is -1 to 1
        float range = maxAngle - minAngle;
        currentAngle = minAngle + ((position + 1f) / 2f * range);
    }
}