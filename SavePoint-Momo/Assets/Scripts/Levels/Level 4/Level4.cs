using System;
using System.Collections.Generic;
using UnityEngine;

public class Level4 : MonoBehaviour
{
    public GameObject meniu;

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
        }
        index++;
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
    }

    public void choseOption(string option)
    {
        if(rightOption == option)
        {
            Debug.Log("Corect");
            meniu.SetActive(false);
            foreach (var pair in options)
            {
                if(pair.key == option)
                {
                    pair.value.SetActive(true);
                    oldOpt = pair.value;
                    Invoke("restMenu", 8f);
                    return;
                }
            }
        }
        else
        {
            Debug.Log("Gresit");
        }
    }

    public void restMenu()
    {
        oldOpt.SetActive(false);
        oldOpt = null;
        rightOption = "";
        meniu.SetActive(true);
        play();
    }

}
