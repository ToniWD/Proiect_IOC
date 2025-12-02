using UnityEngine;
using UnityEngine.InputSystem;

public class TriangleClick : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) Debug.LogError("SpriteRenderer missing!");
        originalColor = sr.color;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("Clicked " + gameObject.name);
                
                Destroy(gameObject);
                GameManager.instance.RemoveTriangle();
            }
        }
        else
        {
            sr.color = originalColor;
        }
    }
}
