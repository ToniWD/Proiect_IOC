using UnityEngine;

public class ClickableFruit : MonoBehaviour
{
    [Header("Configurare Vizuală")]
    [Tooltip("Culoarea highlight când mouse-ul e deasupra")]
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);
    
    [Tooltip("Scala la hover")]
    public float hoverScale = 1.15f;
    
    [Tooltip("Dacă e true, fructul primește un sprite random la start")]
    public bool useRandomSprite = true;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalScale;
    private bool isHovered = false;
    private bool isCollected = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        originalScale = transform.localScale;
    }

    void Start()
    {
        if (useRandomSprite && FruitCollectorManager.Instance != null)
        {
            Sprite randomSprite = FruitCollectorManager.Instance.GetRandomFruitSprite();
            if (randomSprite != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = randomSprite;
            }
        }
    }

    void OnMouseEnter()
    {
        if (isCollected) return;
        
        isHovered = true;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = highlightColor;
        }
        
        transform.localScale = originalScale * hoverScale;
    }

    void OnMouseExit()
    {
        if (isCollected) return;
        
        isHovered = false;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        
        transform.localScale = originalScale;
    }

    void OnMouseDown()
    {
        if (isCollected) return;
        
        if (FruitCollectorManager.Instance != null)
        {
            isCollected = true;
            
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
            
            FruitCollectorManager.Instance.AttemptCollectFruit(this.gameObject);
        }
    }

    public void ResetFruit()
    {
        isCollected = false;
        isHovered = false;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        transform.localScale = originalScale;
        gameObject.SetActive(true);
    }
}