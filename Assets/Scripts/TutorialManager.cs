using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public string title;
        [TextArea(3, 5)]
        public string description;
        public Sprite icon;
        public KeyCode advanceKey = KeyCode.Space;
        public float autoAdvanceDelay = 0;
        public bool waitForAction;
        public string actionName;
    }
    
    [Header("UI References")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;
    public Button nextButton;
    public Button skipButton;
    public Button prevButton;
    
    [Header("Steps")]
    public TutorialStep[] steps;
    
    [Header("Settings")]
    public bool showAtStart = true;
    public bool saveProgress = true;
    
    private int currentStep = 0;
    private bool isShowing = false;
    
    void Start()
    {
        if (saveProgress && PlayerPrefs.GetInt("TutorialComplete", 0) == 1)
        {
            showAtStart = false;
        }
        
        if (showAtStart)
        {
            StartTutorial();
        }
    }
    
    public void StartTutorial()
    {
        currentStep = 0;
        tutorialPanel?.SetActive(true);
        isShowing = true;
        ShowStep(0);
    }
    
    void ShowStep(int index)
    {
        if (index < 0 || index >= steps.Length)
        {
            EndTutorial();
            return;
        }
        
        currentStep = index;
        var step = steps[index];
        
        if (titleText != null) titleText.text = step.title;
        if (descriptionText != null) descriptionText.text = step.description;
        if (iconImage != null && step.icon != null) iconImage.sprite = step.icon;
        
        // Update buttons
        if (prevButton != null) prevButton.interactable = index > 0;
        if (nextButton != null) nextButton.gameObject.SetActive(!step.waitForAction);
        
        // Auto-advance if set
        if (step.autoAdvanceDelay > 0)
        {
            StartCoroutine(AutoAdvance(step.autoAdvanceDelay));
        }
        
        // Show help if waiting for action
        if (step.waitForAction)
        {
            StartCoroutine(WaitForAction(step.actionName));
        }
    }
    
    IEnumerator AutoAdvance(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextStep();
    }
    
    IEnumerator WaitForAction(string actionName)
    {
        descriptionText.text += $"\n\n<color=#00FF00>Press {steps[currentStep].advanceKey}</color>";
        
        while (!Input.GetKeyDown(steps[currentStep].advanceKey))
        {
            yield return null;
        }
        
        NextStep();
    }
    
    void Update()
    {
        if (!isShowing) return;
        
        var step = steps[currentStep];
        
        if (step.advanceKey != KeyCode.None && Input.GetKeyDown(step.advanceKey) && !step.waitForAction)
        {
            NextStep();
        }
        
        // Skip tutorial
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndTutorial();
        }
    }
    
    public void NextStep()
    {
        ShowStep(currentStep + 1);
    }
    
    public void PreviousStep()
    {
        ShowStep(currentStep - 1);
    }
    
    public void SkipTutorial()
    {
        EndTutorial();
    }
    
    void EndTutorial()
    {
        isShowing = false;
        tutorialPanel?.SetActive(false);
        
        if (saveProgress)
        {
            PlayerPrefs.SetInt("TutorialComplete", 1);
            PlayerPrefs.Save();
        }
        
        Debug.Log("Tutorial complete");
    }
    
    public void ResetTutorial()
    {
        PlayerPrefs.SetInt("TutorialComplete", 0);
        StartTutorial();
    }
}
