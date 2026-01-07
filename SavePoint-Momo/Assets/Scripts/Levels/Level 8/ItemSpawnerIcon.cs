using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ItemSpawnerIcon : MonoBehaviour
{
    public SeasonType requiredSeason;
    public GameObject prefabToSpawn;
    public Transform containerRef;

    private void OnMouseDown()
    {
        // Verificam daca e anotimpul corect
        if (TreeLevelManager.CurrentSeason == requiredSeason)
        {
            SpawnItem();
        }
        else
        {
            // DACA A GRESIT -> Redam sunetul de eroare din Manager
            if (TreeLevelManager.Instance != null)
            {
                TreeLevelManager.Instance.PlayErrorSound();
                Debug.Log("Sunet eroare!");
            }
        }
    }

    void SpawnItem()
    {
        if (prefabToSpawn != null)
        {
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPos.z = 0;

            GameObject newItem = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            if (containerRef != null)
            {
                newItem.transform.SetParent(containerRef);
            }
            else
            {
                GameObject foundContainer = GameObject.Find("ActiveDecorations");
                if (foundContainer != null) newItem.transform.SetParent(foundContainer.transform);
            }
        }
    }
}