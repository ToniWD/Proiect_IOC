using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6 : MonoBehaviour
{
    public static int count = 5;
    public AudioManager audioManager;

    public GameObject puzzle;

    public static int currentPiece = 0;

    private void Start()
    {
        count = 5;
        currentPiece = 0;
        audioManager = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();

        StartCoroutine(PlayClipsSequentially());
    }

    IEnumerator PlayClipsSequentially()
    {
        var clip1 = audioManager.GetAudio("intro");
        audioManager.audioSource.PlayOneShot(clip1);
        yield return new WaitForSeconds(clip1.length);
        puzzle.SetActive(true);
        audioManager.audioSource.PlayOneShot(audioManager.GetAudio("instr"));
    }

    public static void removePuzzlePiece()
    {
        count--;
        currentPiece++;
        if (count <= 0) GameObject.Find("Puzzle").GetComponent<Level6>().levelComplited();
    }

    private void levelComplited()
    {
        Debug.Log("Final nivel---------------------");

        StartCoroutine(audioManager.PlayAudioList(new List<string> { "final", "final hai sa continuam" }, () => { MainGameManager.play("Menu"); }));
    }

}
