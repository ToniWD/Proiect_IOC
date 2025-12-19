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
    void Start()
    {
        audioManager = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();

        StartCoroutine(audioManager.PlayAudioList(new List<string> { "intro" },
            this.startGame
        ));
    }

    void Update() { }

    void startGame()
    {
        redLight.SetActive(true);
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
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "99-trecem" }));
            lastHoverTime = Time.time;
        }
    }

    public void playStayButtonSound()
    {
        StartCoroutine(audioManager.PlayAudioList(new List<string> { "98-asteptam" }));
    }

    public void clickStayButton()
    {
        if (redLight.activeSelf)
        {
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "02-rosu-corect" }));
        }
        else
        {
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "03-rosu-gresit" }));
        }
    }

    public void clickGoButton()
    {

        if (greenLight.activeSelf)
        {
            StartCoroutine(audioManager.PlayAudioList(new List<string> { "04-verde-corect" }));
        }
        else
        {
            // Red is active. So it's a NO GO
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
