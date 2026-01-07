using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Level7 : MonoBehaviour
{
    public string currentAnimal;
    public string currentSign;

    public int nrAnimals = 3;
    private ClickGameObject animal;
    private ClickGameObject sign;
    public Transform cam;
    public List<Vector3> pozs;

    public AudioManager audioManager;
    void Start()
    {

        audioManager = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();
        StartCoroutine(audioManager.PlayAudioList(new List<string> { "intro", "instr" }, () => { ClickGameObject.stop = false; }));
    }

    public void SelectSign(string value, ClickGameObject currentSc)
    {
        if (value == currentSign) return;
        currentSign = value;
        if (sign != null) sign.Deselect();

        sign = currentSc;

        if (currentSign == currentAnimal)
        {
            Debug.Log("bun");

            switch(currentAnimal)
            {
                case "fox":
                    cam.position = pozs[3];
                        break;
                case "sheep":
                    cam.position = pozs[1];
                        break;
                case "swan":
                    cam.position = pozs[2];
                        break;
            }
            string msg = "corect";
            if (Random.value > 0.5f)
            {
                msg = "corect 2";
            }
            StartCoroutine(audioManager.PlayAudioList(new List<string> { msg }, () => { Invoke(nameof(Completed), 2f); }));
            return;
        }
        if (animal != null)
        {
            Debug.Log("Gresit");
            string msg = "gresit";
            if (Random.value > 0.5f)
            {
                msg = "gresit 2";
            }
            StartCoroutine(audioManager.PlayAudioList(new List<string> { msg }));
            this.DeselectAll();
        }
    }

    public void SelectAnimal(string value, ClickGameObject currentSc)
    {
        if (value == currentAnimal) return;
        currentAnimal = value;

        if (animal != null) animal.Deselect();

        animal = currentSc;

        if (currentSign == currentAnimal)
        {
            Debug.Log("bun");

            switch (currentAnimal)
            {
                case "fox":
                    cam.position = pozs[3];
                    break;
                case "sheep":
                    cam.position = pozs[1];
                    break;
                case "swan":
                    cam.position = pozs[2];
                    break;
            }
            string msg = "corect";
            if (Random.value > 0.5f)
            {
                msg = "corect 2";
            }
            StartCoroutine(audioManager.PlayAudioList(new List<string> { msg }, () => { Invoke(nameof(Completed), 2f); }));
            return;
        }
        if (sign != null)
        {
            Debug.Log("Gresit");
            string msg = "gresit";
            if (Random.value > 0.5f)
            {
                msg = "gresit 2";
            }
            StartCoroutine(audioManager.PlayAudioList(new List<string> { msg }));
            this.DeselectAll();
        }
    }

 

    public void DeselectAll()
    {
        cam.position = pozs[0];
        if(animal != null)animal.Deselect();
        if (sign != null)sign.Deselect();
        currentAnimal = null;
        currentSign = null;

        animal = null;
        sign = null;
    }

    public void Completed()
    {
        nrAnimals--;
        if (animal != null)
        {
            animal.Deselect();
            animal.gameObject.SetActive(false);
        }
        if (sign != null)
        {
            sign.Deselect();
            //sign.stop = true;
        }
        currentAnimal = null;
        currentSign = null;

        animal = null;
        sign = null;
        cam.position = pozs[0];
        if(nrAnimals <= 0)
        {
            //final

            StartCoroutine(audioManager.PlayAudioList(new List<string> { "final", "final 2" }, () => { MainGameManager.play("Level 9"); }));
            ClickGameObject.stop = true;
            Debug.Log("Final -------------------");
        }
    }

}
