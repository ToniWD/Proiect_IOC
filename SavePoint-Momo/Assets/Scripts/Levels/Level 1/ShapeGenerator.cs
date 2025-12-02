using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    public GameObject trianglePrefab;
    public GameObject circlePrefab;
    public GameObject squarePrefab;
    public GameObject ovalPrefab;

    public int numberOfShapes = 20;
    public float spawnRadius = 0.5f; 
    public int maxAttempts = 50;  

    void Start()
    {
        for (int i = 0; i < numberOfShapes; i++)
        {
            SpawnRandomShape();
        }
    }

    void SpawnRandomShape()
    {
        GameObject prefab = null;
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0: prefab = trianglePrefab; break;
            case 1: prefab = circlePrefab; break;
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
            Instantiate(prefab, pos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Nu s-a găsit loc liber pentru obiect.");
        }
    }
}