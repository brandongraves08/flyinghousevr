using UnityEngine;
using System.Collections;

public class MultiRoomTutorial : MonoBehaviour
{
    [Header("Tutorial Steps")]
    public string[] tutorialMessages = {
        "Welcome to your Flying House! Press 'R' to open the Room Menu.",
        "You start in the Cockpit. This is your main flight deck.",
        "To unlock new rooms, you need credits. Complete flights to earn more!",
        "Try visiting the Navigation Room ($500) to plan routes.",
        "Each room has unique features - the Observatory lets you stargaze.",
        "Press 'E' near a door connector to travel between rooms.",
        "Your progress saves automatically. Happy flying!"
    };
    
    [Header("UI")]
    public TMPro.TextMeshProUGUI messageText;
    public GameObject tutorialPanel;
    public float displayDuration = 5f;
    
    private int currentStep = 0;
    private bool tutorialActive = false;
    
    void Start()
    {
        if (ShouldShowTutorial())
        {
            StartCoroutine(RunTutorial());
        }
    }
    
    bool ShouldShowTutorial()
    {
        return PlayerPrefs.GetInt("MultiRoomTutorialComplete", 0) == 0;
    }
    
    IEnumerator RunTutorial()
    {
        tutorialActive = true;
        tutorialPanel?.SetActive(true);
        
        for (int i = 0; i < tutorialMessages.Length; i++)
        {
            currentStep = i;
            if (messageText != null)
                messageText.text = $"{i+1}/{tutorialMessages.Length}: {tutorialMessages[i]}";
            
            yield return new WaitForSeconds(displayDuration);
            
            // Wait for key press to continue (optional)
            if (i < tutorialMessages.Length - 1)
            {
                yield return new WaitForSeconds(1f);
            }
        }
        
        MarkComplete();
    }
    
    void MarkComplete()
    {
        tutorialActive = false;
        tutorialPanel?.SetActive(false);
        PlayerPrefs.SetInt("MultiRoomTutorialComplete", 1);
        PlayerPrefs.Save();
        Debug.Log("Multi-room tutorial complete");
    }
    
    public void ResetTutorial()
    {
        PlayerPrefs.SetInt("MultiRoomTutorialComplete", 0);
    }
}
