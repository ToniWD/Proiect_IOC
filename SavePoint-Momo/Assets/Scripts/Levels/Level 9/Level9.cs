using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Level9 : MonoBehaviour
{
    public AudioManager audioManager;
    public GameObject greenLight;
    public GameObject redLight;

    public GameObject stayButton;
    public GameObject goButton;

    [Header("Character Animation Settings")]
    public SpriteRenderer characterRenderer; 
    public Sprite[] walkSprites; // Assign your 4 images here
    public Transform startCrosswalkPoint; 
    public Transform endCrosswalkPoint;   
    public Vector3 finalScale = new Vector3(0.5f, 0.5f, 1f); 
    public float animationSpeed = 0.15f; 
    public float movementDuration = 3.0f; 

    private float lastHoverTime; 
    public float hoverCooldown = 1f; 
    private bool isTransitioning = false; 

    void Start()
    {
        audioManager = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();

        // "Vrei să mă ajuți să trec în siguranță?"
        StartCoroutine(audioManager.PlayAudioList(new List<string> { "intro" },
            this.startGame
        ));
    }

    void startGame()
    {
        redLight.SetActive(true);
        greenLight.SetActive(false);
        // "Ce facem dacă e roșu? Așteptăm sau trecem?"
        StartCoroutine(audioManager.PlayAudioList(new List<string> { "01-instructiuni" },
            this.showButtons
        ));
    }

    void showButtons()
    {
        stayButton.SetActive(true);
        goButton.SetActive(true);
    }

    public void playGoButtonSound()
    {
        if (Time.time > lastHoverTime + hoverCooldown)
        {
            // "Trecem"
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "99-trecem" }));
            lastHoverTime = Time.time;
        }
    }

    public void playStayButtonSound()
    {
        // "Așteptăm"
        StartCoroutine(audioManager.PlayAudioList(new List<string> { "98-asteptam" }));
    }

    public void clickStayButton()
    {
        if (redLight.activeSelf && !isTransitioning) 
        {
            isTransitioning = true;
            // "Exact! La roșu stăm pe loc."
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "02-rosu-corect" }));
            
            // Wait 3 seconds then switch the light
            StartCoroutine(WaitAndSwitchLight(3f));
        }
        else if (greenLight.activeSelf)
        {
            // "Hai să alegem alt răspuns"
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "10-hai-sa-alegem-alt-raspuns" }));
        }
    }

    IEnumerator WaitAndSwitchLight(float delay)
    {
        yield return new WaitForSeconds(delay);
        redLight.SetActive(false);
        greenLight.SetActive(true);
        StartCoroutine(audioManager.PlayAudioList(new List<string> { "11-priveste-semaforul" }));
        isTransitioning = false;
    }

    public void clickGoButton()
    {
        if (greenLight.activeSelf)
        {
            stayButton.SetActive(false);
            goButton.SetActive(false);
            
            StartCoroutine(HandleWinningSequence());
        }
        else
        {
            playIncorrectGoSound();
        }
    }

    void playIncorrectGoSound()
    {
        // Keep the 4 incorrect sounds selection
        List<string> audios = new List<string> {
            "01-nu-trecem-pe-rosu",
            "02-nu-trecem-pe-rosu",
            "03-nu-trecem-pe-rosu",
            "04-nu-trecem-pe-rosu"
        };
        int randomIndex = UnityEngine.Random.Range(0, audios.Count);
        string selectedAudio = audios[randomIndex];

        StartCoroutine(audioManager.PlayAudioList(new List<string> { selectedAudio }));
    }
    
    IEnumerator HandleWinningSequence()
    {
        // Start movement and animation
        StartCoroutine(AnimateAndMoveCharacter());

        // "Bravo! La verde traversăm pe trecere."
        yield return StartCoroutine(audioManager.PlayAudioList(new List<string> { "04-verde-corect" }));

        // "Ai fost un ghid grozav! Am trecut strada în siguranță."
        yield return StartCoroutine(audioManager.PlayAudioList(new List<string> { "09-ai-fost-un-ghid-grozav" }));
    }

    IEnumerator AnimateAndMoveCharacter()
    {
        // Capture the original sprite and scale before we start
        Sprite originalSprite = characterRenderer.sprite;
        Vector3 initialScale = characterRenderer.transform.localScale;
    
        // Step 1: Disappear
        characterRenderer.enabled = false;
        yield return new WaitForSeconds(0.5f);

        // Step 2: Reappear at the crosswalk start
        characterRenderer.transform.position = startCrosswalkPoint.position;
        characterRenderer.enabled = true;

        // Step 3: Walk, Scale, and Cycle through the 4 images
        float elapsed = 0f;
        int spriteIndex = 0;
        float spriteTimer = 0f;

        while (elapsed < movementDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / movementDuration;

            // Move and Shrink
            characterRenderer.transform.position = Vector3.Lerp(startCrosswalkPoint.position, endCrosswalkPoint.position, t);
            characterRenderer.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);

            // Sprite Animation loop
            spriteTimer += Time.deltaTime;
            if (spriteTimer >= animationSpeed)
            {
                spriteIndex = (spriteIndex + 1) % walkSprites.Length;
                characterRenderer.sprite = walkSprites[spriteIndex];
                spriteTimer = 0f;
            }

            yield return null;
        }

        // Step 4: Final snap and RESET TO ORIGINAL SPRITE
        characterRenderer.transform.position = endCrosswalkPoint.position;
        characterRenderer.transform.localScale = finalScale;
        characterRenderer.sprite = originalSprite; // This sets Momo back to her idle pose
    }
}