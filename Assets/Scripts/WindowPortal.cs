using UnityEngine;

public class WindowPortal : MonoBehaviour
{
    [Header("Window Settings")]
    public Camera mainCamera;
    public LayerMask terrainLayer;
    
    [Header("Stencil Masking")]
    public Material windowMaterial;
    public Material wallMaterial;
    
    [Header("Visual")]
    public Transform windowFrame;
    public MeshRenderer[] windowGlassRenderers;
    
    void Start()
    {
        SetupStencilMasking();
    }
    
    void SetupStencilMasking()
    {
        if (windowMaterial == null || wallMaterial == null)
        {
            Debug.LogWarning("WindowPortal: Materials not assigned. Creating default materials.");
            CreateDefaultMaterials();
        }
        
        // Apply materials to window frame for stencil effect
        if (windowGlassRenderers != null)
        {
            foreach (var renderer in windowGlassRenderers)
            {
                if (renderer != null)
                {
                    renderer.material = windowMaterial;
                    renderer.material.renderQueue = 3000;  // Make sure it renders properly
                }
            }
        }
    }
    
    void CreateDefaultMaterials()
    {
        // Window material - uses stencil to mask terrain
        windowMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        windowMaterial.SetFloat("_Stencil", 1);
        windowMaterial.SetFloat("_StencilOp", 0);  // Keep
        windowMaterial.SetFloat("_StencilComp", 8); // Always
        windowMaterial.SetFloat("_StencilWriteMask", 255);
        windowMaterial.SetFloat("_StencilReadMask", 255);
        windowMaterial.color = Color.clear;  // Transparent window
        
        // Wall material - masks terrain except where stencil is set
        wallMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        wallMaterial.SetFloat("_Stencil", 1);
        wallMaterial.SetFloat("_StencilRef", 1);
        wallMaterial.SetFloat("_StencilComp", 6);  // NotEqual - won't render where stencil is 1
    }
    
    public void SetWindowSize(float width, float height)
    {
        if (windowFrame != null)
        {
            windowFrame.localScale = new Vector3(width, height, 1f);
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 1.5f, 0.1f));
    }
}