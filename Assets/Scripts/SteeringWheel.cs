using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SteeringWheel : MonoBehaviour
{
    [Header("Steering Settings")]
    public float maxSteeringAngle = 450f;  // Total degrees lock-to-lock
    public float returnSpeed = 100f;       // Auto-center when released
    public float sensitivity = 1f;
    
    [Header("Visual")]
    public Transform wheelMesh;
    public Transform leftHandAttach;
    public Transform rightHandAttach;
    
    [Header("Haptics")]
    public float hapticAmplitude = 0.5f;
    public float hapticDuration = 0.1f;
    
    private XRGrabInteractable grabInteractable;
    private float currentAngle = 0f;
    private float steeringInput = 0f;  // -1 to 1
    private bool isGrabbed = false;
    private Transform grabbingController;
    
    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
        }
        
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        
        if (leftHandAttach != null)
            grabInteractable.attachTransform = leftHandAttach;
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
