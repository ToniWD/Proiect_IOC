using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level5 : MonoBehaviour
{
    public int count = 0;
    public int countStop = 5;

    public TMP_Text score;

    public GameObject part1;
    public GameObject part2;


    public GameObject Outcome1;
    public GameObject Outcome2;
    public GameObject button1;
    public GameObject button2;

    public GameObject final;

    public AudioManager audioManger;

    private void Start()
    {
        audioManger = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();

        StartCoroutine(audioManger.PlayAudioList(new List<string> { "intro", "intro optiuni 1" },
            this.startFirstPhase
        ));
    }

    public void startFirstPhase()
    {
        button1.SetActive(true);
        StartCoroutine(audioManger.PlayAudioList(new List<string> { "intro optiuni 2" }, ()=>{ button2.SetActive(true); audioManger.PlaySingleAfterCurrent(audioManger.GetAudio("inst part 1")); }));
    }

    public void buttonsSound(AudioClip audio)
    {

        if (!audioManger.audioSource.isPlaying)
        {
            audioManger.PlaySingle(audio);
        }
    }

    public void choice1()
    {
        Debug.Log("Imbratisare"); 
        part1.SetActive(false);
        Outcome1.SetActive(true);
        StartCoroutine(audioManger.PlayAudioList(new List<string> { "final part 1" },
            () => { Invoke("startSecondPhase", 1.5f); }
        ));
    }

    public void choice2()
    {
        Debug.Log("Jucarie");
        part1.SetActive(false);
        Outcome2.SetActive(true);
        StartCoroutine(audioManger.PlayAudioList(new List<string> { "final part 1" },
            () => { Invoke("startSecondPhase",1.5f); }
        ));
    }


    public void startSecondPhase()
    {
        Outcome1.SetActive(false);
        Outcome2.SetActive(false);
        part1.SetActive(false);
        part2.SetActive(true);

        StartCoroutine(audioManger.PlayAudioList(new List<string> { "intro part 2", "inst part 2" },
            () => { Clickable.startGame = true; }
        ));
    }

    public int found(int i, GameObject obj)
    {
        if (i != 1)
        {
            error();
            return 0;
        }
        count++;

        score.text = "Fructe: " + count;

        if (count == countStop)
        {
            Clickable.startGame = false;

            part2.SetActive(false);
            final.SetActive(true);
            Debug.Log("Finish---------------------");

            StartCoroutine(audioManger.PlayAudioList(new List<string> { "final part 2", "final part 2.2" },
            () => { Invoke("finish", 1.5f); }
            ));

            return 1;
        }

        return -1;
    }

    private void finish()
    {
        MainGameManager.play("Level 6");
    }

    public void error()
    {
        Debug.Log("Fruct gresit");
    }
}
