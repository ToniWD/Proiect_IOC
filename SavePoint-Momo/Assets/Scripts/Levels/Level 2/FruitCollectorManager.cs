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

    // AICI ESTE LISTA PENTRU LOCURILE LIBERE
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
            backgroundRenderer.sprite = backgroundMainSprite;

        if (basketBackRenderer != null && basketBackSprite != null)
            basketBackRenderer.sprite = basketBackSprite;

        if (basketFrontRenderer != null && basketFrontSprite != null)
            basketFrontRenderer.sprite = basketFrontSprite;

        if (basketWithPawRenderer != null && basketWithPawSprite != null)
            basketWithPawRenderer.sprite = basketWithPawSprite;
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
        // Calculam indexul (daca e primul fruct, index 0, etc.)
        int slotIndex = currentFruits - 1;

        // Verificam daca avem sloturi definite in Inspector
        if (basketTransform != null && fruitSlotsInBasket != null && slotIndex < fruitSlotsInBasket.Length)
        {
            Transform targetSlot = fruitSlotsInBasket[slotIndex];
            StartCoroutine(MoveFruitToBasket(fruitObject, targetSlot));
        }
        else
        {
            // Daca nu ai setat sloturile, fructul dispare (fallback)
            fruitObject.SetActive(false);
        }
    }

    System.Collections.IEnumerator MoveFruitToBasket(GameObject fruitObject, Transform targetSlot)
    {
        Vector3 startPos = fruitObject.transform.position;
        Vector3 startScale = fruitObject.transform.localScale;

        Vector3 endPos = targetSlot.position;

        // Fructul devine putin mai mic in cos (0.7 din marimea originala)
        Vector3 endScale = startScale * 0.7f;

        // Oprim click-ul pe fruct ca sa nu il poti lua de doua ori
        if (fruitObject.GetComponent<Collider2D>() != null)
        {
            fruitObject.GetComponent<Collider2D>().enabled = false;
        }

        // Schimbam ordinea: Stratul 1 este intre Spate (0) si Fata (2)
        if (fruitObject.GetComponent<SpriteRenderer>() != null)
        {
            fruitObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        float elapsed = 0f;

        while (elapsed < fruitMoveAnimationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fruitMoveAnimationDuration;

            // Miscare lina (Smooth Step)
            float smoothT = t * t * (3f - 2f * t);

            fruitObject.transform.position = Vector3.Lerp(startPos, endPos, smoothT);
            fruitObject.transform.localScale = Vector3.Lerp(startScale, endScale, smoothT);

            yield return null;
        }

        // Fixam pozitia finala
        fruitObject.transform.position = endPos;
        fruitObject.transform.localScale = endScale;

        // Facem fructul copil al cosului (se misca odata cu cosul)
        fruitObject.transform.SetParent(basketTransform);

        // Adaugam in lista interna
        fruitsInBasket.Add(fruitObject);

        // Animam cosul (salt mic)
        if (basketTransform != null)
        {
            StartCoroutine(BasketBounceAnimation());
        }
    }

    // --- FUNCTIILE CARE LIPSEAU DIN CODUL TAU ---

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
                Destroy(fruit); // Sau fruit.SetActive(true) si resetat pozitia daca vrei reciclare
        }
        fruitsInBasket.Clear();

        UpdateUI();

        if (winMessageObject) winMessageObject.SetActive(false);
        if (warningTextObject) warningTextObject.SetActive(false);
    }
}