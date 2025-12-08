using UnityEngine;

public class ClickableFruit : MonoBehaviour
{
    [Header("Configurare Vizuală")]
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);
    public float hoverScale = 1.15f;
    public bool useRandomSprite = true;

    private Vector3 initialPosition;

    private bool isInBasket = false;
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

        initialPosition = transform.position;

        if (transform.parent != null && transform.parent.name.Contains("Cos_total"))
        {
            isInBasket = true;
        }
    }

    void Start()
    {
        if (useRandomSprite && FruitCollectorManager.Instance != null && !isInBasket)
        {
            Sprite randomSprite = FruitCollectorManager.Instance.GetRandomFruitSprite();
            if (randomSprite != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = randomSprite;
            }
        }
    }

    public void SetCollectedInBasket()
    {
        isCollected = true;
        isInBasket = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        transform.localScale = originalScale * 0.7f;
    }


    void OnMouseEnter()
    {
        if (isCollected) return;

        isHovered = true;

        if (spriteRenderer != null)
        {
            if (!isInBasket)
            {
                spriteRenderer.color = highlightColor;
            }
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
        if (FruitCollectorManager.Instance == null) return;

        if (isInBasket)
        {
            FruitCollectorManager.Instance.RemoveFruitFromBasket(this.gameObject);
        }
        else 
        {
            if (isCollected) return;

            if (!FruitCollectorManager.Instance.levelCompleted || FruitCollectorManager.Instance.currentFruits < FruitCollectorManager.Instance.targetFruits)
            {
                isCollected = true;
            }

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
        isInBasket = false;

        transform.SetParent(null);
        transform.position = initialPosition;
        transform.localScale = originalScale;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        gameObject.SetActive(true);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
        }
    }
}