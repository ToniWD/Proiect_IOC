using System;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Vector3 barPoz;

    public bool inPlace = false;

    private void Start()
    {
        this.GetComponent<Draggable>().snapPoz = transform.position;

        this.transform.position = barPoz;
    }

    public void setInPlace()
    {
        Debug.Log("In place");
        inPlace = true;

        Level3.removePuzzlePiece();
        this.GetComponent<Draggable>().dragOn = false;

        this.GetComponent<SpriteRenderer>().sortingOrder -= 1;

    }
}
