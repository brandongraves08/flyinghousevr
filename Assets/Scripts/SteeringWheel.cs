using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SteeringWheel : MonoBehaviour
{
    [Header("Steering Settings")]
    public float maxSteeringAngle = 450f;
    public float returnSpeed = 100f;
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
    private float steeringInput = 0f;
    private bool isGrabbed = false;
    private Transform grabbingController;
    
    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
        
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
    {
        if (isGrabbed && grabbingController != null)
        {
            Vector3 controllerForward = grabbingController.forward;
            Vector3 localForward = transform.InverseTransformDirection(controllerForward);
            float targetAngle = Mathf.Atan2(localForward.x, localForward.z) * Mathf.Rad2Deg * sensitivity;
            targetAngle = Mathf.Clamp(targetAngle, -maxSteeringAngle/2, maxSteeringAngle/2);
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, 10f * Time.deltaTime);
            
            if (Mathf.Abs(currentAngle - targetAngle) > 1f)
                SendHaptics(hapticAmplitude * 0.5f);
        }
        else
        {
            currentAngle = Mathf.MoveTowards(currentAngle, 0f, returnSpeed * Time.deltaTime);
        }
        
        if (wheelMesh != null)
            wheelMesh.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
        
        steeringInput = Mathf.Clamp(currentAngle / (maxSteeringAngle / 2f), -1f, 1f);
    }
    
    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        grabbingController = args.interactorObject.transform;
        SendHaptics(hapticAmplitude);
    }
    
    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        grabbingController = null;
    }
    
    void SendHaptics(float amplitude)
    {
        if (grabbingController == null) return;
        var controller = grabbingController.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor>();
        controller?.SendHapticImpulse(amplitude, hapticDuration);
    }
    
    public float GetSteeringInput() => steeringInput;
    public void SetWheelRotation(float angle) => currentAngle = Mathf.Clamp(angle, -maxSteeringAngle/2, maxSteeringAngle/2);
}
