using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level3 : MonoBehaviour
{
    public static int count = 5;

    public static void removePuzzlePiece()
    {
        count--;
        if (count == 0) levelComplited();
    }

    private static void levelComplited()
    {
        Debug.Log("Final nivel---------------------");
    }

}
