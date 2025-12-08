using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class Level3 : MonoBehaviour
{
    public static int count = 5;
    public AudioManager audioManager;

    public GameObject puzzle;

    private void Start()
    {
        count = 5;
        audioManager = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();

        StartCoroutine(PlayClipsSequentially());
    }

    IEnumerator PlayClipsSequentially()
    {
        var clip1 = audioManager.GetAudio("intro");
        audioManager.audioSource.PlayOneShot(clip1);
        yield return new WaitForSeconds(clip1.length);
        puzzle.SetActive(true);
        audioManager.audioSource.PlayOneShot(audioManager.GetAudio("instr 0"));
    }

    public static void removePuzzlePiece()
    {
        count--;
        if (count <= 0) GameObject.Find("Puzzle").GetComponent<Level3>().levelComplited();
    }

    private void levelComplited()
    {
        Debug.Log("Final nivel---------------------");

        StartCoroutine(PlayFinalAndLoadNext());
    }

    private IEnumerator PlayFinalAndLoadNext()
    {
        AudioClip clip = audioManager.GetAudio("final");

        if (clip != null)
        {
            audioManager.audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }

        clip = audioManager.GetAudio("final hai sa continuam");

        if (clip != null)
        {
            audioManager.audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
        MainGameManager.play("Level 4");
    }

}
