using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameOver;
    
    private int triangleCount = 0;
    [SerializeField]
    private ShapeGenerator shapeGenerator;
    
    public Camera mainCamera;
    
    private void Awake()
    {
        instance = this;
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void RemoveTriangle()
    {
        triangleCount++;

        if (triangleCount == shapeGenerator.blueCount)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        isGameOver = true;
        Debug.Log("Ai câștigat! Nu mai sunt triunghiuri!");
        //Time.timeScale = 0f;
        StopAllCoroutines();
    }
    
}