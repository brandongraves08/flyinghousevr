using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Helper script for setting up XR in the FlyingHouse scene.
/// Attach to a GameObject in the scene and call SetupXRFromEditor.
/// </summary>
public class XRSetupHelper : MonoBehaviour
{
    [Header("XR Rig Setup")]
    public GameObject xrRigPrefab;
    public Transform xrOriginTarget;
    
    [Header("Tracked Devices")]
    public GameObject leftHandPrefab;
    public GameObject rightHandPrefab;
    
    [Header("Scene References")]
    public Transform steeringWheelSpawnPoint;
    public Transform leverSpawnPoint;
    public Transform[] windowPoints;
    
    [ContextMenu("Setup Scene for XR")]
    void SetupXREditor()
    {
        Debug.Log("XR Setup Helper: Ensure XR Origin and Interaction Manager are in scene");
        Debug.Log("Add XR Origin (Action-based), XR Interaction Manager, and Input Action Manager");
        Debug.Log("Place Steering Wheel and Lever at spawn points");
        Debug.Log("Tag Window transforms for lean-out detection");
    }
    
    void OnDrawGizmos()
    {
        // Draw spawn point gizmos
        Gizmos.color = Color.green;
        if (steeringWheelSpawnPoint != null)
        {
            Gizmos.DrawWireSphere(steeringWheelSpawnPoint.position, 0.2f);
            Gizmos.DrawLine(transform.position, steeringWheelSpawnPoint.position);
        }
        
        Gizmos.color = Color.blue;
        if (leverSpawnPoint != null)
        {
            Gizmos.DrawWireCube(leverSpawnPoint.position, Vector3.one * 0.2f);
            Gizmos.DrawLine(transform.position, leverSpawnPoint.position);
        }
        
        Gizmos.color = Color.cyan;
        if (windowPoints != null)
        {
            foreach (var window in windowPoints)
            {
                if (window != null)
                {
                    Gizmos.DrawWireCube(window.position, new Vector3(1.5f, 1f, 0.1f));
                }
            }
        }
    }
}
