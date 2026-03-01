using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickGame : MonoBehaviour
{
    void Start()
    {
        // Press any key or wait 3 seconds
        Invoke("StartGame", 3f);
    }
    
    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartGame();
        }
    }
    
    void StartGame()
    {
        SceneManager.LoadScene("FlyingHouse");
    }
}
