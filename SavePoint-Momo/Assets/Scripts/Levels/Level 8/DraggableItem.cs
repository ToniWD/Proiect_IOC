using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class DraggableItem : MonoBehaviour
{
    private bool isDragging = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        // Cand se naste, incepe direct in modul "tras"
        isDragging = true;
    }

    // ACEASTA ESTE FUNCTIA NOUA CARE LIPSEA:
    // Permite sa ridici obiectul din nou dupa ce l-ai lasat jos
    private void OnMouseDown()
    {
        isDragging = true;
    }

    void Update()
    {
        if (isDragging)
        {
            MoveWithMouse();

            // Daca ridici degetul, obiectul cade
            if (Input.GetMouseButtonUp(0))
            {
                StopDragging();
            }
        }
    }

    void MoveWithMouse()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10f;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0;
        transform.position = worldPos;
    }

    void StopDragging()
    {
        isDragging = false;

        // Verificam limitele ecranului
        if (transform.position.x > 15f || transform.position.x < -15f || transform.position.y > 10f || transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}