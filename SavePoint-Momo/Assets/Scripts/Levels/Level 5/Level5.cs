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

    public GameObject final;

    public void choice1()
    {
        Debug.Log("Imbratisare"); 
        part1.SetActive(false);
        Outcome1.SetActive(true);
        Invoke("startSecondPhase", 8f);
    }

    public void choice2()
    {
        Debug.Log("Jucarie");
        part1.SetActive(false);
        Outcome2.SetActive(true);
        Invoke("startSecondPhase", 8f);
    }


    public void startSecondPhase()
    {
        Outcome1.SetActive(false);
        Outcome2.SetActive(false);
        part1.SetActive(false);
        part2.SetActive(true);
        Clickable.startGame = true;
    }

    public void found(int i, GameObject obj)
    {
        if (i != 1)
        {
            error();
            return;
        }
        count++;

        score.text = "Fructe: " + count;

        if (count == countStop)
        {
            Clickable.startGame = false;

            part2.SetActive(false);
            final.SetActive(true);
            Debug.Log("Finish---------------------");
            Invoke("finish", 8f);
        }
    }

    private void finish()
    {
        MainGameManager.play("Menu");
    }

    public void error()
    {
        Debug.Log("Fruct gresit");
    }
}
