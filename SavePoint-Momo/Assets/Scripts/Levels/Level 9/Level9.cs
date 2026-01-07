using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class Level9 : MonoBehaviour
{
    public AudioManager audioManager;
    public GameObject greenLight;
    public GameObject redLight;

    public GameObject stayButton;
    public GameObject goButton;

    private float lastHoverTime; // Stores the timestamp of the last hover
    public float hoverCooldown = 1f; // Delay in seconds (adjust this as you like)
    
    void Update() { }
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
        if (redLight.activeSelf) 
        {
            // "Exact! La roșu stăm pe loc."
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "02-rosu-corect" }));
            
            // Trigger the light change after 2 seconds
            Invoke("SwitchToGreen", 2f);
        }
        else
        {
            // If they stay when it's green, prompt to try again
            // "Hai să alegem alt răspuns"
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "10-hai-sa-alegem-alt-raspuns" }));
        }
    }

    void SwitchToGreen()
    {
        redLight.SetActive(false);
        greenLight.SetActive(true);
    }

    public void clickGoButton()
    {
        if (greenLight.activeSelf)
        {
            // "Bravo! La verde traversăm pe trecere."
            // "Ai fost un ghid grozav! Am trecut strada în siguranță."
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "04-verde-corect", "09-ai-fost-un-ghid-grozav" }));
            
            // Disable buttons after success
            stayButton.SetActive(false);
            goButton.SetActive(false);
        }
        else
        {
            // "Nu trecem niciodată fără să ne uităm"
            playIncorrectGoSound();
        }
    }

    void playIncorrectGoSound()
    {
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
}
