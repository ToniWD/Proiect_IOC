using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    public GameObject trianglePrefab;
    public GameObject circlePrefab;
    public GameObject squarePrefab;
    public GameObject ovalPrefab;

    public int numberOfShapes = 20;
    public float spawnRadius = 20f; 
    public int maxAttempts = 50;
    public int blueCount = 0;

    void Start()
    {
        for (int i = 0; i < numberOfShapes; i++)
        {
            SpawnRandomShape();
        }
        Debug.Log("Blue count - " + blueCount);
        if (blueCount < 3)
        {
            for (int i = 0; i < 3; i++)
            {
                SpawnClickableFish();
            }
        }

        Debug.Log("Blue count - " + blueCount);
    }

    private void SpawnClickableFish()
    {
        GameObject prefab = null;

        prefab = trianglePrefab; 

        Vector3 pos = Vector3.zero;
        bool found = false;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            pos = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);

            Collider2D hit = Physics2D.OverlapCircle(pos, spawnRadius);
            if (hit == null)
            {
                found = true;
                break;
            }
        }

        if (found)
        {
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            go.GetComponent<TriangleClick>().ForceClickable();
            blueCount += go.GetComponent<TriangleClick>().isDestroyable? 1 :0;
        }
        else
        {
            Debug.LogWarning("Nu s-a găsit loc liber pentru obiect.");
        }
    }
    
    void SpawnRandomShape()
    {
        GameObject prefab = null;
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0: 
                prefab = trianglePrefab; 
                break;
            case 1: 
                prefab = circlePrefab;
                break;
            case 2: prefab = squarePrefab; break;
            case 3: prefab = ovalPrefab; break;
        }

        Vector3 pos = Vector3.zero;
        bool found = false;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            pos = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);

            Collider2D hit = Physics2D.OverlapCircle(pos, spawnRadius);
            if (hit == null)
            {
                found = true;
                break;
            }
        }

        if (found)
        {
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            if (go.CompareTag("Triangle"))
            {
                blueCount += go.GetComponent<TriangleClick>().isDestroyable? 1 :0;
            }
        }
        else
        {
            Debug.LogWarning("Nu s-a găsit loc liber pentru obiect.");
        }
    }
    
}