using System;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Vector3 barPoz;
    public Vector3 PuzzlePoz;

    public bool inPlace = false;
    public bool hidden = false;

    public Action<GameObject> removePiece;

    public void setInit(Vector3 barPoz, Vector3 PuzzlePoz, bool hidden)
    {
        this.barPoz = barPoz;
        this.PuzzlePoz = PuzzlePoz;

        inPlace = false;

        this.transform.position = barPoz;
        this.hidden = hidden;

        this.GetComponent<Draggable>().snapPoz = PuzzlePoz;
    }

    public void setBarPoz(Vector3 barPoz)
    {
        if (inPlace)return;

        this.barPoz = barPoz;

        this.transform.position = barPoz;
    }

    public void setVisibility(bool visible)
    {
        if (inPlace) return;

        this.gameObject.SetActive(visible);
    }

    public void setInPlace()
    {
        Debug.Log("In place");
        inPlace = true;

        this.GetComponent<Draggable>().dragOn = false;
        removePiece(this.gameObject);

    }
}
