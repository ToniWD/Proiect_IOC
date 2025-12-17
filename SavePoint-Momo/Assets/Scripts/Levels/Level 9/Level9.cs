using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Level9 : MonoBehaviour
{

    public AudioManager audioManager;
    public GameObject greenLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // TODO: hit Adi for icons for buttons (Trecem / Stam);
    void Start()
    {
        audioManager = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();

        StartCoroutine(audioManager.PlayAudioList(new List<string> { "intro" },
            this.startGame
        ));
    }

    void startGame()
    {
        greenLight.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clickTrecem()
    {
        Debug.Log("Trecem");

        if (greenLight.activeSelf)
        {
            Debug.Log("Bravo");
        }
    }

    public void clickStam()
    {
        Debug.Log("Stam");
    }
}
