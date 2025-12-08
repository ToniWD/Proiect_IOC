using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


//Atentie nu modifica, o poti folosi daca vrei sa poti folosi drag and drop pe un obiect ce are BoxCollider 2D. Scriptul poate fi folosit si in alte nivele dar este creat special pentru nivelul 3
public class Draggable : MonoBehaviour
{
    public bool dragOn = true;
    private bool isDragging = false;
    public bool snap = false;
    public float snapDistance = 0.3f;
    [Header("Events")]
    public UnityEvent doAfterSnap;
    public Vector3 snapPoz;
    private Vector3 offset;

    private void OnMouseEnter()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 7f);
        }
    }

    private void OnMouseExit()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 0f);
        }
    }

    void OnMouseDown()
    {
        if (!dragOn) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePos.x, mousePos.y, transform.position.z);
        isDragging = true;

            this.GetComponent<SpriteRenderer>().sortingOrder += 3;
    }

    void OnMouseDrag()
    {
        if (dragOn && isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 poz = new Vector3(mousePos.x, mousePos.y, transform.position.z) + offset;

            if (snap && Vector3.Distance(poz, snapPoz) <= snapDistance)
            {
                transform.position = snapPoz;
            }
            else
            {
                transform.position = poz;
            }
        }
    }

    void OnMouseUp()
    {
        if (!dragOn) return;

        this.GetComponent<SpriteRenderer>().sortingOrder -= 3;

        if (snap && Vector3.Distance(transform.position, snapPoz) <= snapDistance) doAfterSnap?.Invoke();
        isDragging = false;

    }
}
