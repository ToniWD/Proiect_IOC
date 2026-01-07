using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4 : MonoBehaviour
{
    public GameObject meniu;

    public AudioManager audioManager;

    public List<StringGameObjectPair> options;
    [Serializable]
    public struct StringGameObjectPair
    {
        public string key;
        public GameObject value;
    }


    private string rightOption = "";

    private GameObject oldOpt;
    private List<int> indices;
    private int index = 0;

    private void Start()
    {
        audioManager = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();

        indices = new List<int>();
        for (int i = 0; i < options.Count; i++)
            indices.Add(i);

        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int temp = indices[i];
            indices[i] = indices[j];
            indices[j] = temp;
        }

        StartCoroutine(startLevel());
    }

    IEnumerator startLevel()
    {
        var clip1 = audioManager.GetAudio("intro");
        audioManager.audioSource.PlayOneShot(clip1);
        yield return new WaitForSeconds(clip1.length);

        clip1 = audioManager.GetAudio("inst");
        audioManager.audioSource.PlayOneShot(clip1);
        yield return new WaitForSeconds(clip1.length);


        play();
    }

    private void play()
    {
        if (index < indices.Count) {
            Debug.Log("Play - " + options[indices[index]].key);
            playSoundfor(options[indices[index]].key);
        }
        else
        {
            Debug.Log("Finish -------------------------------------");
            StartCoroutine(finishLevel());
            
        }
        index++;
    }

    IEnumerator finishLevel()
    {
        var clip1 = audioManager.GetAudio("final");
        audioManager.audioSource.PlayOneShot(clip1);
        yield return new WaitForSeconds(clip1.length);

        clip1 = audioManager.GetAudio("continua");
        audioManager.audioSource.PlayOneShot(clip1);
        yield return new WaitForSeconds(clip1.length);


        MainGameManager.play("Level 5");
    }

    private void playSoundfor(string name)
    {
        switch (name)
        {
            case "bufnita":
                rightOption = "bufnita";
                break;
            case "lup":
                rightOption = "lup";
                break;
            case "urs":
                rightOption = "urs";
                break;
            case "pitigoi":
                rightOption = "pitigoi";
                break;
            default:
                Debug.LogError("Sunetul nu exista");
                return;
        }

        rightOption = name;

        if(rightOption == "urs") audioManager.audioSource.PlayOneShot(audioManager.GetAudio(rightOption), 0.05f);
        else audioManager.audioSource.PlayOneShot(audioManager.GetAudio(rightOption));
    }

    public void playAgain()
    {
        if (rightOption.Length > 0)
        {
            if (rightOption == "urs") audioManager.audioSource.PlayOneShot(audioManager.GetAudio(rightOption), 0.05f);
            else audioManager.audioSource.PlayOneShot(audioManager.GetAudio(rightOption));
        }
    }

    private int countGresit = 0;
    public void choseOption(string option)
    {
        if(rightOption == option)
        {
            Debug.Log("Corect");
            meniu.SetActive(false);

            if (rightOption == "urs") StartCoroutine(PlayAudioList(new List<string> { rightOption + " fact" }, "restartMenu"));
            else StartCoroutine(PlayAudioList(new List<string> { rightOption, rightOption + " fact" }, "restartMenu"));

            foreach (var pair in options)
            {
                if(pair.key == option)
                {
                    pair.value.SetActive(true);
                    oldOpt = pair.value;
                    return;
                }
            }
        }
        else
        {
            Debug.Log("Gresit");
            if(audioManager.audioSource.isPlaying)return;
            if (countGresit >= 1)
            {
                StartCoroutine(PlayAudioList(new List<string> { "gresit 1" }, "playAgain"));
                countGresit = 0;
            }
            else
            {
                audioManager.audioSource.PlayOneShot(audioManager.GetAudio("gresit 2"));
                countGresit++;
            }
        }
    }

    IEnumerator PlayAudioList(List<string> list, string invoke = "")
    {
        if (audioManager.audioSource.isPlaying) yield return null;
        foreach (var item in list)
        {
            var clip1 = audioManager.GetAudio(item);
            audioManager.audioSource.PlayOneShot(clip1);
            yield return new WaitForSeconds(clip1.length);
        }

        if(invoke.Length > 0) Invoke(invoke, 2f);
    }

    public void restartMenu()
    {
        oldOpt.SetActive(false);
        oldOpt = null;
        rightOption = "";
        meniu.SetActive(true);
        play();
    }

}
