using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level3 : MonoBehaviour
{
    public int n, m;
    public float barY;
    public float pieceSize = 2.0f;
    public List<GameObject> puzzlePieces = new List<GameObject>();

    public GameObject prefabPiece;

    public int page = 0;

    void Start()
    {
        for (int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                Vector3 poz = new Vector3(j * pieceSize - (m / 2f * pieceSize) + pieceSize / 2, - i * pieceSize + (n / 2f * pieceSize) - pieceSize / 2, 0f);

                GameObject p = Instantiate(prefabPiece, poz, Quaternion.identity);

                float ty = (float)i / (n - 1);
                float tx = (float)j / (m - 1);

                Color c = new Color(tx, ty, 1 - tx);
                p.GetComponent<SpriteRenderer>().color = c;

                p.name = "piece_" + i + "_" + j;

                p.GetComponent<PuzzlePiece>().setInit(Vector3.zero, poz, i > 0);
                p.GetComponent<PuzzlePiece>().removePiece = removePuzzlePiece;

                puzzlePieces.Add(p);

                p.SetActive(false);
            }
        }

        FillOrderRandom();
        switchPage(0);
    }

    public void switchPage(int dir)
    {

        if (dir < -1 || dir > 1) return;

        int nextPage = page + dir;

        if (nextPage < 0) nextPage = puzzlePieces.Count / m;
        if (nextPage * m >= puzzlePieces.Count) nextPage = 0;

        int count = 0;
        for (int i = 0;i < puzzlePieces.Count; i++)
        {

            if (count < m && nextPage * m <= i)
            {
                count++;
                puzzlePieces[i].GetComponent<PuzzlePiece>().setVisibility(true);
                puzzlePieces[i].GetComponent<PuzzlePiece>().setBarPoz(new Vector3((i % m) * (pieceSize + 0.4f) - (m / 2f * pieceSize) + pieceSize / 2 - 1f, barY, 0f));
            }
            else
            {
                puzzlePieces[i].GetComponent<PuzzlePiece>().setVisibility(false);
            }
        }

        page = nextPage;
    }

    public void removePuzzlePiece(GameObject piece)
    {
        puzzlePieces.Remove(piece);
        if (puzzlePieces.Count == 0) levelComplited();
        switchPage(0);
    }

    private void levelComplited()
    {
        Debug.Log("Final nivel---------------------");
    }

    void FillOrderRandom()
    {
        for (int i = puzzlePieces.Count - 1; i > 0; i--)
        {
            int randIndex = Random.Range(0, i + 1);
            GameObject temp = puzzlePieces[i];
            puzzlePieces[i] = puzzlePieces[randIndex];
            puzzlePieces[randIndex] = temp;
        }
    }

}
