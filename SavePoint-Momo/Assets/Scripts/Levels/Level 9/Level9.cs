using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required to use the Button class
using System.Collections.Generic;

public class Level9 : MonoBehaviour
{
    public AudioManager audioManager;
    public GameObject greenLight;
    public GameObject redLight;

    // Changed to Button type to access the interactable property
    public Button stayButton; 
    public Button goButton;

    [Header("Character Animation Settings")]
    public SpriteRenderer characterRenderer; 
    public Sprite[] walkSprites; 
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

        // Lock buttons during initial intro
        StartCoroutine(DisableButtonsTemporarily(10f)); 

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

    void showButtons() {
        stayButton.interactable = true;
        goButton.interactable = true;
    }

    // Helper method to lock buttons for a specific time
    IEnumerator DisableButtonsTemporarily(float duration)
    {
        stayButton.interactable = false;
        goButton.interactable = false;
        yield return new WaitForSeconds(duration);
        stayButton.interactable = true;
        goButton.interactable = true;
    }

    public void clickStayButton()
    {
        if (redLight.activeSelf && !isTransitioning) 
        {
            isTransitioning = true;
            
            // Lock buttons for 1 second
            StartCoroutine(DisableButtonsTemporarily(3f)); 

            // "Exact! La roșu stăm pe loc."
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "02-rosu-corect" }));
            
            StartCoroutine(WaitAndSwitchLight(3f));
        }
        else if (greenLight.activeSelf)
        {
            StartCoroutine(DisableButtonsTemporarily(1.5f));
            // "Hai să alegem alt răspuns"
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "10-hai-sa-alegem-alt-raspuns" }));
        }
    }

    IEnumerator WaitAndSwitchLight(float delay)
    {
        yield return new WaitForSeconds(delay);
        redLight.SetActive(false);
        greenLight.SetActive(true);
        // "Priveste semaforul"
        StartCoroutine(audioManager.PlayAudioList(new List<string> { "11-priveste-semaforul" }));
        isTransitioning = false;
    }

    public void clickGoButton()
    {
        if (greenLight.activeSelf)
        {
            // Fully disable for the end of the level
            stayButton.interactable = false;
            goButton.interactable = false;
            
            StartCoroutine(HandleWinningSequence());
        }
        else
        {
            StartCoroutine(DisableButtonsTemporarily(3f));
            playIncorrectGoSound();
        }
    }

    void playIncorrectGoSound()
    {
        List<string> audios = new List<string> {
            "01-nu-trecem-pe-rosu", "02-nu-trecem-pe-rosu",
            "03-nu-trecem-pe-rosu", "04-nu-trecem-pe-rosu"
        };
        int randomIndex = UnityEngine.Random.Range(0, audios.Count);
        StartCoroutine(audioManager.PlayAudioList(new List<string> { audios[randomIndex] }));
    }
    
    IEnumerator HandleWinningSequence()
    {
        StartCoroutine(AnimateAndMoveCharacter());

        // "Bravo! La verde traversăm pe trecere."
        yield return StartCoroutine(audioManager.PlayAudioList(new List<string> { "04-verde-corect" }));

        // "Ai fost un ghid grozav! Am trecut strada în siguranță."
        yield return StartCoroutine(audioManager.PlayAudioList(new List<string> { "09-ai-fost-un-ghid-grozav" }, () => { MainGameManager.play("Final"); }));
    }

    IEnumerator AnimateAndMoveCharacter()
    {
        Sprite originalSprite = characterRenderer.sprite; // Remember the original pose
        Vector3 initialScale = characterRenderer.transform.localScale;
    
        characterRenderer.enabled = false;
        yield return new WaitForSeconds(0.5f);

        characterRenderer.transform.position = startCrosswalkPoint.position;
        characterRenderer.enabled = true;

        float elapsed = 0f;
        int spriteIndex = 0;
        float spriteTimer = 0f;

        while (elapsed < movementDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / movementDuration;

            characterRenderer.transform.position = Vector3.Lerp(startCrosswalkPoint.position, endCrosswalkPoint.position, t);
            characterRenderer.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);

            spriteTimer += Time.deltaTime;
            if (spriteTimer >= animationSpeed)
            {
                spriteIndex = (spriteIndex + 1) % walkSprites.Length;
                characterRenderer.sprite = walkSprites[spriteIndex];
                spriteTimer = 0f;
            }
            yield return null;
        }

        characterRenderer.transform.position = endCrosswalkPoint.position;
        characterRenderer.transform.localScale = finalScale;
        characterRenderer.sprite = originalSprite; // Return to idle pose
    }

    // Protect hover sounds so they don't play if buttons are locked
    public void playGoButtonSound()
    {
        if (goButton.interactable && Time.time > lastHoverTime + hoverCooldown)
        {
            // "Trecem"
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "99-trecem" }));
            lastHoverTime = Time.time;
        }
    }

    public void playStayButtonSound()
    {
        if (stayButton.interactable)
        {
            // "Așteptăm"
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "98-asteptam" }));
        }
    }
}