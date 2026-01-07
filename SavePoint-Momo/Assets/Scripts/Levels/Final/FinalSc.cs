using System.Collections.Generic;
using UnityEngine;

public class FinalSc : MonoBehaviour
{
    public AudioManager audioManager;
    void Start()
    {

        audioManager = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();
        StartCoroutine(audioManager.PlayAudioList(new List<string> { "final" }, () => { MainGameManager.play("Menu"); }));

    }
}
