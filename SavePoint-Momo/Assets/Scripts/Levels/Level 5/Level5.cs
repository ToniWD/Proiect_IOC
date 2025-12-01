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

    public void choice1()
    {
        Debug.Log("Imbratisare");
        startSecondPhase();
    }

    public void choice2()
    {
        Debug.Log("Jucarie");
        startSecondPhase();
    }


    public void startSecondPhase()
    {
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

        score.text = "Score: " + count;

        if (count == countStop)
        {
            Clickable.startGame = false;
            Debug.Log("Finish---------------------");
        }
    }

    public void error()
    {
        Debug.Log("Fruct gresit");
    }
}
