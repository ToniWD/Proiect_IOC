using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.Build;

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
    public AudioClip nrFructe;
    public AudioClip nrFructeSimple;
    public AudioClip IntroducereInPoveste2;
    public AudioClip InstructiuniDeJoc1;
    public AudioClip InstructiuniDeJoc2;
    public AudioClip FeedbackPozitiv1;
    public AudioClip FeedbackPozitiv2pt1;
    public AudioClip FeedbackPozitiv2pt2;
    public AudioClip LevelCompletionpt1;
    public AudioClip LevelCompletionpt2;
    public AudioClip FeedbackNegativ1;
    public AudioClip FeedbackNegativ2;

    [Header("Obiecte de afișat după intro")]
    public GameObject basketRoot;   // un Empty care conține toate sprite-urile de coș
    public GameObject fruitsRoot;   // un Empty care conține toate fructele
    public GameObject doneButton;
    public GameObject basket2;
    public GameObject basket1;

    [Header("Iepuras")]
    public SpriteRenderer iepuras;
    public Sprite iepuras_hello;
    public Sprite iepuras_happy;
    [Header("Momo")]
    public SpriteRenderer momo;
    public Sprite momoBasic;
   



    [Header("Resurse Audio - FX")]
    public AudioClip fruitSelectSound;
    public AudioClip errorSound;
    public AudioClip basketFullSound;

    private List<GameObject> fruitsInBasket = new List<GameObject>();
    public static FruitCollectorManager Instance;

    private AudioSource momoAudioSource;

    private Coroutine currentSpeechCoroutine;


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

        if (basketRoot) basketRoot.SetActive(false);
        if (fruitsRoot) fruitsRoot.SetActive(false);
        if (doneButton) doneButton.SetActive(false);
        if (basket2) basket2.SetActive(false);
        if (basket1) basket1.SetActive(true);

        StartIntroSequence();
    }


    void StartIntroSequence()
    {
        isIntroPlaying = true;
        PlayMomoReplica(IntroducereInPoveste1, () =>
            PlayMomoReplica(IntroducereInPoveste2, () =>
                
                {
                    isIntroPlaying = false;
                    StartGameVisuals(); // ⬅️ aici se schimbă fundalul + apar coșul și fructele
                }));
    }



    void StartGameVisuals()
    {
        // schimbă fundalul
        if (backgroundRenderer != null && backgroundSearchingSprite != null)
        {
            backgroundRenderer.sprite = backgroundSearchingSprite;
        }

        // afișează coșul și fructele
        if (basketRoot) basketRoot.SetActive(true);
        if (fruitsRoot) fruitsRoot.SetActive(true);
        if (doneButton) { doneButton.SetActive(true); }
        if(iepuras) iepuras.enabled = false;
        if(momo) momo.enabled = false;
        if (basket2) basket2.SetActive(true);
        if (basket1) basket1.SetActive(false);
        PlayMomoReplica(InstructiuniDeJoc1, () =>
                PlayMomoReplica(nrFructeSimple, () =>
                PlayMomoReplica((InstructiuniDeJoc2))));
    }


    public void OnDoneButtonPressed()
    {
        if (levelCompleted)
            return; // dacă ai câștigat deja, ignoră apăsările

        if (currentFruits == targetFruits)
        {
            // exact câte trebuie → win
            WinLevel();
        }
        else if (currentFruits > targetFruits)
        {
            // prea multe
            PlayMomoReplica(FeedbackNegativ1);
            ShowWarning();   // dacă vrei să apară și text de warning
        }
        else // currentFruits < targetFruits
        {
            // prea puține
            PlayMomoReplica(FeedbackNegativ2,()=>PlayMomoReplica(nrFructe));
            // aici poți lăsa copilul să mai adauge fructe
        }
    }


    private void PlayPositiveFeedback2Sequence()
    {
        // Redă mai întâi prima parte
        PlayMomoReplica(FeedbackPozitiv2pt1, () =>
        {
            // Când prima parte s-a terminat, redă a doua parte
            PlayMomoReplica(FeedbackPozitiv2pt2);
        });
    }

    private void PlayLevlComplectionSequence()
    {
        PlayMomoReplica(LevelCompletionpt1,()=>
        PlayMomoReplica(nrFructe,()=>PlayMomoReplica((LevelCompletionpt2))));

    }

    private void PlayMomoReplica(AudioClip clip, System.Action onComplete = null)
    {
        if (clip == null || momoAudioSource == null)
        {
            onComplete?.Invoke();
            return;
        }

        // Oprește orice corutină care aștepta terminația replicii anterioare
        if (currentSpeechCoroutine != null)
        {
            StopCoroutine(currentSpeechCoroutine);
            currentSpeechCoroutine = null;
        }

        // Oprește orice replică audio anterioară
        if (momoAudioSource.isPlaying)
        {
            momoAudioSource.Stop();
        }

        // Redă noul clip
        momoAudioSource.clip = clip;
        momoAudioSource.Play();

        // Pornește o nouă corutină care va apela callback după ce clipul se termină
        if (onComplete != null)
        {
            currentSpeechCoroutine = StartCoroutine(WaitAndExecute(clip.length, () =>
            {
                currentSpeechCoroutine = null;
                onComplete?.Invoke();
            }));
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
        if (levelCompleted)
            return; // dacă nivelul e deja câștigat, nu mai colectăm

        // Feedback pozitiv la fiecare fruct colectat
      
   

        PlayFXSound(fruitSelectSound);

        currentFruits++;
        if (currentFruits <= targetFruits)
        {
            if (nextFeedbackIsPositive1)
            {
                PlayMomoReplica(FeedbackPozitiv1);
            }
            else
            {
                PlayPositiveFeedback2Sequence(); // compus din 2 bucăți
            }
            nextFeedbackIsPositive1 = !nextFeedbackIsPositive1;
        }
        else
        {
            PlayMomoReplica(FeedbackNegativ1);
        }

            SpriteRenderer fruitSpriteRenderer = fruitObject.GetComponent<SpriteRenderer>();
        Sprite collectedFruitSprite = fruitSpriteRenderer != null ? fruitSpriteRenderer.sprite : null;

        AnimateFruitToBasket(fruitObject, collectedFruitSprite);

        UpdateUI();

   
    }



    public void RemoveFruitFromBasket(GameObject fruitObject)
    {
        if (fruitsInBasket.Contains(fruitObject))
        {
            if (currentFruits > 0)
            {
                currentFruits--;
                fruitsInBasket.Remove(fruitObject);

                ClickableFruit cf = fruitObject.GetComponent<ClickableFruit>();
                if (cf != null)
                {
                    cf.ResetFruit();
                }

                UpdateUI();
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

        if (backgroundRenderer != null && backgroundSearchingSprite != null)
        {
            backgroundRenderer.sprite = backgroundMainSprite;
        }

        // afișează coșul și fructele
        if (basketRoot) basketRoot.SetActive(false);
        if (fruitsRoot) fruitsRoot.SetActive(false);
        if (doneButton) { doneButton.SetActive(false); }
        if (iepuras)
        {
            iepuras.sprite = iepuras_happy;
            iepuras.enabled = true; 
        }
        if (momo) momo.enabled = true;

        if (basket2) basket2.SetActive(false);
        if (basket1) basket1.SetActive(false);
         PlayLevlComplectionSequence();
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