using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [Header("UI")]
    public Image fadeImage;
    public CanvasGroup fadeCanvas;
    
    [Header("Settings")]
    public Color fadeColor = Color.black;
    public bool fadeInOnStart = true;
    
    private void Start()
    {
        if (fadeCanvas == null)
        {
            // Create fade canvas if not assigned
            CreateFadeCanvas();
        }
        
        if (fadeInOnStart)
        {
            StartCoroutine(FadeIn(1f));
        }
    }
    
    void CreateFadeCanvas()
    {
        GameObject canvasGO = new GameObject("FadeCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        
        canvasGO.AddComponent<GraphicRaycaster>();
        
        GameObject imageGO = new GameObject("FadeImage");
        imageGO.transform.SetParent(canvasGO.transform);
        
        fadeImage = imageGO.AddComponent<Image>();
        fadeImage.color = fadeColor;
        
        RectTransform rect = imageGO.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        
        fadeCanvas = canvasGO.AddComponent<CanvasGroup>();
        fadeCanvas.alpha = 0;
        
        DontDestroyOnLoad(canvasGO);
    }
    
    public IEnumerator FadeOut(float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        fadeCanvas.alpha = 1;
    }
    
    public IEnumerator FadeIn(float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Clamp01(1 - (elapsed / duration));
            yield return null;
        }
        fadeCanvas.alpha = 0;
    }
    
    public IEnumerator FadeToColor(Color color, float duration)
    {
        if (fadeImage != null)
        {
            fadeImage.color = color;
        }
        yield return FadeOut(duration);
    }
}
