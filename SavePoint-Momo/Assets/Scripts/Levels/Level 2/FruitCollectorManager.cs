using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FruitCollectorManager : MonoBehaviour
{
    [Header("Configurare Nivel")]
    public int targetFruits = 5;
    public int currentFruits = 0;
    public bool levelCompleted = false;

    private bool nextFeedbackIsPositive1 = true;
    private bool isIntroPlaying = false;

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

    [Header("Resurse Audio - Replici Momo")]
    public AudioClip IntroducereInPoveste1;
    public AudioClip IntroducereInPoveste2;
    public AudioClip InstructiuniDeJoc1;
    public AudioClip InstructiuniDeJoc2;
    public AudioClip FeedbackPozitiv1;
    public AudioClip FeedbackPozitiv2;
    public AudioClip LevelCompletion;
    public AudioClip FeedbackNegativ1;
    public AudioClip FeedbackNegativ2;

    [Header("Resurse Audio - FX")]
    public AudioClip fruitSelectSound;
    public AudioClip errorSound;
    public AudioClip basketFullSound;

    private List<GameObject> fruitsInBasket = new List<GameObject>();
    public static FruitCollectorManager Instance;

    private AudioSource momoAudioSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        momoAudioSource = GetComponent<AudioSource>();

        if (momoAudioSource == null)
        {
            Debug.LogError("Componenta AudioSource lipsește de pe GameObject-ul FruitCollectorManager!");
        }
    }

    void Start()
    {
        SetupVisuals();
        UpdateUI();
        if (winMessageObject) winMessageObject.SetActive(false);
        if (warningTextObject) warningTextObject.SetActive(false);

        StartIntroSequence();
    }

    void StartIntroSequence()
    {
        isIntroPlaying = true;
        PlayMomoReplica(IntroducereInPoveste1, () =>
            PlayMomoReplica(IntroducereInPoveste2, () =>
                PlayMomoReplica(InstructiuniDeJoc1, () =>
                {
                    isIntroPlaying = false;
                })));
    }


    private void PlayMomoReplica(AudioClip clip, System.Action onComplete = null)
    {
        if (momoAudioSource != null && clip != null)
        {
            if (momoAudioSource.isPlaying)
            {
                if (isIntroPlaying && (clip != IntroducereInPoveste1 && clip != IntroducereInPoveste2 && clip != InstructiuniDeJoc1))
                {
                    momoAudioSource.Stop();
                    isIntroPlaying = false; 
                }
            }

            momoAudioSource.clip = clip;
            momoAudioSource.Play();

            if (onComplete != null && !isIntroPlaying)
            {
                StartCoroutine(WaitAndExecute(clip.length, onComplete));
            }
        }
        else
        {
            onComplete?.Invoke();
        }
    }

    void PlayFXSound(AudioClip clip)
    {
        if (momoAudioSource != null && clip != null)
        {
            momoAudioSource.PlayOneShot(clip);
        }
    }

    System.Collections.IEnumerator WaitAndExecute(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
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
        if (levelCompleted && currentFruits >= targetFruits)
        {
        }

        if (currentFruits >= targetFruits)
        {
            if (currentFruits == targetFruits)
            {
                PlayMomoReplica(FeedbackNegativ1);
                ShowWarning();
            }
        }
        else
        {
            AudioClip feedbackClip = nextFeedbackIsPositive1 ? FeedbackPozitiv1 : FeedbackPozitiv2;
            PlayMomoReplica(feedbackClip);
            nextFeedbackIsPositive1 = !nextFeedbackIsPositive1;
        }

        PlayFXSound(fruitSelectSound);

        currentFruits++;

        SpriteRenderer fruitSpriteRenderer = fruitObject.GetComponent<SpriteRenderer>();
        Sprite collectedFruitSprite = fruitSpriteRenderer != null ? fruitSpriteRenderer.sprite : null;

        AnimateFruitToBasket(fruitObject, collectedFruitSprite);

        UpdateUI();

        if (currentFruits == targetFruits && !levelCompleted)
        {
            WinLevel();
        }
    }

    public void RemoveFruitFromBasket(GameObject fruitObject)
    {
        if (fruitsInBasket.Contains(fruitObject))
        {
            if (currentFruits > 0)
            {
                int scoreBeforeRemoval = currentFruits;

                ClickableFruit cf = fruitObject.GetComponent<ClickableFruit>();

                currentFruits--;
                fruitsInBasket.Remove(fruitObject);

                if (cf != null)
                {
                    cf.ResetFruit();
                }

                UpdateUI(); 

                if (scoreBeforeRemoval > targetFruits && currentFruits == targetFruits)
                {
                    WinLevel();
                }
                else if (currentFruits < targetFruits)
                {
                    PlayMomoReplica(FeedbackNegativ2);
                    if (levelCompleted)
                    {
                        levelCompleted = false;
                        if (winMessageObject) winMessageObject.SetActive(false);
                    }
                }

                ReorganizeBasket();
            }
        }
    }

    void ReorganizeBasket()
    {
        for (int i = 0; i < fruitsInBasket.Count; i++)
        {
            GameObject fruit = fruitsInBasket[i];
            if (i < fruitSlotsInBasket.Length)
            {
                fruit.transform.position = fruitSlotsInBasket[i].position;
            }
            ClickableFruit cf = fruit.GetComponent<ClickableFruit>();
            if (cf != null)
            {
                cf.SetCollectedInBasket();
            }
        }
    }

    void AnimateFruitToBasket(GameObject fruitObject, Sprite fruitSprite)
    {
        int slotIndex = currentFruits - 1;

        if (basketTransform != null && fruitSlotsInBasket != null && slotIndex < fruitSlotsInBasket.Length)
        {
            Transform targetSlot = fruitSlotsInBasket[slotIndex];
            StartCoroutine(MoveFruitToBasket(fruitObject, targetSlot));
        }
        else
        {
            fruitObject.SetActive(false);
        }
    }

    System.Collections.IEnumerator MoveFruitToBasket(GameObject fruitObject, Transform targetSlot)
    {
        Vector3 startPos = fruitObject.transform.position;
        Vector3 startScale = fruitObject.transform.localScale;
        Vector3 endPos = targetSlot.position;
        Vector3 endScale = startScale * 0.7f;

        Collider2D fruitCollider = fruitObject.GetComponent<Collider2D>();

        if (fruitCollider != null)
        {
            fruitCollider.enabled = false;
        }

        if (fruitObject.GetComponent<SpriteRenderer>() != null)
        {
            fruitObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

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

        fruitObject.transform.position = endPos;
        fruitObject.transform.localScale = endScale;

        fruitObject.transform.SetParent(basketTransform);
        fruitsInBasket.Add(fruitObject);

        if (fruitCollider != null)
        {
            fruitCollider.enabled = true;
        }

        ClickableFruit cf = fruitObject.GetComponent<ClickableFruit>();
        if (cf != null)
        {
            cf.SetCollectedInBasket();
        }

        if (basketTransform != null)
        {
            StartCoroutine(BasketBounceAnimation());
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

            if (currentFruits > targetFruits)
            {
                basketCounterText.color = Color.red;
            }
            else
            {
                basketCounterText.color = Color.white;
            }
        }
    }

    void WinLevel()
    {
        levelCompleted = true;

        PlayFXSound(basketFullSound);

        PlayMomoReplica(LevelCompletion, () => {
            Debug.Log("Nivel Complet! Iepurasul e fericit.");
            if (winMessageObject) winMessageObject.SetActive(true);
        });
    }

    void ShowWarning()
    {
        if (warningTextObject)
        {
            warningTextObject.SetActive(true);
            CancelInvoke("HideWarning");
            Invoke("HideWarning", 3.0f);
            PlayFXSound(errorSound);
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
            ClickableFruit cf = fruit.GetComponent<ClickableFruit>();
            if (cf != null)
            {
                cf.ResetFruit();
            }
        }
        fruitsInBasket.Clear();

        UpdateUI();

        if (winMessageObject) winMessageObject.SetActive(false);
        if (warningTextObject) warningTextObject.SetActive(false);
    }
}