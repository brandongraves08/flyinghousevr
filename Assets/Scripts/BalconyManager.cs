using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BalconyManager : MonoBehaviour
{
    [Header("Balcony Points")]
    public Transform[] windowPoints;
    public Transform balconyRail;
    public Transform playerXRRig;
    
    [Header("Interaction")]
    public float leanDistance = 0.5f;
    public float transitionSpeed = 3f;
    public float tiltAngle = 15f;
    
    [Header("Safety")]
    public bool requireConfirmation = true;
    public AudioSource windRushAudio;
    
    private bool isLeaning = false;
    private int activeWindow = -1;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private FlightController flightController;
    
    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        flightController = FindObjectOfType<FlightController>();
    }
    
    void Update()
    {
        if (isLeaning)
        {
            // Player can look around but maintains leaning position
            UpdateLeanPosition();
            
            // Check for return gesture
            if (Input.GetButtonDown("XRI_Right_Trigger") || Input.GetKeyDown(KeyCode.Space))
            {
                ReturnFromBalcony();
            }
        }
        else
        {
            // Check if player is at window and wants to lean out
            CheckWindowProximity();
        }
    }
    
    void CheckWindowProximity()
    {
        if (playerXRRig == null) return;
        
        for (int i = 0; i < windowPoints.Length; i++)
        {
            float distance = Vector3.Distance(playerXRRig.position, windowPoints[i].position);
            if (distance < 0.5f && Input.GetButtonDown("XRI_Right_Grip"))
            {
                LeanOutWindow(i);
                break;
            }
        }
    }
    
    void LeanOutWindow(int windowIndex)
    {
        if (windowIndex < 0 || windowIndex >= windowPoints.Length) return;
        
        activeWindow = windowIndex;
        isLeaning = true;
        
        // Store original
        originalPosition = playerXRRig.position;
        originalRotation = playerXRRig.rotation;
        
        // Calculate leaning position
        Transform window = windowPoints[windowIndex];
        Vector3 leanPosition = window.position + window.forward * leanDistance;
        Quaternion leanRotation = Quaternion.Euler(tiltAngle, window.rotation.eulerAngles.y, 0);
        
        // Smooth transition
        StartCoroutine(LeanTransition(leanPosition, leanRotation));
        
        // Effects
        if (windRushAudio != null)
        {
            windRushAudio.volume = 0.7f;
            windRushAudio.Play();
        }
        
        Debug.Log("Leaning out window " + windowIndex);
    }
    
    System.Collections.IEnumerator LeanTransition(Vector3 targetPos, Quaternion targetRot)
    {
        float t = 0;
        Vector3 startPos = playerXRRig.position;
        Quaternion startRot = playerXRRig.rotation;
        
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            playerXRRig.position = Vector3.Lerp(startPos, targetPos, t);
            playerXRRig.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
        
        playerXRRig.position = targetPos;
        playerXRRig.rotation = targetRot;
    }
    
    void UpdateLeanPosition()
    {
        // Apply slight turbulence movement when leaning
        if (flightController != null && weatherManager != null)
        {
            WeatherManager wm = FindObjectOfType<WeatherManager>();
            if (wm != null)
            {
                float turbulence = wm.currentTurbulance;  // Manual bridge if needed
                Vector3 shake = Random.insideUnitSphere * turbulence * 0.05f;
                playerXRRig.position += shake;
            }
        }
    }
    
    public void ReturnFromBalcony()
    {
        if (!isLeaning) return;
        
        StartCoroutine(LeanTransition(originalPosition, originalRotation));
        
        if (windRushAudio != null)
        {
            windRushAudio.volume = 0;
        }
        
        isLeaning = false;
        activeWindow = -1;
        
        Debug.Log("Returned to cockpit");
    }
    
    public bool IsLeaning()
    {
        return isLeaning;
    }
}
