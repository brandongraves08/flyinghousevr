using UnityEngine;
using System.Collections;

public class RoomConnector : MonoBehaviour
{
    [Header("Connection")]
    public string connectedRoomId;
    public string connectedRoomName;
    public Transform spawnPoint;
    
    [Header("Visual")]
    public GameObject doorVisual;
    public Material lockedMaterial;
    public Material unlockedMaterial;
    public Material activeMaterial;
    
    [Header("Interaction")]
    public float activationRange = 2f;
    public KeyCode activationKey = KeyCode.E;
    public bool requireVRTrigger = true;
    
    [Header("Transition")]
    public float fadeDuration = 0.5f;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private bool isLocked = true;
    private HomeRoomSystem roomSystem;
    private ScreenFader fader;
    
    void Start()
    {
        roomSystem = FindObjectOfType<HomeRoomSystem>();
        fader = FindObjectOfType<ScreenFader>();
        
        UpdateVisualState();
    }
    
    void Update()
    {
        // Auto-update lock status
        if (roomSystem != null)
        {
            bool shouldBeLocked = IsRoomLocked();
            if (shouldBeLocked != isLocked)
            {
                isLocked = shouldBeLocked;
                UpdateVisualState();
            }
        }
        
        // Check for activation
        CheckActivation();
    }
    
    bool IsRoomLocked()
    {
        var room = roomSystem?.GetRoomById(connectedRoomId);
        return room?.isLocked ?? true;
    }
    
    void UpdateVisualState()
    {
        if (doorVisual == null) return;
        
        var renderer = doorVisual.GetComponent<Renderer>();
        if (renderer == null) return;
        
        if (isLocked)
        {
            renderer.material = lockedMaterial;
        }
        else
        {
            renderer.material = unlockedMaterial;
        }
    }
    
    void CheckActivation()
    {
        // Find player
        var player = Camera.main?.transform;
        if (player == null) return;
        
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > activationRange) return;
        
        // VR or desktop input
        bool shouldActivate = false;
        
        #if UNITY_ANDROID
        // VR: Check for controller trigger
        shouldActivate = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool trigger) && trigger;
        #else
        // Desktop: Key press
        shouldActivate = Input.GetKeyDown(activationKey);
        #endif
        
        if (shouldActivate && !isLocked)
        {
            ActivateConnector();
        }
    }
    
    void ActivateConnector()
    {
        Debug.Log($"Teleporting to {connectedRoomName}");
        StartCoroutine(TeleportSequence());
    }
    
    IEnumerator TeleportSequence()
    {
        // Fade out
        if (fader != null)
        {
            yield return fader.FadeOut(fadeDuration);
        }
        
        // Teleport
        roomSystem?.SwitchRoom(connectedRoomId);
        
        // Move player to spawn point
        var playerRig = FindObjectOfType<UnityEngine.XR.Interaction.Toolkit.XRRig>();
        if (playerRig != null && spawnPoint != null)
        {
            playerRig.MoveCameraToWorldLocation(spawnPoint.position);
        }
        else if (Camera.main != null && spawnPoint != null)
        {
            Camera.main.transform.position = spawnPoint.position;
            Camera.main.transform.rotation = spawnPoint.rotation;
        }
        
        // Fade in
        if (fader != null)
        {
            yield return fader.FadeIn(fadeDuration);
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = isLocked ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, activationRange);
        
        if (spawnPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, spawnPoint.position);
            Gizmos.DrawWireSphere(spawnPoint.position, 0.2f);
        }
    }
}
