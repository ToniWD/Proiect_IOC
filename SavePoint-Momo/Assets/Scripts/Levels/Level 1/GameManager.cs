using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int triangleCount;

    
    public Camera mainCamera;
    
    private void Awake()
    {
        instance = this;
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Start()
    {
        triangleCount = GameObject.FindGameObjectsWithTag("Triangle").Length;
    }

    public void RemoveTriangle()
    {
        triangleCount--;

        if (triangleCount <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.Log("Ai câștigat! Nu mai sunt triunghiuri!");
        Time.timeScale = 0f;
    }
    
}