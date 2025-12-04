using UnityEngine;

public class Clickable : MonoBehaviour
{
    public static bool startGame = false;
    public int cod = 0;
    public bool found = false;
    public Level5 mainSc;

    private void Start()
    {
        mainSc = GameObject.Find("Level5").gameObject.GetComponent<Level5>();
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 0f);
            mat.SetColor("_OutlineColor", Color.black);
        }
    }

    private void OnMouseDown()
    {
        if (!startGame || found) return;

        mainSc.found(cod, this.gameObject);

        if(cod!=1)GetComponent<SpriteRenderer>().color = Color.red;
        else GetComponent<SpriteRenderer>().color = Color.green;

        found = true;
        Debug.Log("Click");
    }

    private void OnMouseEnter()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 2f);
            mat.SetColor("_OutlineColor", Color.green);
        }
    }

    private void OnMouseExit()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 0f);
            mat.SetColor("_OutlineColor", Color.black);
        }
    }
}
