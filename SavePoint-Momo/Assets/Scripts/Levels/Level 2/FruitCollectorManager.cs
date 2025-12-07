using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FruitCollectorManager : MonoBehaviour
{
    [Header("Configurare Nivel")]
    public int targetFruits = 5;      
    private int currentFruits = 0;   
    private bool levelCompleted = false;

    [Header("Referințe UI")]
    public Text counterDisplay;      
    public GameObject winMessageObject; 
    public GameObject warningTextObject;

    [Header("Resurse Vizuale - Fructe")]
    [SerializeField]
    private Sprite[] fruitSprites;

    [Header("Resurse Vizuale - Coș")]
    public Sprite basketBackSprite;
    
    public Sprite basketFrontSprite;
    
    public Sprite basketRefSprite;
    
    public Sprite basketWithPawSprite;

    [Header("Resurse Vizuale - Fundaluri")]
    public Sprite backgroundMainSprite;
    
    public Sprite backgroundSearchingSprite;
    
    public SpriteRenderer backgroundRenderer;

    [Header("Referințe Coș")]
    public Transform basketTransform;
    public SpriteRenderer basketBackRenderer;
    public SpriteRenderer basketFrontRenderer;
    public SpriteRenderer basketWithPawRenderer;
    
    public Transform[] fruitSlotsInBasket;

    [Header("Indicator Contor Coș")]
    public Image basketIndicatorImage;
    public Text basketCounterText;

    [Header("Animații și Efecte")]
    public float fruitMoveAnimationDuration = 0.5f;
    
    public float collectAnimationScale = 1.2f;

    [Header("Prefab Fruct în Coș")]
    public GameObject fruitInBasketPrefab;

    private List<GameObject> fruitsInBasket = new List<GameObject>();

    public static FruitCollectorManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetupVisuals();
        UpdateUI();
        if (winMessageObject) winMessageObject.SetActive(false);
        if (warningTextObject) warningTextObject.SetActive(false);
    }

    void SetupVisuals()
    {

        if (backgroundRenderer != null && backgroundMainSprite != null)
        {
            backgroundRenderer.sprite = backgroundMainSprite;
        }

        if (basketBackRenderer != null && basketBackSprite != null)
        {
            basketBackRenderer.sprite = basketBackSprite;
        }

        if (basketFrontRenderer != null && basketFrontSprite != null)
        {
            basketFrontRenderer.sprite = basketFrontSprite;
        }

        if (basketWithPawRenderer != null && basketWithPawSprite != null)
        {
            basketWithPawRenderer.sprite = basketWithPawSprite;
        }
    }

    public void AttemptCollectFruit(GameObject fruitObject)
    {
        if (levelCompleted) return;

        if (currentFruits >= targetFruits)
        {
            Debug.Log("Miau! Ai ales prea multe fructe. Cosul e deja plin!");
            ShowWarning(); 
            return; 
        }

        currentFruits++;

        SpriteRenderer fruitSpriteRenderer = fruitObject.GetComponent<SpriteRenderer>();
        Sprite collectedFruitSprite = fruitSpriteRenderer != null ? fruitSpriteRenderer.sprite : null;

        AnimateFruitToBasket(fruitObject, collectedFruitSprite);

        UpdateUI();
        Debug.Log("Fruct colectat! Total: " + currentFruits);

        if (currentFruits == targetFruits)
        {
            WinLevel();
        }
    }

    void AnimateFruitToBasket(GameObject fruitObject, Sprite fruitSprite)
    {
        if (basketTransform != null)
        {
            StartCoroutine(MoveFruitToBasket(fruitObject, fruitSprite));
        }
        else
        {
            fruitObject.SetActive(false);
        }
    }

    System.Collections.IEnumerator MoveFruitToBasket(GameObject fruitObject, Sprite fruitSprite)
    {
        Vector3 startPos = fruitObject.transform.position;
        Vector3 startScale = fruitObject.transform.localScale;
        Vector3 endPos = basketTransform.position;
        Vector3 endScale = startScale * 0.3f;

        float elapsed = 0f;

        while (elapsed < fruitMoveAnimationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fruitMoveAnimationDuration;

            float smoothT = t * t * (3f - 2f * t);
            
            fruitObject.transform.position = Vector3.Lerp(startPos, endPos, smoothT);
            fruitObject.transform.localScale = Vector3.Lerp(startScale, endScale, smoothT);
            
            yield return null;
        }

        fruitObject.SetActive(false);

        AddFruitToBasketVisual(fruitSprite);

        if (basketTransform != null)
        {
            StartCoroutine(BasketBounceAnimation());
        }
    }

    void AddFruitToBasketVisual(Sprite fruitSprite)
    {
        if (fruitSlotsInBasket != null && fruitsInBasket.Count < fruitSlotsInBasket.Length)
        {
            Transform slot = fruitSlotsInBasket[fruitsInBasket.Count];
            
            if (fruitInBasketPrefab != null)
            {
                GameObject fruitInBasket = Instantiate(fruitInBasketPrefab, slot.position, Quaternion.identity, basketTransform);
                SpriteRenderer sr = fruitInBasket.GetComponent<SpriteRenderer>();
                if (sr != null && fruitSprite != null)
                {
                    sr.sprite = fruitSprite;
                }
                fruitsInBasket.Add(fruitInBasket);
            }
        }
    }

    System.Collections.IEnumerator BasketBounceAnimation()
    {
        Vector3 originalScale = basketTransform.localScale;
        Vector3 bounceScale = originalScale * collectAnimationScale;
        
        float duration = 0.15f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            basketTransform.localScale = Vector3.Lerp(originalScale, bounceScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            basketTransform.localScale = Vector3.Lerp(bounceScale, originalScale, elapsed / duration);
            yield return null;
        }

        basketTransform.localScale = originalScale;
    }

    public Sprite GetRandomFruitSprite()
    {
        if (fruitSprites != null && fruitSprites.Length > 0)
        {
            return fruitSprites[Random.Range(0, fruitSprites.Length)];
        }
        return null;
    }

    void UpdateUI()
    {
        if (counterDisplay != null)
        {
            counterDisplay.text = currentFruits.ToString() + " / " + targetFruits.ToString();
        }

        if (basketCounterText != null)
        {
            basketCounterText.text = currentFruits.ToString();
        }
    }

    void WinLevel()
    {
        levelCompleted = true;
        Debug.Log("Nivel Complet! Iepurasul e fericit.");

        if (winMessageObject)
            winMessageObject.SetActive(true);
    }

    void ShowWarning()
    {
        if (warningTextObject)
        {
            warningTextObject.SetActive(true);
            CancelInvoke("HideWarning"); 
            Invoke("HideWarning", 2.0f); 
        }
    }

    void HideWarning()
    {
        if (warningTextObject) warningTextObject.SetActive(false);
    }

    public void ResetLevel()
    {
        currentFruits = 0;
        levelCompleted = false;
        
        foreach (GameObject fruit in fruitsInBasket)
        {
            if (fruit != null)
                Destroy(fruit);
        }
        fruitsInBasket.Clear();
        
        UpdateUI();
        
        if (winMessageObject) winMessageObject.SetActive(false);
        if (warningTextObject) warningTextObject.SetActive(false);
    }
}