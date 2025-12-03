using UnityEngine;
using UnityEngine.InputSystem;

public class UnclickableFish : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite[] spriteOptions;
    
    private float shakeDuration = 0.1f;        // How long it shakes
    public float magnitude = 0.25f;     // How strong the shake is

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) Debug.LogError("SpriteRenderer missing!");
        int fish = Random.Range(0, spriteOptions.Length);
        sr.sprite = spriteOptions[fish];
    }
    
    void Update()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }
        
        if (Mouse.current == null) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartCoroutine(Shake());
            }
        }
    }
    
    private System.Collections.IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < 0.1f)
        {
            float x = Random.Range(-1f, 1f) * 0.25f;
            float y = Random.Range(-1f, 1f) * 0.25f;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos; // Reset
    }
}
